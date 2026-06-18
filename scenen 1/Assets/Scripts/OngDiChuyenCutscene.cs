using UnityEngine;

public class OngDiChuyenCutscene : MonoBehaviour
{
    [Header("Đạo Cụ")]
    [Tooltip("Kéo cái cọc tàng hình Diem_Ong_Dung vào đây")]
    public Transform diemDung;
    public Animator anim;

    [Header("Cài Đặt")]
    public float tocDoDiBo = 2f;

    private bool daDenNoi = false;

    void Update()
    {
        // Nếu chưa có điểm dừng hoặc đã đến nơi rồi thì không làm gì cả
        if (diemDung == null || daDenNoi) return;

        // 1. DI CHUYỂN: Lướt con ong về phía điểm dừng
        transform.position = Vector3.MoveTowards(transform.position, diemDung.position, tocDoDiBo * Time.deltaTime);

        // 2. KIỂM TRA ĐÍCH ĐẾN: Nếu cách điểm dừng dưới 0.1m nghĩa là đã tới nơi
        if (Vector3.Distance(transform.position, diemDung.position) < 0.1f)
        {
            daDenNoi = true;

            // Đạo diễn hô: "Đến nơi rồi, đổi sang dáng đứng khoanh tay chờ Floria đi!"
            if (anim != null)
            {
                anim.CrossFadeInFixedTime("Orc Idle", 0.25f);
            }
        }
    }
}