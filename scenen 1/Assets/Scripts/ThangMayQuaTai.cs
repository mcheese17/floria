using UnityEngine;
using System.Collections;

public class ThangMayQuaTai : MonoBehaviour
{
    [Header("Cài đặt Thang Máy")]
    public float thoiGianChiuDung = 3f;
    public float thoiGianPhucHoi = 5f;
    public float tocDoRoi = 10f;
    [Tooltip("Độ mạnh của hiệu ứng rung (càng nhỏ rung càng nhẹ)")]
    public float cuongDoRung = 0.03f; 
    private bool dangBiSap = false;
    private Coroutine coroutineDemNguoc;

    // --- 1. BIẾN KẾT NỐI VỚI ĐỘNG CƠ ---
    private ThangMayBoss dongCoDiChuyen;

    void Start()
    {
        // Tự động tìm và nắm lấy cái Script di chuyển đang gắn trên cùng cục đá
        dongCoDiChuyen = GetComponent<ThangMayBoss>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dangBiSap)
        {
            if (coroutineDemNguoc != null) StopCoroutine(coroutineDemNguoc);
            coroutineDemNguoc = StartCoroutine(KiemTraQuaTai());
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dangBiSap)
        {
            if (coroutineDemNguoc != null) StopCoroutine(coroutineDemNguoc);

            if (dongCoDiChuyen != null) dongCoDiChuyen.enabled = true;
        }
    }

    IEnumerator KiemTraQuaTai()
    {
        yield return new WaitForSeconds(thoiGianChiuDung - 0.5f);

        if (dongCoDiChuyen != null) dongCoDiChuyen.enabled = false;

        Vector3 viTriTruocKhiSap = transform.position;

        float thoiGianRung = 0.5f;
        while (thoiGianRung > 0)
        {
            // Chỉ tập trung lực lắc vào trục X (sang trái/phải), trục Y nhích một chút xíu cho tự nhiên, trục Z khóa chết.
            float lacTraiPhai = Random.Range(-1f, 1f) * cuongDoRung;
            float lacLenXuong = Random.Range(-0.2f, 0.2f) * cuongDoRung; // Y chỉ bằng 1/5 X

            transform.position = viTriTruocKhiSap + new Vector3(lacTraiPhai, lacLenXuong, 0);

            thoiGianRung -= Time.deltaTime;
            yield return null;
        }
        transform.position = viTriTruocKhiSap;

        // SẬP TỤT XUỐNG VỰC!!! 
        dangBiSap = true;

        // XÓA LỆNH TẮT COLLIDER Ở ĐÂY.
        // Giữ nguyên Collider để Floria đứng trên đá và rớt cùng thang máy

        float thoiGianRoi = 1.5f;
        while (thoiGianRoi > 0)
        {
            transform.Translate(Vector3.down * tocDoRoi * Time.deltaTime, Space.World);
            thoiGianRoi -= Time.deltaTime;
            yield return null;
        }

        // CHỜ PHỤC HỒI
        // Bây giờ thang máy đã rớt khuất tầm nhìn, ta mới tắt hình ảnh và va chạm đi
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(thoiGianPhucHoi);

        // TRỒI LÊN LẠI ĐÚNG VỊ TRÍ CŨ
        transform.position = viTriTruocKhiSap;
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        dangBiSap = false;

        if (dongCoDiChuyen != null) dongCoDiChuyen.enabled = true;
    }
}