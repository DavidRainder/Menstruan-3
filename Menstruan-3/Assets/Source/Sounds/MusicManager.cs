using FMOD;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    #region Singleton

    private static MusicManager instance = null;
    public static MusicManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    FMOD.System _system;
    private ChannelGroup _musicGroup;
    private ChannelGroup _dialogueGroup;
    private Sound _musicSound;
    private Channel _channel;
    bool _soundCreated;

    private string _musicPath = Application.streamingAssetsPath + "/Sounds/Music/";
    
    [SerializeField]
    private string _musicName;

    [SerializeField]
    private float _musicVolume;

    public ChannelGroup GetDialogueGroup()
    {
        return _dialogueGroup;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _soundCreated = false;

        _system = FMODUnity.RuntimeManager.CoreSystem;

        RESULT ret = _system.createChannelGroup("Music", out _musicGroup);
        UnityEngine.Debug.Log(ret);
        ret = _system.createChannelGroup("Dialogue", out _dialogueGroup);
        UnityEngine.Debug.Log(ret);

        DSP dialogueDSP;
        DSP compressorDSP;
        ret = _system.createDSPByType(DSP_TYPE.COMPRESSOR, out compressorDSP);
        UnityEngine.Debug.Log(ret);

        ret = compressorDSP.setParameterFloat((int)DSP_COMPRESSOR.THRESHOLD, -30.0f);
        UnityEngine.Debug.Log(ret);

        DSP_PARAMETER_SIDECHAIN dspSideChain = new DSP_PARAMETER_SIDECHAIN();
        byte[] dspdatabytes = new byte[Marshal.SizeOf(typeof(DSP_PARAMETER_SIDECHAIN))];
        dspdatabytes[0] = 1;
        ret = compressorDSP.setParameterData((int)DSP_COMPRESSOR.USESIDECHAIN, dspdatabytes);
        UnityEngine.Debug.Log(ret);

        ret = _system.createDSPByType(DSP_TYPE.SEND, out dialogueDSP);
        UnityEngine.Debug.Log(ret);

        ret = compressorDSP.addInput(dialogueDSP, out DSPConnection connection, DSPCONNECTION_TYPE.SEND_SIDECHAIN);
        UnityEngine.Debug.Log(ret);

        ret = _musicGroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, compressorDSP);
        UnityEngine.Debug.Log(ret);

        ret = _dialogueGroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, dialogueDSP);
        UnityEngine.Debug.Log(ret);

        ret = _system.playSound(_musicSound, _musicGroup, false, out Channel channel);
        channel.setVolume(_musicVolume);

        // UnityEngine.Debug.Log(ret);
        GameManager.Instance.SetMute();
    }

    public void StopMusic(float fadeOutTime)
    {
        if (_soundCreated)
        {
            StartCoroutine(FadeOut(fadeOutTime));
        }
    }

    public void StartMusic(float fadeInTime)
    {
        if(!_soundCreated)
        {
            StartCoroutine(FadeIn(fadeInTime));
        }
    }

    public void Mute(bool enabled)
    {
        StopAllCoroutines();
        _dialogueGroup.setVolume(enabled ? 0 : 1);
        _musicGroup.setVolume(enabled ? 0 : _musicVolume);
    }

    IEnumerator FadeIn(float fadeInTime)
    {
        _soundCreated = true;
        _system.createSound(_musicPath + _musicName, MODE._2D | MODE.LOOP_NORMAL | MODE.CREATESAMPLE | MODE.LOWMEM, out _musicSound);
        _system.playSound(_musicSound, _musicGroup, false, out _channel);
        _channel.setVolume(0);
        float volume = 0;
        float time = 0;
        while(volume < _musicVolume)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            volume = _musicVolume * Mathf.Sqrt(time / fadeInTime);
            _channel.setVolume(_musicVolume);
        }
        volume = _musicVolume;
        _channel.setVolume(volume);
    }

    IEnumerator FadeOut(float fadeOutTime)
    {
        float volume = _musicVolume;
        float time = 0;
        while (volume > 0)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            float sqrt = (fadeOutTime - time) / fadeOutTime;
            Mathf.Clamp(sqrt, 0, 1);
            volume = _musicVolume * Mathf.Sqrt(sqrt);
            _channel.setVolume(volume);
        }
        volume = 0;
        _channel.setVolume(volume);
        _musicSound.release();
        _soundCreated = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        _musicSound.release();
    }
}
