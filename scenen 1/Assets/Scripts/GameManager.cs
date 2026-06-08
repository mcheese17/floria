using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Nhiệm vụ Boss")]
    [Tooltip("Số sâu ban đầu cần giết")]
    public int soSauCanTieuDiet = 25;

    // Biến này để nhớ xem mình đã giết được bao nhiêu con rồi (ẩn đi không cần hiển thị)
    private int soSauDaGiet = 0;

    // --- HÀM NÀY ĐƯỢC GỌI KHI BẮN TRÚNG SÂU ---
    public void GhiNhanSauChet()
    {
        soSauDaGiet++; // Cộng 1 điểm
        Debug.Log("✅ Đã diệt: " + soSauDaGiet + " / " + soSauCanTieuDiet);

        // Kiểm tra xem đã đủ chỉ tiêu chưa?
        if (soSauDaGiet >= soSauCanTieuDiet)
        {
            Debug.Log("🏆 CHIẾN THẮNG! (Chuẩn bị chuyển sang Cutscene)");
            // Bài sau mình sẽ nhét code chuyển cutscene vào đây
        }
    }

    // --- HÀM NÀY ĐƯỢC GỌI KHI SÂU BỎ TRỐN VÀO VÙNG TÍM ---
    public void GhiNhanSauTauThoat()
    {
        soSauCanTieuDiet++; // Hình phạt: Nâng chỉ tiêu lên 1 con
        Debug.Log("❌ BÁO ĐỘNG! Sổng 1 con! Hình phạt: Cần diệt tổng cộng " + soSauCanTieuDiet + " con.");
    }
}