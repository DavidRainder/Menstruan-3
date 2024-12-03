using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public enum Feelings
{
    NEUTRAL, 
    ANGRY, 
    HAPPY,
    SAD,
    NUM_FEELINGS
}


public class ShowDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _endTextPointer;

    private bool _textEnded;

    DialogSettings _settings;

    int _currentText;
    public void SetSettings(DialogSettings settings) { this._settings = settings; }

    FMOD.System _system;

    private int _maxLetters = 0;
    private string _path = Application.streamingAssetsPath + "/Sounds/Letters/";
    private Dictionary<string, Sound> _soundsDict;

    private ChannelGroup[] _channelGroups;
    private ChannelGroup _dialogueGroup;
    
    [SerializeField]
    private float[] _speedFactors;

    [SerializeField]
    private Feelings _feeling;

    private void Start()
    {
        _system = FMODUnity.RuntimeManager.CoreSystem;

        _channelGroups = new ChannelGroup[(int)Feelings.NUM_FEELINGS];
        
        _soundsDict = new Dictionary<string, Sound>();
        DirectoryInfo info = new DirectoryInfo(_path);
        FileInfo[] files = info.GetFiles();

        _system.createChannelGroup("Dialogue", out _dialogueGroup);

        DSP dsp;
        for (int i = 0; i < (int)Feelings.NUM_FEELINGS; i++)
        {
            DSP_TYPE type;
            switch ((Feelings)i)
            {
                case Feelings.NEUTRAL:
                    _system.createChannelGroup("Dialogue_Neutral", out _channelGroups[i]);
                    _channelGroups[i].addGroup(_dialogueGroup);
                    break;
                case Feelings.ANGRY:
                    _system.createChannelGroup("Dialogue_Angry", out _channelGroups[i]);
                    _channelGroups[i].addGroup(_dialogueGroup);

                    type = DSP_TYPE.COMPRESSOR;
                    _system.createDSPByType(type, out dsp);
                    //_dsps[i][0].setParameterInt();
                    _channelGroups[i].addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, dsp);

                    type = DSP_TYPE.MULTIBAND_EQ;
                    _system.createDSPByType(type, out dsp);
                    dsp.setParameterInt((int)DSP_MULTIBAND_EQ.A_FILTER, (int)DSP_MULTIBAND_EQ_FILTER_TYPE.HIGHPASS_24DB);
                    dsp.setParameterFloat((int)DSP_MULTIBAND_EQ.A_FREQUENCY, 2000);
                    dsp.setParameterFloat((int)DSP_MULTIBAND_EQ.A_GAIN, 20);
                    dsp.setWetDryMix(1, 1, 0);
                    _channelGroups[i].addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, dsp);
                    break;
                case Feelings.HAPPY:
                    _system.createChannelGroup("Dialogue_Happy", out _channelGroups[i]);
                    _channelGroups[i].addGroup(_dialogueGroup);

                    type = DSP_TYPE.MULTIBAND_EQ;
                    _system.createDSPByType(type, out dsp);
                    dsp.setParameterInt((int)DSP_MULTIBAND_EQ.A_FILTER, (int)DSP_MULTIBAND_EQ_FILTER_TYPE.HIGHPASS_24DB);
                    dsp.setParameterFloat((int)DSP_MULTIBAND_EQ.A_FREQUENCY, 4000);
                    dsp.setParameterFloat((int)DSP_MULTIBAND_EQ.A_GAIN, -20);
                    dsp.setWetDryMix(1, 1, 0);
                    _channelGroups[i].addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, dsp);
                    break;
                case Feelings.SAD:
                    _system.createChannelGroup("Dialogue_Sad", out _channelGroups[i]);
                    _channelGroups[i].addGroup(_dialogueGroup);

                    type = DSP_TYPE.MULTIBAND_EQ;
                    _system.createDSPByType(type, out dsp);
                    dsp.setParameterInt((int)DSP_MULTIBAND_EQ.A_FILTER, (int)DSP_MULTIBAND_EQ_FILTER_TYPE.LOWPASS_24DB);
                    dsp.setParameterFloat((int)DSP_MULTIBAND_EQ.A_FREQUENCY, 2000);
                    dsp.setParameterFloat((int)DSP_MULTIBAND_EQ.A_GAIN, -20);
                    dsp.setWetDryMix(1, 1, 0);
                    _channelGroups[i].addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, dsp);
                    break;
            }
        }

        foreach (FileInfo file in files)
        {
            string name = file.Name.Split(".")[0];
            if (!_soundsDict.ContainsKey(name))
            {
                RESULT ret = _system.createSound(_path + file.Name, MODE._2D | MODE.LOOP_OFF | MODE.CREATESAMPLE | MODE.LOWMEM, out Sound sound);
                if (ret != RESULT.OK)
                {
                    UnityEngine.Debug.LogError("ERROR: " + ret);
                    return;
                }
                _soundsDict.Add(name, sound);
                _maxLetters++;
            }
        }

        ShowText();
    }

    private void OnDestroy()
    {
        StopCoroutine("AnimText");
    }

    public void Update()
    {
        if (_textEnded && Input.GetMouseButtonUp(0))
        {
            ShowText();
        }
    }

    private void StartDialog()
    {
        _settings.onStartDialog.Invoke();   
    }

    private void EndDialog()
    {
        _settings.onFinishDialog.Invoke();
        Destroy(gameObject);
    }

    public void ShowText()
    {
        if(_currentText == 0)
        {
            StartDialog();
        }
        if(_currentText >= _settings.texts.Count)
        {
            EndDialog();
        }
        else
        {
            int initialIndex = 0;
            StartCoroutine(AnimText(initialIndex, _settings.texts[_currentText++], _feeling));
        }
    }

    IEnumerator AnimText(int initialIndex, string messageToShow, Feelings feelings)
    {
        _textEnded = false;
        _endTextPointer.SetActive(_textEnded);
        messageToShow = StringManager.Instance.GetGenderStringByKey(messageToShow);

        if (initialIndex < messageToShow.Length && initialIndex >= 0)
        {
            int index = initialIndex;
            int length = messageToShow.Length;

            while (++index <= length)
            {
                string dialog = messageToShow.Substring(initialIndex, index - initialIndex);
                _text.text = dialog;
                char currentLetter = dialog[dialog.Length - 1];

                if (_soundsDict.ContainsKey(currentLetter.ToString()))
                {
                    _system.playSound(_soundsDict[currentLetter.ToString()], _channelGroups[(int)feelings], false, out Channel channel);
                }


                yield return new WaitForSeconds(_settings.speed * _speedFactors[(int)feelings]);
            }
        }
        _textEnded = true;
        _endTextPointer.SetActive(_textEnded);
    }
}