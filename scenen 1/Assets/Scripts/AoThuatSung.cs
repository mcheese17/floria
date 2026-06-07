using UnityEngine;
using System.Collections;

public class AoThuatSung : MonoBehaviour
{
    [Header("Tráo Đổi Súng")]
    public GameObject sungTrenHoa; // Súng A: Đang cắm trên cây
    public GameObject sungTrenTay; // Súng B: Đang tàng hình ở xương bàn tay

    [Header("Hiệu Ứng Phụ (Không bắt buộc)")]
    [Tooltip("Em có thể tạo 1 Particle System nổ lấp lánh gán vào đây để lúc hái có chớp sáng")]
    public ParticleSystem hieuUngChopSang;

    // --- HÀM NÀY SẼ GỌI BẰNG ANIMATION EVENT LÚC TAY CHẠM SÚNG ---
    public void ThucHienTraoSung()
    {
        // 1. Hiện khẩu súng trên tay Floria lên ngay lập tức
        if (sungTrenTay != null) sungTrenTay.SetActive(true);

        // 2. Chạy hiệu ứng lấp lánh (nếu em có gán)
        if (hieuUngChopSang != null) hieuUngChopSang.Play();

        // 3. Làm súng trên hoa biến mất mượt mà (Thu nhỏ rồi tàng hình)
        if (sungTrenHoa != null)
        {
            StartCoroutine(ThuNhoVaBienMat(sungTrenHoa));
        }
    }

    IEnumerator ThuNhoVaBienMat(GameObject obj)
    {
        float thoiGianBienMat = 0.15f; // Thời gian thu nhỏ siêu tốc (0.15 giây)
        float tg = 0;
        Vector3 scaleBanDau = obj.transform.localScale;

        // Ép Scale của cây súng nhỏ dần về 0
        while (tg < thoiGianBienMat)
        {
            tg += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(scaleBanDau, Vector3.zero, tg / thoiGianBienMat);
            yield return null;
        }

        // Tắt hẳn object để tối ưu game
        obj.SetActive(false);
    }
}