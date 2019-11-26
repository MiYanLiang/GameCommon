using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickReturnMain()
    {
        PlayerPrefs.SetInt("prestigeNum",200);
        SceneManager.LoadScene(0);
    }
}
