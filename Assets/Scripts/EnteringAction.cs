using UnityEngine;

public abstract class EnteringAction : MonoBehaviour
{
    [SerializeField] private Transform enteringPos;
    [SerializeField] private Quaternion enterinRot;
    public virtual void Action()
    {
        return;
    }
    public Quaternion GetEnteringRot() { return enterinRot; }
    public Vector3 GetEnteringPos() { return enteringPos.position; }
}
