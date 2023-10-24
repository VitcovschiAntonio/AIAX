using System.IO;
using HuggingFace.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechRecognitionTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private AudioClip clip;
    private byte[] bytes;
    private bool recording;

    //define states
    public enum RecordingState
    {
        Idle,
        Recording,
        Sending
    }

    private RecordingState currentState = RecordingState.Idle;

    private void Start()
    {
        //initialize the AI assistant with an initial state
        currentState = RecordingState.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case RecordingState.Idle:
                //handle behavior for the "Idle" state
                
                break;

            case RecordingState.Recording:
                //handle behavior for the "Recording" state
               
                if (Microphone.GetPosition(null) >= clip.samples)
                {
                    StopRecording();
                }
                break;

            case RecordingState.Sending:
                //handle behavior for the "Sending" state
                
                SendRecording();
                break;

                //add more states and their respective behaviors as needed.
        }
    }

    private void StartRecording()
    {
        //transition to the "Recording" state
        currentState = RecordingState.Recording;

        //implement state-specific behavior here
        text.color = Color.white;
        text.text = "Recording...";

        clip = Microphone.Start(null, false, 10, 44100);
        recording = true;
    }

    private void StopRecording()
    {
        //transition to the "Sending" state
        currentState = RecordingState.Sending;

        //implement state-specific behavior here
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);

        //send the recording here and handle the response
        SendRecording();
    }

    private void SendRecording()
    {
        //implement state-specific behavior here
        text.color = Color.yellow;
        text.text = "Sending...";

        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response =>
        {
            text.color = Color.white;
            text.text = response;
            currentState = RecordingState.Idle; //transition back to Idle
        }, error =>
        {
            text.color = Color.red;
            text.text = error;
            currentState = RecordingState.Idle; //transition back to Idle
        });
    }

    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
    {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
        {
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
}
