using UnityEngine;
using UnityEngine.UI; // Cần cái này để điều khiển UI Image
using UnityEngine.SceneManagement; // Cần cái này để load Scene mới
using System.Collections;

public class ChuyenSceneFade : MonoBehaviour
{
    [Header("Cài đặt Chuyển Cảnh")]
    [Tooltip("Gõ chính xác tên Scene thứ 2 của em vào đây")]
    public string tenSceneTiepTheo = "Scene2";

    [Tooltip("Thời gian màn hình tối dần (tính bằng giây)")]
    public float thoiGianMoDan = 1.5f;

    [Header("UI Màn Hình Đen")]
    public Image manHinhDen; // Kéo vật thể ManHinhDen vào đây

    private bool dangChuyenScene = false;

    void OnTriggerEnter(Collider other)
    {
        // Khi Floria đi vào vùng gốc cây
        if (other.CompareTag("Player") && !dangChuyenScene)
        {
            dangChuyenScene = true;
            StartCoroutine(HieuUngChuyenMan());
        }
    }

    IEnumerator HieuUngChuyenMan()
    {
        // 1. Khóa di chuyển của Floria (Tùy chọn, để cô bé không chạy lố rớt map)
        PlayerController scriptDiChuyen = FindAnyObjectByType<PlayerController>();
        if (scriptDiChuyen != null)
        {
            scriptDiChuyen.enabled = false;
            scriptDiChuyen.GetComponent<Animator>().SetFloat("Speed", 0f);
        }

        // 2. Bắt đầu làm mờ màn hình
        float thoiGianDaTroiQua = 0f;
        Color mauHienTai = manHinhDen.color;

        while (thoiGianDaTroiQua < thoiGianMoDan)
        {
            thoiGianDaTroiQua += Time.deltaTime;
            // Tăng dần độ đục (Alpha) từ từ lên 1 (đen đặc)
            mauHienTai.a = Mathf.Lerp(0f, 1f, thoiGianDaTroiQua / thoiGianMoDan);
            manHinhDen.color = mauHienTai;
            yield return null;
        }

        // Đảm bảo đen 100% trước khi chuyển
        mauHienTai.a = 1f;
        manHinhDen.color = mauHienTai;

        // Nghỉ nửa giây cho màn hình đen thui rồi mới load Scene mới cho êm
        yield return new WaitForSeconds(0.5f);

        // 3. Tải Scene 2
        SceneManager.LoadScene(tenSceneTiepTheo);
    }
}