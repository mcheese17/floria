using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour
{
    [Header("Cài đặt Vị trí (Các điểm Xanh lá)")]
    public Transform[] cacDiemSpawn;

    [Header("Kho Sâu Bọ")]
    public GameObject[] danhSachLoaiSau;

    [Header("Nhịp độ nhả quái")]
    [Tooltip("Khoảng thời gian (giây) giữa 2 lần thả sâu")]
    public float thoiGianGiua2LanTha = 3f;

    private bool dangChoPhepTha = true; // Công tắc để sau này Boss chết thì dừng thả

    void Start()
    {
        // Vừa vào màn là bắt đầu kịch bản thả sâu luôn
        StartCoroutine(ThongDichThaSau());
    }

    IEnumerator ThongDichThaSau()
    {
        // Vòng lặp vô tận: Cứ chờ hết thời gian -> Thả 1 con -> Lại chờ -> Lại thả
        while (dangChoPhepTha)
        {
            // Nghỉ một chút theo thời gian quy định
            yield return new WaitForSeconds(thoiGianGiua2LanTha);

            // Kiểm tra an toàn xem có lỡ quên nạp điểm hay nạp sâu không
            if (cacDiemSpawn.Length > 0 && danhSachLoaiSau.Length > 0)
            {
                // 1. Nhắm mắt bốc Random 1 trong 3 tầng (Điểm Xanh)
                int viTriRandom = Random.Range(0, cacDiemSpawn.Length);
                Transform diemDuocChon = cacDiemSpawn[viTriRandom];

                // 2. Nhắm mắt bốc Random Sên hoặc Sâu Xanh
                int loaiSauRandom = Random.Range(0, danhSachLoaiSau.Length);
                GameObject loaiSauDuocChon = danhSachLoaiSau[loaiSauRandom];

                // 3. Phép thuật biến hình: Thả con sâu ra sân khấu
                Instantiate(loaiSauDuocChon, diemDuocChon.position, diemDuocChon.rotation);
            }
        }
    }
}