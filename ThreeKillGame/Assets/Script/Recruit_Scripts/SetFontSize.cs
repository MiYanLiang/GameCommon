using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetFontSize : MonoBehaviour
{
    // Start is called before the first frame update
    string txt;
    void Start()
    {

    }
    void Update()
    {
        txt = this.GetComponent<Text>().text;
        if (txt.Length > 2)
        {
            this.GetComponent<Text>().fontSize = 40;
        }
        else
        {
            this.GetComponent<Text>().fontSize = 50;
        }
    }
}
