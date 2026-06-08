using UnityEngine;

public class SauCamTu : MonoBehaviour
{
    [Header("Cài đặt")]
    [Tooltip("Tốc độ bò sang bên trái")]
    public float tocDo = 3f;

    void Update()
    {
        // Cứ sinh ra là cắm đầu bò thẳng sang bên trái (trục X âm)
        transform.Translate(Vector3.right * tocDo * Time.deltaTime, Space.World);
    }

    // Xử lý các pha va chạm
    // Xử lý các pha va chạm
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VienDan"))
        {
            // GỌI ĐIỆN BÁO TRỌNG TÀI CỘNG ĐIỂM
            FindAnyObjectByType<GameManager>().GhiNhanSauChet();

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("💀 Floria đã chạm sâu! (Game Over)");
        }
        else if (other.CompareTag("LoiThoat"))
        {
            // GỌI ĐIỆN BÁO TRỌNG TÀI PHẠT ĐIỂM
            FindAnyObjectByType<GameManager>().GhiNhanSauTauThoat();

            Destroy(this.gameObject);
        }
    }
}