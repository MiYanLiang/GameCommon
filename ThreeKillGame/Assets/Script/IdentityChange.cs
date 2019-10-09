using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdentityChange : MonoBehaviour {

    public GameObject btnText;
    public GameObject identityText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //身份改变
    public void IdentityChange1()
    {
        identityText.GetComponent<Text>().text = btnText.GetComponent<Text>().text;
    }

}
