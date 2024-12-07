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

    private ChannelGroup _dialogueGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (_soundsDict != null) return;
        _system = FMODUnity.RuntimeManager.CoreSystem;
        _soundsDict = new Dictionary<string, Sound>();
        DirectoryInfo info = new DirectoryInfo(_lettersPath);
        FileInfo[] files = info.GetFiles();

        _dialogueGroup = MusicManager.Instance.GetDialogueGroup();

        MODE mode = MODE._2D | MODE.LOOP_OFF | MODE.CREATESAMPLE | MODE.LOWMEM;
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

    public void playLetterSound(char letter)
    {
        if (_soundsDict.ContainsKey(letter.ToString()))
        {
            _system.playSound(_soundsDict[letter.ToString()], _dialogueGroup, false, out Channel channel);
        }
    }
}
