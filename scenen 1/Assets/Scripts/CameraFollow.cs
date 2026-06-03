using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Setup")]
    [Tooltip("Kéo object nhân vật (tpose_unity) vào đây")]
    public Transform target;

    [Header("Camera Settings")]
    [Tooltip("Thời gian làm mượt (càng lớn camera đi theo càng trễ)")]
    public float smoothTime = 0.25f;

    // Biến lưu trữ khoảng cách ban đầu
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Khi game vừa chạy, tự động tính toán khoảng cách hiện tại giữa Camera và Nhân vật
        if (target != null)
        {
            offset = transform.position - target.position;
        }
        else
        {
            Debug.LogWarning("Em chưa gán nhân vật vào ô Target của Camera!");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Vị trí mục tiêu của camera = vị trí hiện tại của nhân vật + khoảng cách ban đầu
        Vector3 targetPosition = target.position + offset;

        // Di chuyển camera mượt mà đến vị trí mục tiêu
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}