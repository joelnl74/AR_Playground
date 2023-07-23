using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToastView : MonoBehaviour
    ,IToastView
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private RectTransform _rect;

    private bool isShowing;

    private void Awake()
    {
        _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, _rect.anchoredPosition.y + _rect.sizeDelta.y);
    }

    public void Show(string title, string message)
    {
        if (isShowing)
        {
            return;
        }

        _titleText.text = title;
        _descriptionText.text = message;

        StartCoroutine(ShowAnimation());
    }

    private IEnumerator ShowAnimation()
    {
        isShowing = true;
        _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, _rect.anchoredPosition.y - _rect.sizeDelta.y);
        
        yield return new WaitForSeconds(2);
        
        _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, _rect.anchoredPosition.y + _rect.sizeDelta.y);
        isShowing = false;
    }
}
