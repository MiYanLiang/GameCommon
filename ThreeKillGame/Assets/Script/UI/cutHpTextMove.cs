using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class cutHpTextMove : MonoBehaviour
{
    Text cutHpText;

    public string content_text; //展示内容
    
    private Sequence textMoveSequence;  //DOTween队列

    private void Awake()
    {
        content_text = "";
        cutHpText = GetComponent<Text>();
        cutHpText.text = content_text;
    }
    
    private void OnEnable()
    {
        TextMoved(cutHpText);
        cutHpText.text = content_text;
        if(UIControl.isShowCutHpText)
            Invoke("DelayShowText", 1.5f);
    }

    /// <summary>
    /// 延迟显示
    /// </summary>
    private void DelayShowText()
    {
        cutHpText.enabled = true;
        textMoveSequence.Play();
        Invoke("ToDisable", 4f);
    }

    private void ToDisable()
    {
        cutHpText.enabled = false;
    }

    private void TextMoved(Graphic graphic)
    {
        Color color = graphic.color;
        graphic.color = new Color(color.r, color.g, color.b, 0);
        textMoveSequence = DOTween.Sequence();
        
        Tweener textColor01 = graphic.DOColor(new Color(color.r, color.g, color.b, 1), 1f);
        Tweener textColor02 = graphic.DOColor(new Color(color.r, color.g, color.b, 1), 2f);
        Tweener textColor03 = graphic.DOColor(new Color(color.r, color.g, color.b, 0), 1f);
        
        textMoveSequence.Join(textColor01);

        textMoveSequence.AppendInterval(0);

        textMoveSequence.Join(textColor02);

        textMoveSequence.AppendInterval(0);

        textMoveSequence.Join(textColor03);

        textMoveSequence.Pause();
    }
}
