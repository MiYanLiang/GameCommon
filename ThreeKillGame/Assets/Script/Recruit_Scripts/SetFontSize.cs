using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetFontSize : MonoBehaviour
{
    Text thisText;

    private void Awake()
    {
        thisText = GetComponent<Text>();
        //Invoke("UpdateFontSize", 0.05f);
    }

    private void Start()
    {
        UpdateFontSize();
    }

    public void UpdateFontSize()
    {
        if (thisText.text.Length > 2)
        {
            thisText.fontSize = 40;
        }
        else
        {
            thisText.fontSize = 50;
        }
    }
}