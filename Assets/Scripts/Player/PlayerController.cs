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

    private ObjectPool<DynamicBlock>[] blockPools;

    public GameObject pawn;
    private void Awake()
    {
        buildItemsContainer = GetComponent<BuildItemsContainer>(); //zenjet
        playerActions = new PlayerControlActions();
        blockPools = new ObjectPool<DynamicBlock>[buildItemsContainer.GetBuildBlocks().Length];

        for (int i = 0; i < buildItemsContainer.GetBuildBlocks().Length; i++)
            blockPools[i] = new ObjectPool<DynamicBlock>(buildItemsContainer.GetBuildBlocks()[i].buildBlock, 50);

        buildActionsMap = playerActions.Build;
        controllActionsMap = playerActions.Controll;
    }

    private void OnEnable()
    {
        playerActions.Build.Place.performed += PlaceObjectAction;
        playerActions.Build.DisableBuildMode.performed += DisableBuildModeAction;
        playerActions.Build.Rotate.performed += RotateAction;
        playerActions.Build.Delete.performed += DeleteAction;

        playerActions.Controll.Select.performed += SelectAction;
        playerActions.Controll.Order.performed += OrderAction;

        controllActionsMap.Enable();
    }

    private void OnDisable()
    {
        playerActions.Build.Place.performed -= PlaceObjectAction;
        playerActions.Build.DisableBuildMode.performed -= DisableBuildModeAction;
        playerActions.Build.Rotate.performed -= RotateAction;
        playerActions.Build.Delete.performed -= DeleteAction;

        playerActions.Controll.Select.performed -= SelectAction;
        playerActions.Controll.Order.performed -= OrderAction;

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
            if (destinationBlock != null)
            {
                destinationBlock.PlaceBlock(activeBlock, destinationBlock.GetClosestSnapPoint(destinationHit.normal, destinationHit.point));
                activeBlock.GetComponent<Collider>().enabled = true;
                SetActiveObj(lastBlockType);
            }
        }
    }

    private void DeleteAction(InputAction.CallbackContext obj)
    {
        if (!pointerOverUI)
        {
            if (destinationBlock != null)
            {
                destinationBlock.DeleteBlock();
                destinationBlock = null;
            }
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

    private void OrderAction(InputAction.CallbackContext obj)
    {
        if (!pointerOverUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 1000))
            {
                var nav = new NavMeshData();
                var instance = NavMesh.AddNavMeshData(nav);
                NavMeshUpdater.UpdateNavMesh(new Bounds(hitData.point, Vector3.one * 0.1f), nav);
                pawn.GetComponent<Pawn>().MoveTo(hitData.point);
            }
        }
    }

    private void SelectAction(InputAction.CallbackContext obj)
    {
        
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

        activeBlock.ReturnToPool();
    }

    private void SetActiveObj(int index)
    {
        if (index < buildItemsContainer.GetBuildBlocks().Length && index >= 0)
        {

            activeBlock = blockPools[index].Pull();
            activeBlock.GetComponent<Collider>().enabled = false;
            lastBlockType = index;
        }
    }

    public void ChangeActiveObj(int index)
    {
        if(activeBlock) activeBlock.ReturnToPool();
        SetActiveObj(index);
    }
}
