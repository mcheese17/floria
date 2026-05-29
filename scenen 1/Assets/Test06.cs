using UnityEngine;
// Lưu ý: Hệ thống mới sử dụng thư viện InputSystem dưới đây
using UnityEngine.InputSystem;

public class Test06 : MonoBehaviour
{
    [Header("Cấu hình di chuyển 3D (Hệ thống mới)")]
    [SerializeField] private float speed = 5f;

    void move()
    {
        // Tạo biến lưu trữ hướng bấm phím
        float moveX = 0f;
        float moveZ = 0f;

        // Lấy trạng thái của bàn phím hiện tại thông qua Input System mới
        Keyboard currentKeyboard = Keyboard.current;

        // Nếu bàn phím hợp lệ (đã kết nối với máy tính)
        if (currentKeyboard != null)
        {
            // 1. Kiểm tra Trục X (Trái/Phải): Phím A/D hoặc Mũi tên Trái/Phải
            if (currentKeyboard.aKey.isPressed || currentKeyboard.leftArrowKey.isPressed) moveX = -1f;
            if (currentKeyboard.dKey.isPressed || currentKeyboard.rightArrowKey.isPressed) moveX = 1f;

            // 2. Kiểm tra Trục Z (Xa/Gần): Phím W/S hoặc Mũi tên Lên/Xuống
            if (currentKeyboard.sKey.isPressed || currentKeyboard.downArrowKey.isPressed) moveZ = -1f; // Tiến về phía mình
            if (currentKeyboard.wKey.isPressed || currentKeyboard.upArrowKey.isPressed) moveZ = 1f;  // Đi ra xa mình
        }

        // 3. Tạo Vector hướng di chuyển trong không gian 3D (Trục Y = 0 vì đi trên mặt đất)
        Vector3 direction = new Vector3(moveX, 0f, moveZ);

        // 4. Chuẩn hóa Vector để đi chéo không bị nhanh hơn đi thẳng
        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        // 5. Cập nhật vị trí mới cho khối Cube
        transform.position += direction * speed * Time.deltaTime;
    }

    void Update()
    {
        // Gọi hàm di chuyển liên tục mỗi khung hình
        move();
    }
}