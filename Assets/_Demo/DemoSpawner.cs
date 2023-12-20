using Fusion;
using UnityEngine;

public class DemoSpawner : NetworkBehaviour
{
    public float PlayerSpeed = 5f;

    private NetworkCharacterControllerPrototype _controller;

    private void Awake()
    {
        _controller = GetComponent<NetworkCharacterControllerPrototype>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out InputDataCustom data))
        {
            data.direction.Normalize();
            _controller.Move(PlayerSpeed * data.direction * Runner.DeltaTime);
        }
    }
}