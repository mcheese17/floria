using UnityEngine;

public class VienDan : MonoBehaviour
{
    public float tocDo = 10f;
    public float thoiGianTuHuy = 3f;

    // Biến này bị ẩn đi, nó sẽ nhận lệnh từ Floria truyền sang
    [HideInInspector] public Vector3 huongBay = Vector3.right;

    void Start()
    {
        // Tự xóa sau 3s nếu không trúng gì
        Destroy(gameObject, thoiGianTuHuy);
    }

    void Update()
    {
        // Dùng Space.World để đạn bay chuẩn theo trục tọa độ của môi trường
        transform.Translate(huongBay * tocDo * Time.deltaTime, Space.World);

        // Khóa cứng đạn ở trục Z = 0 để không bị bắn trượt sâu bọ
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SauBo"))
        {
            Destroy(gameObject);
        }
    }
}