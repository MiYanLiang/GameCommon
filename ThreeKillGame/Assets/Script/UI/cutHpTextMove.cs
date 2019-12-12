using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class cutHpTextMove : MonoBehaviour
{
    Vector3 vec;    //记录初始位置

    Text cutHpText;

    public string content_text; //展示内容

    private float addPosY = 10;

    private Sequence textMoveSequence;  //DOTween队列

    private void Awake()
    {
        content_text = "-2";
        cutHpText = GetComponent<Text>();
        cutHpText.text = content_text;

        //TextMoved(cutHpText);
    }

    private void Start()
    {
        vec = cutHpText.transform.position;
    }

    private void OnEnable()
    {
        //textMoveSequence.Play();
        TextMoved(cutHpText);
        cutHpText.text = content_text;
        Invoke("DelayShowText", 1f);
    }

    /// <summary>
    /// 延迟显示
    /// </summary>
    private void DelayShowText()
    {
        cutHpText.transform.position = vec;
        cutHpText.enabled = true;
        textMoveSequence.Play();
        Invoke("ToDisable", 2f);
    }

    private void ToDisable()
    {
        cutHpText.enabled = false;
    }

    private void TextMoved(Graphic graphic)
    {

        Transform rect = graphic.transform;

        Color color = graphic.color;

        graphic.color = new Color(color.r, color.g, color.b, 0);

        //设置DOTween队列

        textMoveSequence = DOTween.Sequence();

        //设置Text移动和透明度的变化值
        print(addPosY);
        Tweener textMove01 = rect.DOMoveY(rect.position.y + addPosY, 1f);

        Tweener textMove02 = rect.DOMoveY(rect.position.y + addPosY, 1f);

        Tweener textColor01 = graphic.DOColor(new Color(color.r, color.g, color.b, 1), 1f);

        Tweener textColor02 = graphic.DOColor(new Color(color.r, color.g, color.b, 0), 1f);

        //Append 追加一个队列，Join 添加一个队列

        //中间间隔

        //Append 再追加一个队列，再Join 添加一个队列

        textMoveSequence.Append(textMove01);
        textMoveSequence.Join(textColor01);

        textMoveSequence.AppendInterval(0f);

        textMoveSequence.Append(textMove02);
        textMoveSequence.Join(textColor02);

        textMoveSequence.Pause();
    }
}
