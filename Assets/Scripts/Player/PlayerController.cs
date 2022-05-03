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
    private InputActionMap terrainActionsMap;

    public event Action buildModeEnabled;
    public event Action controllModeEnabled;
    public event Action terrainModeEnabled;
    public event Action<float> resourcesChanged;

    private bool pointerOverUI = false;
    private int lastBlockType = 0;
    private float resourceAmount = 0;

    private IBlock destinationBlock;
    private RaycastHit destinationHit;
    private DynamicBlock activeBlock;
    private BlocksContainer blocksContainer;

    private ObjectPool<DynamicBlock>[] blockPools;

    private Pawn selectedPawn;

    //-----------Object Controll-----------//

    private void Awake()
    {
        blocksContainer = GetComponent<BlocksContainer>(); //zenjet
        playerActions = new PlayerControlActions();
        blockPools = new ObjectPool<DynamicBlock>[blocksContainer.GetBlocks().Length];

        for (int i = 0; i < blocksContainer.GetBlocks().Length; i++)
            blockPools[i] = new ObjectPool<DynamicBlock>(blocksContainer.GetBlocks()[i].buildBlock, 50);

        buildActionsMap = playerActions.Build;
        controllActionsMap = playerActions.Controll;
        terrainActionsMap = playerActions.Terrain;
    }

    private void OnEnable()
    {
        playerActions.Build.Place.performed += PlaceObjectAction;
        playerActions.Build.Rotate.performed += RotateAction;
        playerActions.Build.ChangeMode.performed += DisableBuildModeAction;

        playerActions.Controll.Select.performed += SelectAction;
        playerActions.Controll.Order.performed += OrderAction;

        playerActions.Terrain.Add.performed += AddTerrainAction;
        playerActions.Terrain.Remove.performed += RemoveTerrainAction;
        playerActions.Terrain.ChangeMode.performed += DisableTerrainModeAction;

        controllActionsMap.Enable();
    }

    private void OnDisable()
    {
        playerActions.Build.Place.performed -= PlaceObjectAction;
        playerActions.Build.Rotate.performed -= RotateAction;
        playerActions.Build.Rotate.performed -= RotateAction;

        playerActions.Controll.Select.performed -= SelectAction;
        playerActions.Controll.Order.performed -= OrderAction;

        playerActions.Terrain.Add.performed -= AddTerrainAction;
        playerActions.Terrain.Remove.performed -= RemoveTerrainAction;
        playerActions.Terrain.ChangeMode.performed -= DisableTerrainModeAction;

        controllActionsMap.Disable();
        buildActionsMap.Disable();
        terrainActionsMap.Disable();
    }

    private void Start()
    {
        AddResources(1000);
    }

    //-----------Logic-----------//

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
        if (destinationBlock != null && activeBlock != null)
        {
            destinationBlock.SnapBlock(activeBlock, destinationHit.normal, destinationHit.point);
        }
    }

    //-----------Build Mode Actions-----------//

    private void PlaceObjectAction(InputAction.CallbackContext inputValue)
    {
        if (!pointerOverUI)
        {
            if (destinationBlock != null && activeBlock != null)
            {
                if(resourceAmount >= blocksContainer.GetBlocks()[lastBlockType].price)
                destinationBlock.PlaceBlock(activeBlock, destinationHit.normal, destinationHit.point);
                activeBlock.GetComponent<Collider>().enabled = true;
                RemoveRecources(blocksContainer.GetBlocks()[lastBlockType].price);
                activeBlock = null;
                SetActiveObj(lastBlockType);
            }
        }
    }

    private void RotateAction(InputAction.CallbackContext value)
    {
        if (activeBlock != null)
        {
            float angle = value.ReadValue<float>() * 90f;
            activeBlock.SetRotation(angle);
        }
    }

    private void DisableBuildModeAction(InputAction.CallbackContext obj)
    {
        DisableBuildMode();
    }

    //-----------Controll Mode Actions-----------//

    private void SelectAction(InputAction.CallbackContext obj)
    {
        if (!pointerOverUI)
        {
            Deselect();

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 1000))
            {
                if(hitData.collider.gameObject.layer == 10)
                    Select(hitData.collider.GetComponent<Pawn>());
            }
        }
    }

    private void OrderAction(InputAction.CallbackContext obj)
    {
        if (!pointerOverUI)
        {
            if (selectedPawn != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                RaycastHit hitData;
                if (Physics.Raycast(ray, out hitData, 1000))
                {
                    if(hitData.collider.gameObject.layer == 11)
                        selectedPawn.Interact(hitData.collider.gameObject);
                    else
                        selectedPawn.MoveTo(hitData.point);
                }
            }
        }
    }

    //-----------Terrain Mode Actions-----------//

    private void AddTerrainAction(InputAction.CallbackContext obj)
    {
        if (!pointerOverUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 1000, (1 << 7)))
            {
                 //hitData.collider.GetComponent<Chunk>().AddBlock(hitData.collider, hitData.normal);
            }
        }
    }

    private void RemoveTerrainAction(InputAction.CallbackContext obj)
    {
        if (!pointerOverUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 1000, ((1 << 6) | (1 << 7))))
            {
                BlockQueue blqueu = hitData.collider.GetComponent<BlockQueue>();
                blqueu.GetBlock(hitData.collider).DeleteBlock();
            }
        }
    }

    private void DisableTerrainModeAction(InputAction.CallbackContext obj)
    {
        DisableTerrainMode();
    }

    //-----------Build Mode Logic-----------//

    public void DisableBuildMode()
    {
        EnableControllMode();

        activeBlock.ReturnToPool();
    }

    public void EnableBuildMode()
    {
        if (!buildActionsMap.enabled)
        {
            buildActionsMap.Enable();
            controllActionsMap.Disable();
            terrainActionsMap.Disable();

            //SetActiveObj(0);

            buildModeEnabled?.Invoke();
        }
    }

    private void SetActiveObj(int index)
    {
        if (index < blocksContainer.GetBlocks().Length && index >= 0)
        {
            activeBlock = blockPools[index].Pull();

            activeBlock.GetComponent<Collider>().enabled = false;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 1000))
            {
                activeBlock.transform.position = hitData.point;
            }

            lastBlockType = index;
        }
    }

    public void ChangeActiveObj(int index)
    {
        if (activeBlock) activeBlock.ReturnToPool();
        SetActiveObj(index);
    }

    //-----------Controll Mode Logic-----------//

    public void EnableControllMode()
    {
        if (!controllActionsMap.enabled)
        {
            if(activeBlock != null) activeBlock.ReturnToPool();

            controllActionsMap.Enable();
            buildActionsMap.Disable();
            terrainActionsMap.Disable();

            controllModeEnabled?.Invoke();
        }
    }

    private void Select(Pawn pawn)
    {
        Deselect();
        selectedPawn = pawn;
        selectedPawn.Select();
    }

    private void Deselect()
    {
        if (selectedPawn != null)
        {
            selectedPawn.Deselect();
            selectedPawn = null;
        }
    }

    //-----------Terrain Mode Logic-----------//

    public void EnableTerrainMode()
    {
        if (!terrainActionsMap.enabled)
        {
            if (activeBlock != null) activeBlock.ReturnToPool();

            terrainActionsMap.Enable();
            buildActionsMap.Disable();
            controllActionsMap.Disable();

            terrainModeEnabled?.Invoke();
        }
    }

    public void DisableTerrainMode()
    {
        EnableControllMode();
    }

    //-----------Others-----------//

    public void AddResources(float amount)
    {
        if (amount >= 0)
        {
            resourceAmount += amount;
            resourcesChanged?.Invoke(resourceAmount);
        }
    }

    public void RemoveRecources(float amount)
    {
        if (amount >= 0)
        {
            resourceAmount -= amount;
            resourcesChanged?.Invoke(resourceAmount);
        }
    }
}
