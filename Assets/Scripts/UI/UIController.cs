using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Zenject.Inject]
    private PlayerController player;
    [Zenject.Inject]
    private BlocksContainer blocksContainer;

    private RectTransform ListButton;
    [SerializeField]
    private GameObject ListElement;
    [SerializeField]
    private RectTransform BuildListPanel;
    [SerializeField]
    private RectTransform BuildListButton;
    [SerializeField]
    private RectTransform TerrainButton;
    [SerializeField]
    private RectTransform TerrainText;
    [SerializeField]
    private Text ResourcesText;

    private bool listOpened = false;
    private bool terrainModeActive = false;

    private float listWidth;

    //-----------Object Controll-----------//

    private void OnEnable()
    {
        player.buildModeEnabled += OnBuildModeEnabled;
        player.controllModeEnabled += OnControllModeEnabled;
        player.terrainModeEnabled += OnTerrainmodeEnabled;
        player.resourcesChanged += OnResourcesChanged;
    }

    private void OnDisable()
    {
        player.buildModeEnabled -= OnBuildModeEnabled;
        player.controllModeEnabled -= OnControllModeEnabled;
        player.terrainModeEnabled -= OnTerrainmodeEnabled;
    }

    private void Start()
    {
        ListButton = ListElement.GetComponent<RectTransform>();

        listWidth = CalculateListWidth();
        CreateListElements();
        BuildListPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, listWidth);
        CloseList();

        DisableTerrainText();
    }

    //-----------UI Events-----------//

    public void OnBuildListButtonClicked()
    {
        if (listOpened)
        {
            CloseList();
        }
        else
        {
            OpenList();
        }
    }

    public void OnTerrianButtonClicked()
    {
        if (!terrainModeActive)
        {
            terrainModeActive = true;
            EnableTerrainText();
            player.EnableTerrainMode();
        }
        else
        {
            terrainModeActive = false;
            DisableTerrainText();
            player.DisableTerrainMode();
        }
    }

    public void OnBuildItemSelected(int index)
    {
        player.EnableBuildMode();
        player.ChangeActiveObj(index);

        EventSystem.current.SetSelectedGameObject(null);
    }

    //-----------Player Events-----------//

    private void OnTerrainmodeEnabled()
    {
        terrainModeActive = true;
        EnableTerrainText();
        CloseList();
    }

    private void OnControllModeEnabled()
    {
        terrainModeActive = false;
        DisableTerrainText();
        CloseList();
    }

    private void OnBuildModeEnabled()
    {
        terrainModeActive = false;
        DisableTerrainText();
        OpenList();
    }

    private void OnResourcesChanged(float value)
    {
        ResourcesText.text = value.ToString();
    }

    //-----------UI Controll-----------//

    private void CloseList()
    {
        LeanTween.moveLocalX(BuildListPanel.gameObject, listWidth, 0.2f);
        LeanTween.rotateZ(BuildListButton.gameObject, 180, 0.2f);
        listOpened = false;
    }

    private void OpenList()
    {
        LeanTween.moveLocalX(BuildListPanel.gameObject, 0, 0.2f);
        LeanTween.rotateZ(BuildListButton.gameObject, 0, 0.2f);
        listOpened = true;
    }

    private void DisableTerrainText()
    {
        TerrainText.gameObject.SetActive(false);
    }

    private void EnableTerrainText()
    {
        TerrainText.gameObject.SetActive(true);
    }

    //-----------Others-----------//

    private void CreateListElements()
    {
        for (int i = 0; i < blocksContainer.GetBlocks().Length; i++)
        {
            int buttonIndex = i;
            GameObject go = Instantiate(ListButton.gameObject, BuildListPanel);
            go.GetComponent<Button>().onClick.AddListener(() => OnBuildItemSelected(buttonIndex));
            go.GetComponent<Image>().sprite = blocksContainer.GetBlocks()[i].icon;
        }
    }

    private float CalculateListWidth()
    {
        return (ListButton.sizeDelta.x + 20f) * blocksContainer.GetBlocks().Length + 10f;
    }
}
