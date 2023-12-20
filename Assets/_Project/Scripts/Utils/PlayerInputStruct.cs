using Fusion;

public struct PlayerInputStruct : INetworkInput
{
    public float horizontalAxis;
    public float verticalAxis;
    public bool isJumpPressed;
    public bool isHoldingLeftShift;
}