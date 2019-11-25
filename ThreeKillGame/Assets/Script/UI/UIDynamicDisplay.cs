using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDynamicDisplay : MonoBehaviour
{
    [SerializeField]
    private Transform parentTf;
    [SerializeField]
    private string description_text;    //说明文字，或前缀
    [SerializeField]
    private int index;  //定位上阵位0或备战位1

    // Start is called before the first frame update
    void Start()
    {
        ChangeNumOfPeople();
    }

    /// <summary>
    /// 刷新 上阵0/0 和 备战0/0 的值
    /// </summary>
    public void ChangeNumOfPeople()
    {
        int sum = 0, own_car = 0;
        switch (index)
        {
            //上阵位
            case 0:
                sum = CreateAndUpdate.battleNum;
                break;
            //备战位
            case 1:
                sum = CreateAndUpdate.prepareNum;
                break;
        }
        //sum = parentTf.childCount;
        for (int i = 0; i < parentTf.childCount; i++)
        {
            if (parentTf.GetChild(i).childCount > 0)
                own_car++;
        }
        transform.GetComponent<Text>().text = description_text + own_car + "/" + sum;
    }
}
