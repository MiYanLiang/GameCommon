using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlayer : MonoBehaviour
{
    public AudioSource[] AudioSources;//要播放的音频列表
    public float IntervalSec = 10f;//间隔秒数

    private AudioSource _currentAudio;//当前播放
    private float _timer;//秒表

    private void RandomPlay()
    {
        var random = Random.Range(0, AudioSources.Length);//根据音频数量来获取随机值
        _currentAudio = AudioSources[random];//设置当前音频
        _currentAudio.Play();//播放
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentAudio !=null)
        {
            if (_currentAudio.isPlaying) return;//如果正在播放时，不执行
            //如果播放停止
            _timer += Time.deltaTime;//记录间隔时间
            if (_timer < IntervalSec) return;//如果还没超过预设的间隔时间，不执行
        }
        _timer = 0;//秒表清零
        RandomPlay();//随机播放
    }
}
