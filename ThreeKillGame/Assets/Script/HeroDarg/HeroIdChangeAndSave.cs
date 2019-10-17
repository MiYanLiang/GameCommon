using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroIdChangeAndSave : MonoBehaviour
{
    public static int[] pos_heroId = new int[16];     //记录九宫格和备战区的英雄id

    private void Awake()
    {
        for (int i = 0; i < pos_heroId.Length; i++)
        {
            pos_heroId[i] = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
