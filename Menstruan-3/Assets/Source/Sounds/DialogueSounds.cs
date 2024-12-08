using FMOD;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueSounds : MonoBehaviour
{
    FMOD.System _system;

    private int _maxLetters = 0;
    private string _lettersPath = Application.streamingAssetsPath + "/Sounds/Letters/";
    private Dictionary<string, Sound> _soundsDict = null;

    private Transform _soundTransform;

    [SerializeField]
    private float _distanceFactorX = 2.0f;
    [SerializeField]
    private float _distanceZ = 2.0f;

    private ChannelGroup _dialogueGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (_soundsDict != null) return;
        _system = FMODUnity.RuntimeManager.CoreSystem;
        VECTOR pos, vel, foward, up;
        pos.x = 0; pos.y = 0; pos.z = -_distanceZ;
        vel.x = 0; vel.y = 0; vel.z = 0;
        foward.x = 0; foward.y = 0; foward.z = 1;
        up.x = 0; up.y = 1; up.z = 0;
        _system.set3DListenerAttributes(0, ref pos, ref vel, ref foward, ref up);
        _soundsDict = new Dictionary<string, Sound>();
        DirectoryInfo info = new DirectoryInfo(_lettersPath);
        FileInfo[] files = info.GetFiles();

        _dialogueGroup = MusicManager.Instance.GetDialogueGroup();

        MODE mode = MODE._3D | MODE.LOOP_OFF | MODE.CREATESAMPLE | MODE.LOWMEM;
        Sound sound;
        foreach (FileInfo file in files)
        {
            string name = file.Name.Split(".")[0];
            if (!_soundsDict.ContainsKey(name))
            {
                string fullPath = _lettersPath + file.Name;
                RESULT ret = _system.createSound(fullPath, mode, out sound);
                if (ret != RESULT.OK)
                {
                    UnityEngine.Debug.LogError("ERROR: " + FMOD.Error.String(ret));
                    return;
                }
                _soundsDict.Add(name, sound);
                _maxLetters++;
            }
        }
    }

    public void SetSoundTransform(Transform soundTransform)
    {
        _soundTransform = soundTransform;
    }

    public void playLetterSound(char letter)
    {
        if (_soundsDict.ContainsKey(letter.ToString()))
        {
            _system.playSound(_soundsDict[letter.ToString()], _dialogueGroup, false, out Channel channel);
            if(_soundTransform != null)
            {
                VECTOR v = new VECTOR();
                v.x = _soundTransform.position.x * _distanceFactorX;
                v.y = _soundTransform.position.y;
                v.z = _soundTransform.position.z;
                VECTOR vel = new VECTOR();
                vel.x = 0; vel.y = 0; vel.z = 0;
                RESULT ret = channel.set3DAttributes(ref v, ref vel);
                VECTOR orientation = new VECTOR();
                orientation.x = 0; orientation.y = 0; orientation.z = 1;
                channel.set3DConeOrientation(ref orientation);
                channel.set3DConeSettings(120, 180, 1);
            }
        }
    }
}
