using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // BẮT BUỘC PHẢI THÊM THƯ VIỆN NÀY ĐỂ QUẢN LÝ SCENE

public class HeThongHoiSinh : MonoBehaviour
{
    [Header("Hiệu ứng Màn Hình Đen")]
    public CanvasGroup manHinhDen;
    public float thoiGianFade = 0.5f;

    private bool dangHoiSinh = false;

    void Start()
    {
        // Khi Scene vừa load xong (lúc mới vào game hoặc vừa hồi sinh)
        // Tự động làm sáng màn hình từ từ để tạo cảm giác mượt mà
        if (manHinhDen != null)
        {
            manHinhDen.alpha = 1; // Bắt đầu bằng màn đen kịt
            StartCoroutine(FadeVaoGame());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dangHoiSinh)
        {
            StartCoroutine(PhaXuLyHoiSinh());
        }
    }

    IEnumerator PhaXuLyHoiSinh()
    {
        dangHoiSinh = true;

        // 1. Tối dần màn hình
        float tg = 0;
        while (tg < thoiGianFade)
        {
            tg += Time.deltaTime;
            manHinhDen.alpha = Mathf.Lerp(0, 1, tg / thoiGianFade);
            yield return null;
        }
        manHinhDen.alpha = 1;

        // Đợi một chút trong bóng tối cho kịch tính
        yield return new WaitForSeconds(0.2f);

        // 2. TẢI LẠI TOÀN BỘ SCENE (Reset quái, thang máy, vị trí người chơi...)
        // Lệnh này sẽ tự động lấy tên của màn chơi hiện tại và load lại nó từ vạch xuất phát
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Lưu ý: Các dòng code dưới lệnh LoadScene sẽ không bao giờ chạy nữa, 
        // vì ngay lúc này toàn bộ hệ thống đã được "đập đi xây lại".
    }

    IEnumerator FadeVaoGame()
    {
        float tg = 0;
        while (tg < thoiGianFade)
        {
            tg += Time.deltaTime;
            manHinhDen.alpha = Mathf.Lerp(1, 0, tg / thoiGianFade);
            yield return null;
        }
        manHinhDen.alpha = 0;
    }
}