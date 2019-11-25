using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour {

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