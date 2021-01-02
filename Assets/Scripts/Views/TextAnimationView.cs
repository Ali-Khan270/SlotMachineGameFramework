using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextAnimationView : MonoBehaviour
{
    private TextMeshProUGUI textView;

    private long currentValue = 0;
    private Tweener tweener;

    void Awake()
    {
        textView = GetComponent<TextMeshProUGUI>();
        textView.text = currentValue.ToString();
    }

    public void ShowValue(long value)
    {
        tweener = DOTween.To(() => currentValue, x => currentValue = x, value, 1f);
        tweener.OnUpdate(UpdateTween);
    }

    private void UpdateTween()
    {
        textView.text = currentValue == 0 ? currentValue.ToString() : currentValue.ToString("#,#");
    }
}
