using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetBestTIme : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        text.text = ("Best  time: "+ PlayerPrefs.GetFloat("BestScore", 0f).ToString("F2") + " s");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
