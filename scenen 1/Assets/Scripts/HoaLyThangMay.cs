using UnityEngine;

// TẠO RA MỘT KIỂU DỮ LIỆU MỚI ĐỂ HIỂN THỊ NGOÀI INSPECTOR
[System.Serializable]
public class TramDungThangMay
{
    public Transform diemNeo; // Vị trí điểm neo (A, B, C, D...)

    [Tooltip("Tích dấu V vào đây nếu muốn thang máy đến đây thì DỪNG LẠI CHỜ Floria")]
    public bool dungChoNguoiChoi = true;
}

public class HoaLyThangMay : MonoBehaviour
{
    [Header("Hành trình Di Chuyển (Danh sách các trạm)")]
    public TramDungThangMay[] hanhTrinh;

    [Header("Cài đặt")]
    public float tocDo = 2f;

    private int chiSoHienTai = 0;
    private bool dangDiChuyen = false; // Công tắc

    void Start()
    {
        // Vừa vào game, tự động dời hoa về vị trí trạm đầu tiên (Trạm số 0)
        if (hanhTrinh.Length > 0 && hanhTrinh[0].diemNeo != null)
        {
            transform.position = hanhTrinh[0].diemNeo.position;

            // Lên lịch trình đến trạm số 1
            if (hanhTrinh.Length > 1)
            {
                chiSoHienTai = 1;
            }
        }
    }

    void Update()
    {
        // Nếu công tắc TẮT thì đứng im
        if (hanhTrinh.Length == 0 || !dangDiChuyen) return;

        Transform mucTieu = hanhTrinh[chiSoHienTai].diemNeo;
        if (mucTieu == null) return;

        // 1. Di chuyển bông hoa về phía trạm mục tiêu
        transform.position = Vector3.MoveTowards(transform.position, mucTieu.position, tocDo * Time.deltaTime);

        // 2. Nếu ĐÃ ĐẾN TRẠM
        if (Vector3.Distance(transform.position, mucTieu.position) < 0.05f)
        {
            // Kiểm tra xem trạm này có yêu cầu dừng lại chờ người chơi không?
            if (hanhTrinh[chiSoHienTai].dungChoNguoiChoi == true)
            {
                dangDiChuyen = false; // Tắt động cơ, chờ Floria dẫm lại thì mới đi tiếp
            }

            // Chốt điểm đến cho chặng tiếp theo
            chiSoHienTai++;
            if (chiSoHienTai >= hanhTrinh.Length)
            {
                chiSoHienTai = 0; // Quay vòng lại trạm đầu tiên
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);

            // Floria dẫm lên -> Bật công tắc cho chạy!
            dangDiChuyen = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}