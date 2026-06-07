using UnityEngine;
using DG.Tweening; // Bắt buộc phải gọi thư viện DOTween ra

public class BouncingFlower : MonoBehaviour
{
    [Header("1. Chế độ Đung đưa (Idle)")]
    public Vector3 idleSwayAngle = new Vector3(0, 0, 5f); // Chỉ lắc nhẹ 5 độ ở trục Z
    public float idleSwayDuration = 2f; // Thời gian 1 nhịp lắc

    [Header("2. Chế độ Nhún lò xo (Bounce)")]
    public Vector3 punchAngle = new Vector3(0, 0, -25f); // Ép gập mạnh trục Z xuống 25 độ
    public float punchDuration = 0.6f; // Thời gian nảy
    public int punchVibrato = 6; // Số lần rung bần bật (càng cao rung càng gắt)

    private Tween currentTween; // Biến này dùng để lưu lại chuyển động hiện tại

    void Start()
    {
        // Khi game bắt đầu, gọi trạng thái lắc lư nhẹ
        StartIdleSway();
    }

    void StartIdleSway()
    {
        // Xoay tới góc idleSwayAngle, sau đó lặp lại qua lại (Yoyo) vô tận (-1)
        currentTween = transform.DORotate(idleSwayAngle, idleSwayDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    // Hàm bắt va chạm vật lý khi Floria chạm vào hoa
    void OnCollisionEnter(Collision collision)
    {
        // Phải đảm bảo Floria (có gắn Tag "Player") dẫm lên thì hoa mới phản ứng
        if (collision.gameObject.CompareTag("Player"))
        {
            // BƯỚC 1: Dừng ngay chuyển động lắc lư nhẹ hiện tại
            currentTween.Kill();

            // BƯỚC 2: Trả hoa về góc thẳng đứng để lấy đà
            transform.localRotation = Quaternion.identity;

            // BƯỚC 3: Giáng một cú đấm (Punch) vào trục xoay để tạo cảm giác nhún lò xo
            transform.DOPunchRotation(punchAngle, punchDuration, punchVibrato, 1f)
                .OnComplete(() => {
                    // BƯỚC 4: Sau khi nhún bần bật xong, từ từ lắc lư nhẹ trở lại như chưa có gì xảy ra
                    StartIdleSway();
                });
        }
    }
}