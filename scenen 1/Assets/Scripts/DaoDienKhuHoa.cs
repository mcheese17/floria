using UnityEngine;

public class DaoDienKhuHoa : MonoBehaviour
{
    public BoRuaController scriptBoRua;
    public Transform diemBoRuaCanDen; // Kéo Diem_BoRua_Ngui vào đây

    private bool daKichHoat = false;

    void OnTriggerEnter(Collider other)
    {
        // Khi Floria dẫm vào vùng này lần đầu tiên
        if (other.CompareTag("Player") && !daKichHoat)
        {
            daKichHoat = true;
            // Gọi bọ rùa bay lên trước thực hiện nhiệm vụ
            scriptBoRua.KichHoatBaoRuaNguiHoa(diemBoRuaCanDen);
        }
    }
}