using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControlActions playerActions;

    private RaycastHit cursorHitData;

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


        if (cursorHitData.collider.CompareTag("Block"))
        {
            block.GetComponent<SnapPointsManager>().SnapBlock(cursorHitData.collider.GetComponent<SnapPointsManager>().GetClosestSnapPoint(cursorHitData.normal, cursorHitData.point));
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
