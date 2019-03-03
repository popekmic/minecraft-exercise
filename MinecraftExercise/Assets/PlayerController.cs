using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float gravity;
    public float jumpSpeed;
    [FormerlySerializedAs("camera")] public GameObject playerCamera;
    
    private CharacterController controller;
    private Vector3 moveVector = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveVector = transform.TransformDirection(moveVector) * speed;
            if (Input.GetButtonDown("Jump"))
            {
                moveVector.y = jumpSpeed;
            }
        }
        else
        {
            moveVector = new Vector3(Input.GetAxis("Horizontal"), moveVector.y, Input.GetAxis("Vertical"));
            moveVector = transform.TransformDirection(moveVector);
            moveVector.x *= speed;
            moveVector.z *= speed;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
            playerCamera.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0, 0));
        }

        moveVector.y -= gravity * Time.deltaTime;
        controller.Move(moveVector * Time.deltaTime);
    }
}