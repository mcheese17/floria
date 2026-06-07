using UnityEngine;

public class HoaNhunAnimation : MonoBehaviour
{
    public Animator animatorCuaHoa;

    // Tạo một biến đếm giờ để khóa va chạm (Cooldown)
    private float thoiGianHoiChieu = 0f;

    void Update()
    {
        // Trừ dần thời gian hồi chiêu theo thời gian thực của game
        if (thoiGianHoiChieu > 0f)
        {
            thoiGianHoiChieu -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // THÊM ĐIỀU KIỆN: Chỉ nhún khi là Player VÀ thời gian hồi chiêu đã về 0
        if (collision.gameObject.CompareTag("Player") && thoiGianHoiChieu <= 0f)
        {
            animatorCuaHoa.SetTrigger("VaCham");

            // Cài đặt thời gian khóa nhún là 1 giây (em có thể chỉnh số này cho khớp với độ dài clip Hoa_Bounce)
            thoiGianHoiChieu = 1f;
        }
    }
}