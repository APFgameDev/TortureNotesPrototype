using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

[RequireComponent(typeof(AudioSource))]
public class TryRecord : MonoBehaviour {

    public int m_recordSeconds = 10;

    AudioSource audioSource;
    DictationRecognizer dictationRecognizer;
    PhraseRecognizer phraseRecognizer;
    string microphone;

    [SerializeField]
    UnityEngine.UI.Text RecordButtonText;
    [SerializeField]
    UnityEngine.UI.Text RecordText;

    [SerializeField]

    UnityEngine.UI.Text dictationText;

    private void Start()
    {
        dictationRecognizer = new DictationRecognizer(ConfidenceLevel.Low);
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

        audioSource = GetComponent<AudioSource>();

        foreach (string device in Microphone.devices)
        {
            microphone = device;
        }

        RecordText.text = "Record Audio";

        RecordText.text = "";
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        dictationText.text = text;
    }

    public void RecordAudio()
    {
        if (!Microphone.IsRecording(microphone))
        {
            audioSource.clip = Microphone.Start(microphone, false, m_recordSeconds, 44100);
            StartCoroutine(WaitForEndRecord());
        }
        else
        {
            Microphone.End(microphone);
        }
    }

    IEnumerator WaitForEndRecord()
    {
        RecordButtonText.text = "Stop Record";
        dictationRecognizer.Start();

        while (Microphone.IsRecording(microphone))
        {
            yield return new WaitForEndOfFrame();

            RecordText.text = "Recording Time Remaining = " + (m_recordSeconds - Microphone.GetPosition(microphone) / 44100.0f);


        }

        RecordButtonText.text = "Record Audio";
    }

    public void PlayRecordedAudio()
    {
        audioSource.Play();
    }

    private void OnDestroy()
    {
        dictationRecognizer.Dispose();
    }

    public void SaveClip()
    {
        SavWav.Save("Test", audioSource.clip);
    }
}
