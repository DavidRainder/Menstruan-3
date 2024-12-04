using FMOD;
using System.Collections.Generic;
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

    public ChannelGroup getDialogueGroup()
    {
        return _dialogueGroup;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _system = FMODUnity.RuntimeManager.CoreSystem;
        RESULT ret = _system.createSound(_musicPath + _musicName, MODE._2D | MODE.LOOP_NORMAL | MODE.CREATESAMPLE | MODE.LOWMEM, out _musicSound);
        _system.createChannelGroup("Music", out _musicGroup);
        _system.createChannelGroup("Dialogue", out _dialogueGroup);

        DSP dialogueDSP;
        DSP compressorDSP;
        ret = _system.createDSPByType(DSP_TYPE.COMPRESSOR, out compressorDSP);
        compressorDSP.setParameterFloat((int)DSP_COMPRESSOR.THRESHOLD, -60.0f);
        compressorDSP.setParameterBool((int)DSP_COMPRESSOR.USESIDECHAIN, true);

        ret = _system.createDSPByType(DSP_TYPE.SEND, out dialogueDSP);
        compressorDSP.addInput(dialogueDSP, out DSPConnection connection, DSPCONNECTION_TYPE.SEND_SIDECHAIN);
        _musicGroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, compressorDSP);
        _dialogueGroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, dialogueDSP);

        _system.playSound(_musicSound, _musicGroup, false, out Channel channel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
