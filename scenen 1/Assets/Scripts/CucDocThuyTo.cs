using UnityEngine;

public class CucDocThuyTo : MonoBehaviour
{
    [Header("Cài đặt Cục Độc")]
    [Tooltip("Tốc độ bay phải nhanh hơn sâu để ép người chơi nhảy")]
    public float tocDoBay = 6f;

    [Tooltip("Tự động bốc hơi sau vài giây cho nhẹ máy")]
    public float thoiGianTonTai = 5f;

    void Start()
    {
        // Vừa sinh ra là bắt đầu đếm ngược tự hủy
        Destroy(gameObject, thoiGianTonTai);
    }

    void Update()
    {
        // Bay thẳng sang trái (về phía Floria)
        transform.Translate(Vector3.right * tocDoBay * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        // Nếu chạm vào người chơi -> Báo tử
        if (other.CompareTag("Player"))
        {
            // 🚨 ĐÃ CẬP NHẬT GỌI LỆNH CHẾT!
            FindAnyObjectByType<HeThongHoiSinh>().FloriaBiGiet();

            // Hủy luôn cục độc khi đã trúng người
            Destroy(gameObject);
        }

        // CỐ TÌNH KHÔNG KIỂM TRA TAG "VienDan"
        // Nghĩa là đạn của Floria bắn vào cục độc sẽ bay xuyên qua luôn, không cản được độc!
    }
}