using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDynamicDisplay : MonoBehaviour
{
    [SerializeField]
    private Transform parentTf;
    [SerializeField]
    private string description_text;    //  说明文字，或前缀

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
        int sum, own_car=0;
        sum = parentTf.childCount;
        for (int i = 0; i < sum; i++)
        {
            if (parentTf.GetChild(i).childCount > 0)
                own_car++;
        }
        transform.GetComponent<Text>().text = description_text + own_car + "/" + sum;

    }

}
