using FMOD;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
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

    private DSP[][] _dsps;

    private void Start()
    {
        _dsps = new DSP[(int)Feelings.NUM_FEELINGS][];

        _system = FMODUnity.RuntimeManager.CoreSystem;
        _soundsDict = new Dictionary<string, Sound>();
        DirectoryInfo info = new DirectoryInfo(_path);
        FileInfo[] files = info.GetFiles();

        for(int i = 0; i < (int)Feelings.NUM_FEELINGS; i++)
        {
            DSP_TYPE type;
            switch ((Feelings)i)
            {
                case Feelings.ANGRY:
                    _dsps[i] = new DSP[2];
                    
                    type = DSP_TYPE.COMPRESSOR;
                    _system.createDSPByType(type, out _dsps[i][0]);
                    //_dsps[i][0].setParameterInt();

                    type = DSP_TYPE.MULTIBAND_EQ;
                    _system.createDSPByType(type, out _dsps[i][1]);
                    break;
                case Feelings.HAPPY:
                    _dsps[i] = new DSP[1];

                    type = DSP_TYPE.MULTIBAND_EQ;
                    _system.createDSPByType(type, out _dsps[i][0]);
                    break;
                case Feelings.SAD:
                    _dsps[i] = new DSP[1];

                    type = DSP_TYPE.MULTIBAND_EQ;
                    _system.createDSPByType(type, out _dsps[i][0]);
                    _dsps[i][0].setParameterFloat((int)DSP_MULTIBAND_EQ.A_FILTER, (int)DSP_MULTIBAND_EQ_FILTER_TYPE.LOWPASS_24DB);
                    _dsps[i][0].setParameterFloat((int)DSP_MULTIBAND_EQ.A_FREQUENCY, (int)10);
                    _dsps[i][0].setParameterFloat((int)DSP_MULTIBAND_EQ.A_GAIN, -20);
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
            StartCoroutine(AnimText(initialIndex, _settings.texts[_currentText++], Feelings.SAD));
        }
    }

    IEnumerator AnimText(int initialIndex, string messageToShow, Feelings feelings)
    {
        _system.getMasterChannelGroup(out ChannelGroup channelgroup);

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
                    _system.playSound(_soundsDict[currentLetter.ToString()], channelgroup, false, out Channel channel);
                    int size = _dsps[(int)feelings].Length;
                    for (int i = 0; i < size; ++i)
                    {
                        channel.addDSP(i, _dsps[(int)feelings][i]);
                        //_system.playDSP(_dsps[(int)feelings][i], channelgroup, false, channel);
                    }
                }


                yield return new WaitForSeconds(_settings.speed);
            }
        }
        _textEnded = true;
        _endTextPointer.SetActive(_textEnded);
    }
}