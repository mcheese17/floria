using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HeThongHoiSinh : MonoBehaviour
{
    [Header("Hiệu ứng Màn Hình Đen")]
    public CanvasGroup manHinhDen;
    public float thoiGianFade = 0.5f;

    [Header("Cài đặt Nhân Vật (MỚI THÊM)")]
    public Animator animNhanVat;
    [Tooltip("Kéo script di chuyển của Floria vào đây để khóa lúc chết")]
    public MonoBehaviour scriptDiChuyen;
    public Rigidbody rbNhanVat;

    private bool dangHoiSinh = false;

    void Start()
    {
        if (manHinhDen != null)
        {
            manHinhDen.alpha = 1;
            StartCoroutine(FadeVaoGame());
        }
    }

    // --- CÁI CHẾT SỐ 1: DO QUÁI VẬT VÀ KỊCH ĐỘC (Có truyền tham số TRUE) ---
    public void FloriaBiGiet()
    {
        if (!dangHoiSinh)
        {
            // true = Bắt buộc phải chờ 3.7s để diễn Animation
            StartCoroutine(PhaXuLyHoiSinh(true));
        }
    }

    // --- CÁI CHẾT SỐ 2: DO RỚT VỰC CHẠM BOX DIE (Có truyền tham số FALSE) ---
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dangHoiSinh)
        {
            // false = Rớt vực rồi, không cần chờ Animation, tắt màn hình luôn
            StartCoroutine(PhaXuLyHoiSinh(false));
        }
    }

    // Đã thêm biến "chetDoQuai" để phân loại thời gian chờ
    IEnumerator PhaXuLyHoiSinh(bool chetDoQuai)
    {
        dangHoiSinh = true;

        // 1. KHÓA ĐIỀU KHIỂN & DỪNG CHUYỂN ĐỘNG
        if (scriptDiChuyen != null) scriptDiChuyen.enabled = false;
        if (rbNhanVat != null) rbNhanVat.linearVelocity = Vector3.zero;

        // 2. PHÂN LOẠI XỬ LÝ THEO KIỂU CHẾT
        if (chetDoQuai)
        {
            // Nếu bị quái giết: Chạy Anim và chờ đủ 3.7 giây
            if (animNhanVat != null) animNhanVat.SetTrigger("DieTrigger");
            yield return new WaitForSeconds(3.7f);
        }
        else
        {
            // Nếu rớt vực: Chỉ cho chờ 0.5 giây để nhân vật kịp rớt khuất khỏi màn hình là tắt đèn luôn
            yield return new WaitForSeconds(0.5f);
        }

        // 3. MÀN HÌNH TỐI DẦN (Fade out)
        float tg = 0;
        while (tg < thoiGianFade)
        {
            tg += Time.deltaTime;
            if (manHinhDen != null) manHinhDen.alpha = Mathf.Lerp(0, 1, tg / thoiGianFade);
            yield return null;
        }
        if (manHinhDen != null) manHinhDen.alpha = 1;

        // Đợi một chút trong bóng tối cho kịch tính
        yield return new WaitForSeconds(0.2f);

        // 4. TẢI LẠI SCENE
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FadeVaoGame()
    {
        float tg = 0;
        while (tg < thoiGianFade)
        {
            tg += Time.deltaTime;
            if (manHinhDen != null) manHinhDen.alpha = Mathf.Lerp(1, 0, tg / thoiGianFade);
            yield return null;
        }
        if (manHinhDen != null) manHinhDen.alpha = 0;
    }
}