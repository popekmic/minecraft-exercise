using UnityEngine;

public interface IPlayerMovementInput
{
    Vector3 GetMovementVector();
    float GetMouseAxisX();
    float GetMouseAxisY();
    bool IsJumpKeyPressed();
    bool IsPointerOverUi();
}

public interface IMiningInput
{
    bool IsMiningKeyActive();
}

public interface IBuildingInput
{
    bool IsBuildModeKeyPressed();
    bool IsBuildCubeKeyPressed();
}