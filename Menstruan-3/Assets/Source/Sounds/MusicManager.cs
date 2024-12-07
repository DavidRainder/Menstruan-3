using FMOD;
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

    private string _musicPath = Application.streamingAssetsPath + "/Sounds/Music/";
    
    [SerializeField]
    private string _musicName;

    public ChannelGroup GetDialogueGroup()
    {
        return _dialogueGroup;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _system = FMODUnity.RuntimeManager.CoreSystem;
        RESULT ret = _system.createSound(_musicPath + _musicName, MODE._2D | MODE.LOOP_NORMAL | MODE.CREATESAMPLE | MODE.LOWMEM, out _musicSound);
        UnityEngine.Debug.Log(ret);

        ret = _system.createChannelGroup("Music", out _musicGroup);
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
        UnityEngine.Debug.Log(ret);
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
