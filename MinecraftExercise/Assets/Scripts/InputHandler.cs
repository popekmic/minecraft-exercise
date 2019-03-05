using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : IPlayerMovementInput, IMiningInput, IBuildingInput
{
    public Vector3 GetMovementVector()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    public float GetMouseAxisX()
    {
        return Input.GetAxis("Mouse X");
    }

    public float GetMouseAxisY()
    {
        return Input.GetAxis("Mouse Y");
    }

    public bool IsJumpKeyPressed()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool IsPointerOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool IsMiningKeyActive()
    {
        return Input.GetButton("Fire1");
    }

    public bool IsBuildModeKeyPressed()
    {
        return Input.GetButtonDown("Fire2");
    }

    public bool IsBuildCubeKeyPressed()
    {
        return Input.GetButtonDown("Fire3");
    }
}