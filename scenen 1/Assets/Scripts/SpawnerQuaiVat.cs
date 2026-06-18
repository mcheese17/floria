using UnityEngine;
using System.Collections;

public class SpawnerQuaiVat : MonoBehaviour
{
    [Header("Cài đặt Vị trí 3 Tầng")]
    public Transform diemSpawnTang1;
    public Transform diemSpawnTang2; // Khuyên dùng tầng 2 chỉ đẻ sâu bay
    public Transform diemSpawnTang3;

    [Header("Kho Quái Vật")]
    public GameObject[] danhSachTatCaSau;
    public GameObject[] danhSachSauBay;

    [Header("Nhịp độ thả quái")]
    public float thoiGianGiua2LanTha = 3f;

    private int soSauDaSpawn = 0; // Bộ đếm số lượng đã xuất xưởng
    private GameManager trongTai;

    void Start()
    {
        trongTai = FindAnyObjectByType<GameManager>();
        StartCoroutine(SanXuatQuaiVat());
    }

    IEnumerator SanXuatQuaiVat()
    {
        // Chạy vô tận trong suốt màn chơi
        while (true)
        {
            yield return new WaitForSeconds(thoiGianGiua2LanTha);

            // CẦU DAO CHÍNH: Chỉ đẻ nếu chưa hết hạn ngạch
            if (trongTai != null && soSauDaSpawn < trongTai.tongSoSauDuocPhepSpawn)
            {
                if (danhSachTatCaSau.Length > 0 && danhSachSauBay.Length > 0)
                {
                    int tangRandom = Random.Range(1, 4);
                    Transform diemDuocChon = null;
                    GameObject loaiSauDuocChon = null;

                    if (tangRandom == 1)
                    {
                        diemDuocChon = diemSpawnTang1;
                        loaiSauDuocChon = danhSachTatCaSau[Random.Range(0, danhSachTatCaSau.Length)];
                    }
                    else if (tangRandom == 2)
                    {
                        diemDuocChon = diemSpawnTang2;
                        loaiSauDuocChon = danhSachSauBay[Random.Range(0, danhSachSauBay.Length)];
                    }
                    else if (tangRandom == 3)
                    {
                        diemDuocChon = diemSpawnTang3;
                        loaiSauDuocChon = danhSachTatCaSau[Random.Range(0, danhSachTatCaSau.Length)];
                    }

                    if (diemDuocChon != null && loaiSauDuocChon != null)
                    {
                        Instantiate(loaiSauDuocChon, diemDuocChon.position, diemDuocChon.rotation);
                        soSauDaSpawn++; // Cập nhật sổ sách
                    }
                }
            }
        }
    }
}