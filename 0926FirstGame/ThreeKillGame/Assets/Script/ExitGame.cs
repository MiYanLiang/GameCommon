using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //退出游戏
    public void ExitGame1()
    {
        Application.Quit();
    }
    //重新开始
    public void NextGame()
    {
        SceneManager.LoadScene(0);
    }
    //暂停游戏
    public void SuspendedGame()
    {
        Time.timeScale = 0;
    }
    //继续游戏
    public void ContinueGame()
    {
        Time.timeScale = 1;
    }
}
