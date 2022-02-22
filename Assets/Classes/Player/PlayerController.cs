using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int MoveSpeed;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnRotate(InputValue input)
    {
        Debug.Log("rotate");
    }

    public void OnMove(InputValue input)
    {
        Vector2 InputVec = input.Get<Vector2>();
        Vector3 MoveVector = new Vector3(InputVec.x, 0, InputVec.y);
        transform.Translate(MoveVector);

        Debug.Log("move");
    }   
}
