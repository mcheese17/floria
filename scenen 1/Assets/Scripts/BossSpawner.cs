using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour
{
    [Header("Liên Kết Hệ Thống")]
    public Transform player; // Kéo Floria vào đây

    [Header("Cài đặt Vị trí Từng Tầng")]
    public Transform diemSpawnTang1;
    public Transform diemSpawnTang2;
    public Transform diemSpawnTang3;

    [Header("Kho Sâu Bọ")]
    public GameObject[] danhSachTatCaSau;
    public GameObject[] danhSachSauBay;

    [Header("Cấu hình Nhịp Tim Sinh Học")]
    public float nhipTimGoc = 4.0f;       // Lúc bình thường, 4s đập 1 nhịp
    public float nhipTimNhanhNhat = 1.5f; // Dù cuống cuồng cũng không nhanh hơn mức này
    public float buocNhayNhipTim = 0.5f;  // Mỗi lần sổng 1 con, tim đập nhanh hơn 0.5s

    [Header("Radar Chống Kẹt Xe")]
    public float khoangCachAnToan = 2.5f;

    // Âm thanh nhịp tim (Tùy chọn để game thêm phần rùng rợn)
    [Header("Âm thanh (Không bắt buộc)")]
    public AudioSource loaNhipTim;
    public AudioClip tiengThich;

    private GameObject lastWormT1, lastWormT2, lastWormT3;
    private GameManager trongTai;

    void Start()
    {
        trongTai = FindAnyObjectByType<GameManager>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        StartCoroutine(NhipTimThuyTo());
    }

    IEnumerator NhipTimThuyTo()
    {
        yield return new WaitForSeconds(2f); // Cho người chơi 2 giây thở lúc mới vào

        while (true)
        {
            // 1. TÍNH TOÁN TỐC ĐỘ NHỊP TIM DỰA TRÊN SỐ VẬT HIẾN TẾ ĐÃ SỔNG
            float thoiGianCho = nhipTimGoc;
            if (trongTai != null)
            {
                // Giới hạn đẻ: Nếu trên sân đã đủ số lượng quy định thì tạm nín chờ
                if (trongTai.tongSoSauDaSanXuat >= trongTai.tongSoSauDuocPhepSpawn)
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                // Tính toán độ cuồng nộ (soSauCanTieuDiet gốc là 25, mỗi lần phạt nó tăng lên 1)
                int soLanSong = trongTai.soSauCanTieuDiet - 25;

                // Trừ dần thời gian chờ. Hàm Max để đảm bảo không bao giờ đập nhanh hơn 1.5s
                thoiGianCho = Mathf.Max(nhipTimNhanhNhat, nhipTimGoc - (soLanSong * buocNhayNhipTim));
            }

            // 2. TIM ĐẬP! (Phát âm thanh nếu có)
            if (loaNhipTim != null && tiengThich != null)
            {
                loaNhipTim.PlayOneShot(tiengThich);
            }

            // 3. ĐẠO DIỄN THÍCH NGHI: Quét vị trí Floria để ép góc
            int tangUuTien = QuetViTriPlayer();

            // 4. KIỂM TRA RADAR & THẢ QUÁI (Có kế hoạch dự phòng)
            ThucThiNhaQuai(tangUuTien);

            // 5. NGHỈ NGƠI CHỜ NHỊP TIM TIẾP THEO
            yield return new WaitForSeconds(thoiGianCho);
        }
    }

    // --- HÀM THÍCH NGHI HÀNH VI (ADAPTIVE AI) ---
    int QuetViTriPlayer()
    {
        if (player == null) return Random.Range(1, 4); // Nếu Player chết, random bừa

        // Nếu Floria ở dưới thấp (Tầng 1 hoặc mặt đất)
        if (player.position.y < diemSpawnTang2.position.y - 1f)
        {
            // 80% ép ra quái ở Tầng 3, 20% ra ở Tầng 2
            return (Random.value < 0.8f) ? 3 : 2;
        }
        // Nếu Floria đang ở trên cao (Tầng 3)
        else if (player.position.y > diemSpawnTang2.position.y + 1f)
        {
            // 80% ép ra quái ở Tầng 1, 20% ra ở Tầng 2
            return (Random.value < 0.8f) ? 1 : 2;
        }
        // Nếu Floria lơ lửng ở giữa (Tầng 2)
        else
        {
            // Ép phân tán hai đầu
            return (Random.value < 0.5f) ? 1 : 3;
        }
    }

    // --- HÀM AN TOÀN CHỐNG KẸT XE VÀ DỰ PHÒNG ---
    void ThucThiNhaQuai(int tangUuTien)
    {
        bool daNhaXong = false;

        // Thử nhả ở tầng ưu tiên trước
        daNhaXong = ThuNhaTaiTang(tangUuTien);

        // NẾU TẦNG ƯU TIÊN ĐANG KẸT XE -> KÍCH HOẠT KẾ HOẠCH B (Tìm tầng thoáng để nhả)
        if (!daNhaXong)
        {
            // Thử vòng lặp qua các tầng còn lại
            for (int i = 1; i <= 3; i++)
            {
                if (i != tangUuTien)
                {
                    daNhaXong = ThuNhaTaiTang(i);
                    if (daNhaXong) break; // Nhả được rồi thì thoát
                }
            }
        }
    }

    bool ThuNhaTaiTang(int tang)
    {
        if (tang == 1 && KiemTraAnToan(diemSpawnTang1, lastWormT1))
        {
            lastWormT1 = DeQuai(diemSpawnTang1, danhSachTatCaSau); return true;
        }
        else if (tang == 2 && KiemTraAnToan(diemSpawnTang2, lastWormT2))
        {
            lastWormT2 = DeQuai(diemSpawnTang2, danhSachSauBay); return true;
        }
        else if (tang == 3 && KiemTraAnToan(diemSpawnTang3, lastWormT3))
        {
            lastWormT3 = DeQuai(diemSpawnTang3, danhSachTatCaSau); return true;
        }
        return false; // Tầng này đang kẹt xe!
    }

    bool KiemTraAnToan(Transform diemSpawn, GameObject conSauCuoiCung)
    {
        if (conSauCuoiCung == null) return true;
        float khoangCach = Vector3.Distance(diemSpawn.position, conSauCuoiCung.transform.position);
        return khoangCach > khoangCachAnToan;
    }

    GameObject DeQuai(Transform diemSpawn, GameObject[] danhSach)
    {
        if (trongTai != null && danhSach.Length > 0)
        {
            GameObject prefabDuocChon = danhSach[Random.Range(0, danhSach.Length)];
            GameObject moi = Instantiate(prefabDuocChon, diemSpawn.position, diemSpawn.rotation);
            trongTai.tongSoSauDaSanXuat++;
            return moi;
        }
        return null;
    }
}