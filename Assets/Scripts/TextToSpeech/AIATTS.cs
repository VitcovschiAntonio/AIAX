using System.Collections;
using System.Collections.Generic;
using LMNT;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class AIATTS : LMNTSpeech
{
    [SerializeField] private TextMeshProUGUI _STT_response;
    [SerializeField] private Button speakButton;
    // Start is called before the first frame update
    void Start()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
