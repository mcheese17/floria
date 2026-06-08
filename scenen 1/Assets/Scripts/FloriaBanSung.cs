using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FloriaBanSung : MonoBehaviour
{
    public Animator anim;
    public GameObject prefabVienDan;
    public Transform diemRaDan;

    [Header("Căn chỉnh Cảm giác Bắn")]
    public float doTreBan = 0.3f;
    public float thoiGianHoiChieu = 0.5f;

    private bool dangBan = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame && !dangBan)
        {
            StartCoroutine(XuLyBanSung());
        }
    }

    IEnumerator XuLyBanSung()
    {
        dangBan = true;

        // 1. FIX LỖI SPAM PHÍM: Ép Animation phải reset chạy lại đúng từ Frame 0
        anim.Play("Shooting", -1, 0f);

        // 2. Đợi tay giơ lên ngang ngực
        yield return new WaitForSeconds(doTreBan);

        // 3. Sinh ra viên đạn ngay tại nòng súng
        GameObject dan = Instantiate(prefabVienDan, diemRaDan.position, diemRaDan.rotation);
        VienDan scriptDan = dan.GetComponent<VienDan>();

        // 4. FIX LỖI HƯỚNG ĐẠN: Dùng transform.forward.x (Mặt nhân vật đang nhìn về đâu)
        // Nếu tọa độ X âm (Mặt đang nhìn sang TRÁI)
        if (transform.forward.x < -0.01f)
        {
            scriptDan.huongBay = Vector3.left;
            dan.transform.rotation = Quaternion.Euler(0, 180, 0); // Lật đuôi đạn
        }
        // Nếu tọa độ X dương (Mặt đang nhìn sang PHẢI)
        else if (transform.forward.x > 0.01f)
        {
            scriptDan.huongBay = Vector3.right;
            dan.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // Phòng hờ lúc mới vào game chưa đi bước nào (Mặc định bắn phải)
        else
        {
            scriptDan.huongBay = Vector3.right;
            dan.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // 5. Đợi súng nguội rồi mới cho bắn phát tiếp theo
        yield return new WaitForSeconds(thoiGianHoiChieu);
        dangBan = false;
    }
}