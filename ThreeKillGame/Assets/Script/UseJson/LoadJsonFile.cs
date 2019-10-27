using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using LitJson;
using System.Reflection;
using System.Data;

public class LoadJsonFile : MonoBehaviour
{
    //Resources文件夹下
    public static readonly string Folder = "Jsons/";

    private void Start()
    {
        //string data = LoadJsonByName("LevelTable");
        //Roots root = JsonMapper.ToObject<Roots>(data);
        //Debug.Log("LevelTable: "+root.LevelTable[0].prepareNum);
        //string data1 = LoadJsonByName("DifficultyChoose");
        //root = JsonMapper.ToObject<Roots>(data1);
        //Debug.Log("DifficultyChoose: " + root.DifficultyChoose[0].difficulty);
    }

    private void Awake()
    {
        JsonDataToSheets();
    }
    /// <summary>
    /// 经验等级表数据
    /// </summary>
    public static List<List<string>> levelTableDatas;

    private void JsonDataToSheets()
    {
        string jsonData = LoadJsonByName("LevelTable");
        Roots root = JsonMapper.ToObject<Roots>(jsonData);
        Debug.Log("加载成功 :"+ "LevelTable"+ ".Json");
        levelTableDatas = new List<List<string>>(root.LevelTable.Count);
        for (int i = 0; i < root.LevelTable.Count; i++)
        {
            levelTableDatas.Add(new List<string>(6));
            levelTableDatas[i].Add(root.LevelTable[i].index);
            levelTableDatas[i].Add(root.LevelTable[i].level);
            levelTableDatas[i].Add(root.LevelTable[i].experience);
            levelTableDatas[i].Add(root.LevelTable[i].money);
            levelTableDatas[i].Add(root.LevelTable[i].battleNum);
            levelTableDatas[i].Add(root.LevelTable[i].prepareNum);
        }
    }


    /// <summary>
    /// 通过json文件名获取json数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string LoadJsonByName(string fileName)
    {
        string path = string.Empty;
        string data = string.Empty;
        if (Application.isPlaying)
        {
            path = Path.Combine(Folder, fileName);  //合并文件路径
            var asset = Resources.Load<TextAsset>(path);
            Debug.Log("Loading JsonFile " + fileName + " from: " + path);
            if (asset == null)
            {
                Debug.LogError("No text asset could be found at resource path: " + path);
                return null;
            }
            data = asset.text;
            Resources.UnloadAsset(asset);
        }
        else
        {
#if UNITY_EDITOR
            path = Application.dataPath + "/Resources/" + Folder + "/" + fileName + ".json";
            Debug.Log("Loading JsonFile " + fileName + " from: " + path);
            var asset1 = System.IO.File.ReadAllText(path);
            data = asset1;
#endif
        }
        return data;
    }


    /// <summary>
    /// 通过StreamReader读取json,json存在StreamingAssets文件夹下
    /// </summary>
    public JsonReader LoadJsonUseStreamReader(string fileName)
    {
        StreamReader streamreader = new StreamReader(Application.dataPath + "/StreamingAssets/Jsons/"+ fileName + ".json");  //读取数据，转换成数据流
        JsonReader js = new JsonReader(streamreader);   //再转换成json数据
        //Root r = JsonMapper.ToObject<Root>(js);     //读取
        //for (int i = 0; i < r.LevelTable.Count; i++)  //遍历获取数据
        //{
        //    textone.text += r.LevelTable[i].experience + "   ";
        //}
        streamreader.Close();
        return js;
    }

    /// <summary>
    /// 通过WWW方法读取Json数据，json存在StreamingAssets文件夹下
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public string LoadJsonUseWWW(string fileName)
    {
        string localPath = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
        {
            //localPath = Application.streamingAssetsPath + "/" + path;
            localPath = "jar:file://" + Application.dataPath + "!/assets/" + fileName + ".json";
        }
        else
        {
            localPath = "file:///" + Application.streamingAssetsPath + "/" + fileName + ".json";
        }
        WWW www = new WWW(localPath);     //格式必须是"ANSI"，不能是"UTF-8"
        if (www.error != null)
        {
            Debug.LogError("error : " + localPath);
            return null;          //读取文件出错
        }
        return www.text;
    }

}
