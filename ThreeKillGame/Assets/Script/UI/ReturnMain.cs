using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMain : MonoBehaviour
{
    public void ClickReturnMain()
    {
        PlayerPrefs.SetInt("prestigeNum", 200);
        SceneManager.LoadScene(0);
    }
}