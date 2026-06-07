using UnityEngine;
using System.Collections;

public class HeThongHoiSinh : MonoBehaviour
{
    [Header("Liên kết Nhân vật")]
    public Transform nhanVat;
    public Rigidbody rbNhanVat;

    // --- ĐÃ SỬA CHỖ NÀY ---
    [Tooltip("Kéo script di chuyển vào đây để khóa lúc hồi sinh")]
    public PlayerController scriptDiChuyenGoc;

    [Header("Cài đặt Hồi Sinh")]
    public Transform diemHoiSinh;

    [Header("Hiệu ứng Màn Hình Đen")]
    public CanvasGroup manHinhDen;
    public float thoiGianFade = 0.5f;

    private bool dangHoiSinh = false;

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

        if (scriptDiChuyenGoc != null) scriptDiChuyenGoc.enabled = false;
        rbNhanVat.linearVelocity = Vector3.zero;
        rbNhanVat.isKinematic = true;

        float tg = 0;
        while (tg < thoiGianFade)
        {
            tg += Time.deltaTime;
            manHinhDen.alpha = Mathf.Lerp(0, 1, tg / thoiGianFade);
            yield return null;
        }
        manHinhDen.alpha = 1;

        nhanVat.position = diemHoiSinh.position;

        yield return new WaitForSeconds(0.2f);

        rbNhanVat.isKinematic = false;

        tg = 0;
        while (tg < thoiGianFade)
        {
            tg += Time.deltaTime;
            manHinhDen.alpha = Mathf.Lerp(1, 0, tg / thoiGianFade);
            yield return null;
        }
        manHinhDen.alpha = 0;

        if (scriptDiChuyenGoc != null) scriptDiChuyenGoc.enabled = true;
        dangHoiSinh = false;
    }
}