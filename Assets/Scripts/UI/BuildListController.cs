using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildListController : MonoBehaviour
{

    [SerializeField]
    private GameObject playerRef;
    [SerializeField]
    private GameObject listElement;

    private RectTransform BuildListPanel;
    private RectTransform BuildListButton;
    private RectTransform ListButton;

    private bool listOpened = false;

    private BuildItemsContainer buildBlocks;
    private float listWidth;
    private void Start()
    {
        BuildListPanel = transform.GetChild(0).GetComponent<RectTransform>();
        BuildListButton = transform.parent.GetChild(2).GetComponent<RectTransform>();
        ListButton = listElement.GetComponent<RectTransform>();
        buildBlocks = playerRef.GetComponent<BuildItemsContainer>();
        listWidth = CalculateListWidth();
        CreateListElements();
        BuildListPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, listWidth);
        LeanTween.moveLocalX(BuildListPanel.gameObject, listWidth, 0.2f);
    }

    public void OnBuildListButtonClicked()
    {
        if (listOpened)
        {
            LeanTween.moveLocalX(BuildListPanel.gameObject, listWidth, 0.2f);
            LeanTween.rotateZ(BuildListButton.gameObject, 180, 0.2f);
            listOpened = false;
        }
        else
        {
            LeanTween.moveLocalX(BuildListPanel.gameObject, 0, 0.2f);
            LeanTween.rotateZ(BuildListButton.gameObject, 0, 0.2f);
            listOpened = true;
        }
    }

    public void OnBuildItemSelected(int index)
    {
        playerRef.GetComponent<PlayerController>().EnableBuildMode();
        playerRef.GetComponent<PlayerController>().ChangeActiveObj(index);
    }

    private float CalculateListWidth()
    {
        return (ListButton.sizeDelta.x + 20f) * buildBlocks.GetBuildBlocks().Length + 10f;
    }

    private void CreateListElements()
    {
        for(int i = 0; i < buildBlocks.GetBuildBlocks().Length; i++)
        {
            int buttonIndex = i;
            GameObject go = Instantiate(ListButton.gameObject, BuildListPanel);
            go.GetComponent<Button>().onClick.AddListener(() => OnBuildItemSelected(buttonIndex));
        }
    }
}
