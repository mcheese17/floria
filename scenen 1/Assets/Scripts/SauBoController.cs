using UnityEngine;
using System.Collections;

public class SauBoController : MonoBehaviour
{
    private Collider vungChanNguoi;
    private bool dangHien = true;
    private Vector3 kichThuocBanDau; // Ghi nhớ kích thước gốc của con sâu

    [Header("Cài đặt Sâu")]
    [Tooltip("Thời gian sâu trồi lên lại (tính bằng giây)")]
    public float thoiGianHoiSinh = 5f;

    [Header("Hiệu Ứng (Kéo thả Prefab vào đây)")]
    public GameObject prefabHieuUngNo; // Biến mới để chứa khuôn nổ

    void Start()
    {
        vungChanNguoi = GetComponent<Collider>();
        kichThuocBanDau = transform.localScale; // Lưu lại size lúc đầu game
    }

    void OnTriggerEnter(Collider other)
    {
        // Nếu chạm phải đạn và sâu đang hiện
        if (other.CompareTag("VienDan") && dangHien)
        {
            StartCoroutine(SauTamThoiBienMat());
        }
    }

    IEnumerator SauTamThoiBienMat()
    {
        dangHien = false;

        // --- BẬT HIỆU ỨNG NỔ ---
        if (prefabHieuUngNo != null)
        {
            // Sinh ra vụ nổ ngay tại vị trí con sâu đang đứng
            GameObject vuNo = Instantiate(prefabHieuUngNo, transform.position, Quaternion.identity);

            // Tự động xóa vụ nổ sau 2 giây để dọn rác bộ nhớ
            Destroy(vuNo, 2f);
        }

        // 1. Tàng hình bằng cách thu nhỏ về 0 (Script vẫn sống và chạy bình thường)
        transform.localScale = Vector3.zero;

        // 2. Tắt chức năng chặn đường để Floria đi qua
        if (vungChanNguoi != null) vungChanNguoi.enabled = false;

        // 3. Đếm ngược thời gian hồi sinh
        yield return new WaitForSeconds(thoiGianHoiSinh);

        // 4. Phình to lại y như kích thước cũ
        transform.localScale = kichThuocBanDau;

        // 5. Bật lại chức năng chặn đường
        if (vungChanNguoi != null) vungChanNguoi.enabled = true;

        dangHien = true;
    }
}