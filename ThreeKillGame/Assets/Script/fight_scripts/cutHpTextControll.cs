using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutHpTextControll : MonoBehaviour
{
    Text cutHpText;

    private void Start()
    {
        cutHpText = GetComponent<Text>();
        TextMoved(cutHpText);
        //Invoke("HideWidget", FightControll.speedTime * 1.5f);
    }
    

    /// <summary>
    /// 隐藏自身
    /// </summary>
    private void HideWidget()
    {
        gameObject.SetActive(false);
    }

    private void TextMoved(Graphic graphic)
    {

        //获得Text的rectTransform，和颜色，并设置颜色微透明

        RectTransform rect = graphic.rectTransform;

        Color color = graphic.color;

        graphic.color = new Color(color.r, color.g, color.b, 0);

        //设置一个DOTween队列

        Sequence textMoveSequence = DOTween.Sequence();

        //设置Text移动和透明度的变化值

        Tweener textMove01 = rect.DOMoveY(rect.position.y + 10, FightControll.speedTime * 0.3f);

        Tweener textMove02 = rect.DOMoveY(rect.position.y + 20, FightControll.speedTime * 0.1f);

        Tweener textColor01 = graphic.DOColor(new Color(color.r, color.g, color.b, 1), FightControll.speedTime * 0.3f);

        Tweener textColor02 = graphic.DOColor(new Color(color.r, color.g, color.b, 0), FightControll.speedTime * 0.1f);

        //Append 追加一个队列，Join 添加一个队列

        //中间间隔一秒

        //Append 再追加一个队列，再Join 添加一个队列

        textMoveSequence.Append(textMove01);

        textMoveSequence.Join(textColor01);

        textMoveSequence.AppendInterval(1);

        textMoveSequence.Append(textMove02);

        textMoveSequence.Join(textColor02);
    }
}