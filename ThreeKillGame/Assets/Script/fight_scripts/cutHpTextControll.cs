using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutHpTextControll : MonoBehaviour
{
    Text cutHpText;

    private float addPosY = 15;

    private Sequence textMoveSequence;  //DOTween队列

    private void Start()
    {
        cutHpText = GetComponent<Text>();
        TextMoved(cutHpText);

        int count = transform.parent.childCount;
        Debug.Log("count: " + count);
        if (count>0)
        {
            textMoveSequence.Play();
            Invoke("DestortThisText", FightControll.speedTime * 1.6f);  //销毁
        }
        else
        {
            Invoke("DelayShowText", FightControll.speedTime * 1f);
            Invoke("DestortThisText", FightControll.speedTime * 2.6f);  //销毁
        }

    }

    /// <summary>
    /// 延迟销毁
    /// </summary>
    private void DestortThisText()
    {
        Destroy(transform.gameObject);
    }

    /// <summary>
    /// 延迟显示
    /// </summary>
    private void DelayShowText()
    {
        textMoveSequence.Play();
    }

    private void TextMoved(Graphic graphic)
    {

        RectTransform rect = graphic.rectTransform;

        Color color = graphic.color;

        graphic.color = new Color(color.r, color.g, color.b, 0);

        //设置DOTween队列

        textMoveSequence = DOTween.Sequence();

        //设置Text移动和透明度的变化值

        Tweener textMove01 = rect.DOMoveY(rect.position.y + addPosY, FightControll.speedTime * 0.3f);

        Tweener textMove02 = rect.DOMoveY(rect.position.y + addPosY, FightControll.speedTime * 0.3f);

        Tweener textColor01 = graphic.DOColor(new Color(color.r, color.g, color.b, 1), FightControll.speedTime * 0.3f);

        Tweener textColor02 = graphic.DOColor(new Color(color.r, color.g, color.b, 0), FightControll.speedTime * 0.3f);

        //Append 追加一个队列，Join 添加一个队列

        //中间间隔

        //Append 再追加一个队列，再Join 添加一个队列

        textMoveSequence.Append(textMove01);
        textMoveSequence.Join(textColor01);

        textMoveSequence.AppendInterval(FightControll.speedTime * 1f);

        textMoveSequence.Append(textMove02);
        textMoveSequence.Join(textColor02);

        textMoveSequence.Pause();
    }
}