using UnityEngine;

public abstract class EnteringAction : MonoBehaviour
{
    [SerializeField] private Transform enteringPos;
    [SerializeField] private Transform jumpscarePos;
    [SerializeField] private Quaternion enterinRot;
    [SerializeField] private Quaternion jumpscareRot;
    public virtual void Action()
    {
        return;
    }
    public Quaternion GetEnteringRot() { return enterinRot; }
    public Quaternion GetJumpscareRot() { return jumpscareRot; }
    public Vector3 GetEnteringPos() { return enteringPos.position; }
    public Vector3 GetJumpscarePos() { return jumpscarePos.position; }
}
