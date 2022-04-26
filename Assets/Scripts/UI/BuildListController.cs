using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildListController : MonoBehaviour
{
    private RectTransform BuildListPanel;
    private bool listOpened = false;

    private void OnEnable()
    {
        BuildListPanel = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnBuildListButtonClicked()
    {
        if (!listOpened)
        {
            LeanTween.size(BuildListPanel, new Vector2(800, 180), Time.deltaTime * 10f);
            listOpened = true;
        }
        else
        {
            LeanTween.size(BuildListPanel, new Vector2(0, 180),Time.deltaTime * 10f);
            listOpened = false;
        }
    }
}
