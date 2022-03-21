using UnityEngine;

public class ProtoControls : MonoBehaviour, IPlayerControls
{
    public bool IsFlying { get; private set; }

    private void Update()
    {
        IsFlying = Input.GetMouseButton(0);
    }
}
