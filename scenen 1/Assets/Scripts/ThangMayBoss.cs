using UnityEngine;

public class ThangMayBoss : MonoBehaviour
{
    [Header("Cài đặt Hành Trình")]
    [Tooltip("Kéo thả 3 điểm mốc (Tầng 1, 2, 3) vào đây")]
    public Transform[] cacDiemDung;

    [Tooltip("Tốc độ di chuyển của thang máy")]
    public float tocDo = 4f;

    private int diemHienTai = 0; // Đang hướng tới điểm nào trong danh sách
    private bool diLen = true;   // true = đi lên tầng cao hơn, false = đi lùi xuống
    private bool daKichHoat = false; // Công tắc: Chờ Floria nhảy lên mới chạy

    void Update()
    {
        // Nếu chưa kích hoạt, hoặc lỡ quên chưa kéo điểm mốc vào thì đứng im
        if (!daKichHoat || cacDiemDung.Length == 0) return;

        // 1. Lệnh di chuyển thang máy về phía Điểm Dừng hiện tại
        transform.position = Vector3.MoveTowards(transform.position, cacDiemDung[diemHienTai].position, tocDo * Time.deltaTime);

        // 2. Kiểm tra xem thang máy đã đến nơi chưa?
        if (Vector3.Distance(transform.position, cacDiemDung[diemHienTai].position) < 0.1f)
        {
            // Nếu đã đến nơi, tính toán xem tiếp theo sẽ đi lên hay đi xuống (Hiệu ứng Ping-Pong)
            if (diLen)
            {
                diemHienTai++; // Hướng tới tầng tiếp theo
                if (diemHienTai >= cacDiemDung.Length - 1) // Nếu đụng trần (Tầng 3)
                {
                    diemHienTai = cacDiemDung.Length - 1;
                    diLen = false; // Ra lệnh quay đầu đi xuống
                }
            }
            else
            {
                diemHienTai--; // Hướng về tầng dưới
                if (diemHienTai <= 0) // Nếu đụng đáy (Tầng 1)
                {
                    diemHienTai = 0;
                    diLen = true; // Ra lệnh quay đầu đi lên
                }
            }
        }
    }

    // --- KÍCH HOẠT VÀ CÕNG NGƯỜI CHƠI ---
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Chạm vào Floria là tự động chạy luôn!
            daKichHoat = true;

            // Đặt Floria làm "con" của thang máy để nó cõng đi theo
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Khi Floria nhảy ra khỏi thang máy -> Hủy quan hệ cha con
            collision.transform.SetParent(null);
        }
    }
}