using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuggingFace.API;
using UnityEngine.UI;
using TMPro;
using LMNT;
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
            Debug.Log("Label: "+text.text);

            Debug.Log("Speech: "+speech.dialogue);
            
            
            
        }, error => {
            text.color = Color.red;
            text.text = error;
           
        });             
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
