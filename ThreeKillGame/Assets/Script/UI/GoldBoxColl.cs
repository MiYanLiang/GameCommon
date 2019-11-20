using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBoxColl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GoldBox;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickBox()
    {
        GoldBox.SetActive(true);
    }
}
