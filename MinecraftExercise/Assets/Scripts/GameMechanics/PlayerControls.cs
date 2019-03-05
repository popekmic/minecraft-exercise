using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMechanics
{
    public class PlayerControls : MonoBehaviour
    {
        public float speed;
        public float gravity;
        public float jumpSpeed;
        public GameObject playerCamera;
        public IPlayerMovementInput input;
    
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
                moveVector = input.GetMovementVector();
                moveVector = transform.TransformDirection(moveVector) * speed;
                if (input.IsJumpKeyPressed())
                {
                    moveVector.y = jumpSpeed;
                }
            }
            else
            {
                float y = moveVector.y;
                moveVector = input.GetMovementVector();
                moveVector.y = y;
                moveVector = transform.TransformDirection(moveVector);
                moveVector.x *= speed;
                moveVector.z *= speed;
            }

            if (!input.IsPointerOverUi())
            {
                transform.Rotate(new Vector3(0, input.GetMouseAxisX(), 0));
                playerCamera.transform.Rotate(new Vector3(-input.GetMouseAxisY(), 0, 0));
            }

            moveVector.y -= gravity * Time.deltaTime;
            controller.Move(moveVector * Time.deltaTime);
        }
    }
}