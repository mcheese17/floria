using UnityEngine;

public class KichHoatHoaBay : MonoBehaviour
{
    private ParticleSystem hieuUngHoa;

    [Tooltip("Tích vào đây nếu em muốn hoa CHỈ bay 1 lần duy nhất khi chạm vào. Bỏ tích nếu muốn dẫm lên lần nào bay lần đó.")]
    public bool chiBayMotLan = false;

    private bool daKichHoat = false;

    void Start()
    {
        // Tự động tìm và lấy cái Particle System đang gắn trên cùng Object này
        hieuUngHoa = GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem thứ vừa dẫm vào bẫy có phải là Floria không (thông qua Tag "Player")
        if (other.CompareTag("Player"))
        {
            if (chiBayMotLan && daKichHoat)
                return; // Nếu đã bay rồi và cài đặt chỉ bay 1 lần thì dừng lại luôn

            hieuUngHoa.Play(); // BÙM! Kích hoạt hoa bay
            daKichHoat = true;
        }
    }
}