using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [Header("Camera & Tốc Độ")]
    public Transform mainCamera;

    [Tooltip("1 = Đi theo 100% (bầu trời). 0.5 = Đi theo một nửa. 0 = Đứng im.")]
    [Range(0f, 1f)]
    public float tyLeDiTheo = 0.8f;

    private float viTriBatDauX;
    private float viTriCameraBatDauX; // Thêm cái chốt này

    void Start()
    {
        // Ghi nhớ vị trí đứng ban đầu của tấm ảnh
        viTriBatDauX = transform.position.x;

        // Tự động tìm Camera chính
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }

        // Chốt cứng vị trí của Camera ngay khoảnh khắc bắt đầu game
        if (mainCamera != null)
        {
            viTriCameraBatDauX = mainCamera.position.x;
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Chỉ đo khoảng cách Camera ĐÃ ĐI so với lúc nãy
            float khoangCachCameraDaDi = mainCamera.position.x - viTriCameraBatDauX;

            // Tính toán quãng đường tấm ảnh cần dịch chuyển theo
            float khoangCachDiChuyen = khoangCachCameraDaDi * tyLeDiTheo;

            // Cập nhật vị trí
            transform.position = new Vector3(viTriBatDauX + khoangCachDiChuyen, transform.position.y, transform.position.z);
        }
    }
}