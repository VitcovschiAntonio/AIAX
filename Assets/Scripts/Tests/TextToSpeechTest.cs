using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuggingFace.API;
using UnityEngine.UI;
using TMPro;
using LMNT;
using System;
using System.Reflection;
public class TextToSpeechTest : MonoBehaviour
{
     [SerializeField] private TextMeshProUGUI text;
      [SerializeField] private Button speakButton;
      [SerializeField] private LMNTSpeech speech;
    // Start is called before the first frame update
    private void StartSpeaking()
    {
        HuggingFaceAPI.Conversation(text.text,response => {
            text.color = Color.white;
            text.text = response;
            speech.dialogue = text.text;
            StartCoroutine(speech.Prefetch());
            StartCoroutine(WaitForPrefetchAndTalk());
            Debug.Log("Label: 1 "+text.text);

            Debug.Log("Speech: 1 "+speech.dialogue);
            Assembly assembly = Assembly.GetAssembly (typeof(UnityEditor.SceneView));
            Type logEntries = assembly.GetType ("UnityEditor.LogEntries");
            logEntries.GetMethod ("Clear");
            
            
            
        }, error => {
            text.color = Color.red;
            text.text = error;
           
        });  
         Debug.Log("Label 2: "+text.text);

            Debug.Log("Speech 2: "+speech.dialogue);           
    }
    private IEnumerator WaitForPrefetchAndTalk()
{
    yield return StartCoroutine(speech.Prefetch());
    StartCoroutine(speech.Talk());
}

    void Start()
    {
        speakButton.onClick.AddListener(StartSpeaking);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
