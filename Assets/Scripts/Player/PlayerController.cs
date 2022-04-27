using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private PlayerControlActions playerActions;
    private InputActionMap buildActionsMap;
    private InputActionMap controllActionsMap;

    private bool pointerOverUI = false;
    private int lastBlockType = 0;

    private IBlock destinationBlock;
    private RaycastHit destinationHit;
    private DynamicBlock activeBlock;
    private BuildItemsContainer buildItemsContainer;

    public GameObject pawn;
    private void Awake()
    {
        buildItemsContainer = GetComponent<BuildItemsContainer>();
        playerActions = new PlayerControlActions();
        buildActionsMap = playerActions.Build;
        controllActionsMap = playerActions.Controll;
    }

    private void OnEnable()
    {
        playerActions.Build.Place.performed += PlaceObjectAction;
        playerActions.Build.DisableBuildMode.performed += DisableBuildModeAction;
        playerActions.Build.Rotate.performed += RotateAction;

        playerActions.Controll.Select.performed += SelectAction;
        playerActions.Controll.Order.performed += OrderAction;
        playerActions.Controll.Delete.performed += DeleteAction;
    }

    private void OnDisable()
    {
        playerActions.Build.Place.performed -= PlaceObjectAction;
        playerActions.Build.DisableBuildMode.performed -= DisableBuildModeAction;
        playerActions.Build.Rotate.performed -= RotateAction;

        playerActions.Controll.Select.performed -= SelectAction;
        playerActions.Controll.Order.performed -= OrderAction;
        playerActions.Controll.Delete.performed -= DeleteAction;

        controllActionsMap.Disable();
        buildActionsMap.Disable();
    }

    private void Start()
    { 
    }

    private void Update()
    {
        pointerOverUI = EventSystem.current.IsPointerOverGameObject();

        if (buildActionsMap.enabled)
        {
            UpdateMouseWorldPosition();
            UpdateObjectWorldPosition();
        }
    }

    private void UpdateMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000, ((1 << 6) | (1 << 7))))
        {
            BlockQueue blqueu = hitData.collider.GetComponent<BlockQueue>();
            destinationBlock = blqueu.GetBlock(hitData.collider);
            destinationHit = hitData;
        }
    }

    private void UpdateObjectWorldPosition()
    {
        if (destinationBlock != null)
        {
            destinationBlock.SnapBlock(activeBlock, destinationBlock.GetClosestSnapPoint(destinationHit.normal, destinationHit.point));
        }
    }

    private void PlaceObjectAction(InputAction.CallbackContext inputValue)
    {
        if (!pointerOverUI)
        {
            destinationBlock.PlaceBlock(activeBlock, destinationBlock.GetClosestSnapPoint(destinationHit.normal, destinationHit.point));
            activeBlock.GetComponent<Collider>().enabled = true;
            SetActiveObj(lastBlockType);
        }
    }

    private void DisableBuildModeAction(InputAction.CallbackContext obj)
    {
        DisableBuildMode();
    }

    private void RotateAction(InputAction.CallbackContext value)
    {
        float angle = value.ReadValue<float>() * 90f;
        activeBlock.SetRotation(angle);
    }

    private void DeleteAction(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    private void OrderAction(InputAction.CallbackContext obj)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000, ((1 << 3) | (1 << 6))))
        {
            pawn.GetComponent<NavMeshAgent>().SetDestination(hitData.point);
        }
    }

    private void SelectAction(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    public void EnableBuildMode()
    {
        if (!buildActionsMap.enabled)
        {
            buildActionsMap.Enable();
            controllActionsMap.Disable();

            SetActiveObj(0);
        }
    }

    public void DisableBuildMode()
    {
        buildActionsMap.Disable();
        controllActionsMap.Enable();

        GameObject.Destroy(activeBlock.gameObject);
    }

    private void SetActiveObj(int index)
    {
        if (index < buildItemsContainer.GetBuildBlocks().Length && index >= 0)
        {

            GameObject go = GameObject.Instantiate(buildItemsContainer.GetBuildBlocks()[index].buildBlock);
            activeBlock = go.GetComponent<DynamicBlock>();
            go.GetComponent<Collider>().enabled = false;
            lastBlockType = index;
        }
    }

    public void ChangeActiveObj(int index)
    {
        if(activeBlock) GameObject.Destroy(activeBlock.gameObject);
        SetActiveObj(index);
    }
}
