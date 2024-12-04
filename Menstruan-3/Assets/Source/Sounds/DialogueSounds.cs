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
        _soundsDict = new Dictionary<string, Sound>();
        DirectoryInfo info = new DirectoryInfo(_lettersPath);
        FileInfo[] files = info.GetFiles();

        _dialogueGroup = MusicManager.Instance.getDialogueGroup();

        foreach (FileInfo file in files)
        {
            string name = file.Name.Split(".")[0];
            if (!_soundsDict.ContainsKey(name))
            {
                RESULT ret = _system.createSound(_lettersPath + file.Name, MODE._2D | MODE.LOOP_OFF | MODE.CREATESAMPLE | MODE.LOWMEM, out Sound sound);
                if (ret != RESULT.OK)
                {
                    UnityEngine.Debug.LogError("ERROR: " + ret);
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
