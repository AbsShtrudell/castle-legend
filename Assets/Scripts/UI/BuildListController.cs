using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildListController : MonoBehaviour
{
    private RectTransform BuildListPanel;
    private RectTransform BuildListButton;
    private bool listOpened = false;

    private void OnEnable()
    {
        BuildListPanel = transform.GetChild(0).GetComponent<RectTransform>();
        BuildListButton = transform.GetChild(1).GetComponent<RectTransform>();
    }

    public void OnBuildListButtonClicked()
    {
        if (!listOpened)
        {
            LeanTween.size(BuildListPanel, new Vector2(800, 180), Time.deltaTime * 20f);
            LeanTween.rotateZ(BuildListButton.gameObject, 180, Time.deltaTime * 20f);
            listOpened = true;
        }
        else
        {
            LeanTween.size(BuildListPanel, new Vector2(0, 180),Time.deltaTime * 20f);
            LeanTween.rotateZ(BuildListButton.gameObject, 0, Time.deltaTime * 20f);
            listOpened = false;
        }
    }
}
