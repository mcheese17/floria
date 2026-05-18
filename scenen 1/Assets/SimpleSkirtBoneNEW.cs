using UnityEngine;

public class SimpleSkirtBoneNEW : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;
    public float lagAmount = 10f;

    private Quaternion lastRotation;

    void Start()
    {
        if (target != null)
            lastRotation = target.rotation;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Quaternion delta = target.rotation * Quaternion.Inverse(lastRotation);
        Quaternion lag = Quaternion.Slerp(Quaternion.identity, delta, -lagAmount * Time.deltaTime);

        transform.rotation = lag * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, followSpeed * Time.deltaTime);

        lastRotation = target.rotation;
    }
}