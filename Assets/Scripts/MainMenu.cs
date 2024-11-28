using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public MainController mainController;
    private CanvasGroup _canvasGroup;

    public void RestartMainMenu()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    internal void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, .2f);
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(SetActiveFalse);
    }

    void SetActiveFalse()
    {
        gameObject.SetActive(false);    
    }

    public void Play3X3Clicked()
    {
        mainController.RestartLevel(3,3);
        Hide();
    }

    public void Play4X4Clicked()
    {
        mainController.RestartLevel(4, 4);
        Hide();
    }

    public void Play5X5Clicked()
    {
        mainController.RestartLevel(5, 5);
        Hide();
    }

}
