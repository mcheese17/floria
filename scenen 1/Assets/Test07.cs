using UnityEngine;

public class Test07 : MonoBehaviour
{
    [Header("Cấu hình di chuyển 3D")]
    [SerializeField] private float speed = 5f;

    void move()
    {
        // 1. Lấy dữ liệu phím bấm từ người chơi
        float moveX = Input.GetAxisRaw("Horizontal"); // Trục X: Phím A/D hoặc Mũi tên Trái/Phải
        float moveZ = Input.GetAxisRaw("Vertical");   // Trục Z: Phím W/S hoặc Mũi tên Lên/Xuống

        // 2. Tạo Vector hướng trong không gian 3D (Trục Y bằng 0 vì vật đi trên mặt đất)
        // Bấm W (moveZ = 1): Vật tiến ra xa màn hình (về phía trước)
        // Bấm S (moveZ = -1): Vật TIẾN VỀ PHÍA MÌNH (đi lùi lại)
        Vector3 direction = new Vector3(moveX, 0f, moveZ);

        // 3. Chuẩn hóa Vector để tốc độ đi chéo không bị nhanh hơn đi thẳng
        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        // 4. Dịch chuyển khối Cube trong không gian 3D
        transform.position += direction * speed * Time.deltaTime;
    }

    void Update()
    {
        move();
    }
}
