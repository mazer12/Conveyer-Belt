using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Teleporter exitGate;

    public Vector3 GetExitPosition()
    {
        return exitGate.transform.position;
    }
}
