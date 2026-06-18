using UnityEngine;
using System.Collections.Generic; // Bắt buộc phải có để dùng Danh sách (List)

public class RandomSauScene2: MonoBehaviour
{
    [Header("Cài đặt Vị trí & Số lượng")]
    [Tooltip("Kéo thả tất cả các Diem_Spawn vào đây")]
    public Transform[] cacDiemSpawn;
    public int soLuongSauCanSinh = 2; // Số lượng sâu muốn xuất hiện mỗi lần chơi

    [Header("Kho Quái Vật (Prefabs)")]
    [Tooltip("Kéo thả Prefab Sâu Xanh, Sên... ở cửa sổ Project vào đây")]
    public GameObject[] danhSachLoaiSau;

    void Start()
    {
        // Kiểm tra an toàn: Nếu quên chưa nạp điểm spawn hoặc quên nạp prefab sâu thì báo lỗi và dừng lại
        if (cacDiemSpawn.Length == 0 || danhSachLoaiSau.Length == 0) return;

        // Chống lỗi ngớ ngẩn: Lỡ em đòi sinh 5 con mà chỉ có 3 điểm spawn
        int soLuongThucTe = Mathf.Min(soLuongSauCanSinh, cacDiemSpawn.Length);

        // Tạo một hộp bốc thăm chứa tất cả các điểm spawn
        List<Transform> hopBocTham = new List<Transform>(cacDiemSpawn);

        for (int i = 0; i < soLuongThucTe; i++)
        {
            // 1. Nhắm mắt bốc đại 1 vị trí trong hộp
            int viTriNgauNhien = Random.Range(0, hopBocTham.Count);
            Transform diemDuocChon = hopBocTham[viTriNgauNhien];

            // 2. Nhắm mắt bốc đại 1 loại sâu (Sên hoặc Sâu Xanh)
            int loaiSauNgauNhien = Random.Range(0, danhSachLoaiSau.Length);
            GameObject quaiVatDuocChon = danhSachLoaiSau[loaiSauNgauNhien];

            // 3. Phép thuật biến hình: Triệu hồi con sâu ra đúng vị trí đó
            Instantiate(quaiVatDuocChon, diemDuocChon.position, diemDuocChon.rotation);

            // 4. QUAN TRỌNG: Vứt cái điểm vừa bốc được ra khỏi hộp để con thứ 2 không bị sinh đè lên vị trí cũ!
            hopBocTham.RemoveAt(viTriNgauNhien);
        }
    }
}