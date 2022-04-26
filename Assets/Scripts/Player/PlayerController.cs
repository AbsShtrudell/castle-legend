using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControlActions playerActions;

    private RaycastHit cursorHitData;
    bool placed = false;
    [SerializeField]
    private GameObject block; 

    private void Awake()
    {
        playerActions = new PlayerControlActions();
    }

    private void OnEnable()
    {
        playerActions.Build.Place.performed += PlaceObject;
        playerActions.Build.Enable();
    }

    private void OnDisable()
    {
        playerActions.Build.Disable();
    }
    private void Update()
    {
        UpdateMouseWorldPosition();


        if (cursorHitData.collider.CompareTag("Block") && placed != true)
        {
            cursorHitData.collider.GetComponent<DynamicBlock>().SnapBlock(block.GetComponent<DynamicBlock>(), cursorHitData.collider.GetComponent<DynamicBlock>().GetClosestSnapPoint(cursorHitData.normal, cursorHitData.point));
        }
        if (cursorHitData.collider.CompareTag("Terrain") && placed != true)
        {
            //Chunk chunk = cursorHitData.collider.GetComponent<Chunk>();
            StaticBlock stblock = cursorHitData.collider.GetComponent<Chunk>().GetBlock((BoxCollider)cursorHitData.collider);
            stblock.SnapBlock(block.GetComponent<DynamicBlock>(), stblock.GetClosestSnapPoint(cursorHitData.normal, cursorHitData.point));
        }

        if (Mouse.current.leftButton.isPressed && placed != true)
        {
            cursorHitData.collider.GetComponent<DynamicBlock>().PlaceBlock(block.GetComponent<DynamicBlock>(), cursorHitData.collider.GetComponent<DynamicBlock>().GetClosestSnapPoint(cursorHitData.normal, cursorHitData.point));
            placed = true;
        }

    }

    private void UpdateMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000))
        {
            cursorHitData = hitData;
        }
    }

    private void PlaceObject(InputAction.CallbackContext inputValue)
    {
       
    }
}
