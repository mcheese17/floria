using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DaoDienCutsceneBoss : MonoBehaviour
{
    [Header("Nhân Vật & Đạo Cụ")]
    public MonoBehaviour scriptDiChuyenFloria;
    public Rigidbody rbFloria;
    public Animator animFloria;
    public Animator animBoss;
    public GameObject ongCutscene;

    [Header("Các Điểm Neo")]
    public Transform diemFloriaDung;
    // --- THÊM MỚI 1: ĐIỂM ÔNG ĐI VỀ ---
    public Transform diemOngXuatHien;

    [Header("Máy Quay & Giao Diện")]
    public GameObject camChinh;
    public GameObject camCutscene;
    public GameObject uiKhungThoai;
    public TextMeshProUGUI textPhuDe;
    public CanvasGroup manHinhDen;

    [Header("Âm Thanh & Hiệu Ứng")]
    public AudioSource loaThoai;
    public AudioClip[] danhSachAmThanh = new AudioClip[9];

    // --- THÊM MỚI 2: HIỆU ỨNG NỔ ---
    public GameObject prefabHieuUngNo;

    [Tooltip("Em tự gõ số giây tương ứng với độ dài từng file âm thanh vào đây nhé!")]
    public float[] thoiGianCho = new float[9] { 6f, 2f, 9f, 6f, 3f, 4f, 6f, 6f, 6f };

    private bool daKichHoat = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !daKichHoat)
        {
            daKichHoat = true;
            // Đạp xong hủy luôn bẫy để cực kỳ an toàn
            GetComponent<Collider>().enabled = false;
            StartCoroutine(KichBanPhimDienAnh());
        }
    }

    IEnumerator KichBanPhimDienAnh()
    {
        // ==========================================
        // PHẦN 1: SETUP MÁY QUAY VÀ DI CHUYỂN
        // ==========================================
        if (scriptDiChuyenFloria != null) scriptDiChuyenFloria.enabled = false;
        if (rbFloria != null) rbFloria.linearVelocity = Vector3.zero;
        animFloria.SetFloat("Speed", 0f);
        animFloria.Play("Idle");

        if (camChinh != null) camChinh.SetActive(false);
        if (camCutscene != null) camCutscene.SetActive(true);

        yield return new WaitForSeconds(1f);

        // Floria đi bộ vào vị trí
        if (diemFloriaDung != null)
        {
            animFloria.SetFloat("Speed", 1f);
            Transform phomFloria = scriptDiChuyenFloria.transform;
            while (Vector3.Distance(new Vector3(phomFloria.position.x, 0, 0), new Vector3(diemFloriaDung.position.x, 0, 0)) > 0.1f)
            {
                phomFloria.position = Vector3.MoveTowards(phomFloria.position, new Vector3(diemFloriaDung.position.x, phomFloria.position.y, phomFloria.position.z), 3f * Time.deltaTime);
                yield return null;
            }
            animFloria.SetFloat("Speed", 0f);
            animFloria.Play("Idle");
            yield return new WaitForSeconds(0.5f);
        }

        if (uiKhungThoai != null) uiKhungThoai.SetActive(true);

        // ==========================================
        // PHẦN 2: CHẠY THOẠI TUẦN TỰ TỪNG CÂU
        // ==========================================

        // --- CÂU 1: QUÁI VẬT NÓI ---
        animFloria.Play("Idle");
        animBoss.CrossFadeInFixedTime("BossTalking", 0.2f);
        textPhuDe.text = "Quái vật: Rất nhiều năm trước, trong một mùa đông kéo dài, tôi đã mất đi đôi cánh của mình. Từ đó tôi mắc phải một căn bệnh kỳ lạ, có thể sinh ra sâu bọ.";
        if (loaThoai != null && danhSachAmThanh[0] != null) { loaThoai.Stop(); loaThoai.clip = danhSachAmThanh[0]; loaThoai.Play(); }
        yield return new WaitForSeconds(thoiGianCho[0]);
        textPhuDe.text = ""; yield return new WaitForSeconds(0.5f);

        // --- CÂU 2: FLORIA NÓI ---
        animBoss.CrossFadeInFixedTime("Orc Idle", 0.2f);
        animFloria.CrossFadeInFixedTime("Talking_Floria", 0.2f);
        textPhuDe.text = "Floria: Vậy sao?";
        if (loaThoai != null && danhSachAmThanh[1] != null) { loaThoai.Stop(); loaThoai.clip = danhSachAmThanh[1]; loaThoai.Play(); }
        yield return new WaitForSeconds(thoiGianCho[1]);
        textPhuDe.text = ""; yield return new WaitForSeconds(0.5f);

        // --- CÂU 3: QUÁI VẬT NÓI ---
        animFloria.Play("Idle");
        animBoss.CrossFadeInFixedTime("BossTalking2", 0.2f);
        textPhuDe.text = "Quái vật: Khi mùa xuân trở lại, tôi không còn khả năng làm những công việc trước đây. Tôi phải làm những việc nặng nhọc nhưng vẫn bị khinh thường và xa lánh. Rồi gia đình cũng bỏ tôi mà đi. Không tìm được nơi nào chấp nhận mình, tôi quyết định gieo rắc đại dịch sâu bọ để cả vùng đất phải chịu nỗi đau giống như tôi.";
        if (loaThoai != null && danhSachAmThanh[2] != null) { loaThoai.Stop(); loaThoai.clip = danhSachAmThanh[2]; loaThoai.Play(); }
        yield return new WaitForSeconds(thoiGianCho[2]);
        textPhuDe.text = ""; yield return new WaitForSeconds(0.5f);

        // --- CÂU 4: FLORIA NÓI ---
        animBoss.CrossFadeInFixedTime("Orc Idle", 0.2f);
        animFloria.CrossFadeInFixedTime("Talking_Floria", 0.2f);
        textPhuDe.text = "Floria: Tôi cũng là một sinh vật đã mất cánh. Những tiên khác đều có đôi cánh của riêng mình, còn tôi vẫn đang trên hành trình tìm lại đôi cánh đã mất.";
        if (loaThoai != null && danhSachAmThanh[3] != null) { loaThoai.Stop(); loaThoai.clip = danhSachAmThanh[3]; loaThoai.Play(); }
        yield return new WaitForSeconds(thoiGianCho[3]);
        textPhuDe.text = ""; yield return new WaitForSeconds(0.5f);

        // --- CÂU 5: QUÁI VẬT NÓI ---
        animFloria.Play("Idle");
        animBoss.CrossFadeInFixedTime("BossTalking", 0.2f);
        textPhuDe.text = "Quái vật: Cô là tiên mà lại không có cánh sao?";
        if (loaThoai != null && danhSachAmThanh[4] != null) { loaThoai.Stop(); loaThoai.clip = danhSachAmThanh[4]; loaThoai.Play(); }
        yield return new WaitForSeconds(thoiGianCho[4]);
        textPhuDe.text = ""; yield return new WaitForSeconds(0.5f);

        // --- CÂU 6: FLORIA NÓI ---
        animBoss.CrossFadeInFixedTime("Orc Idle", 0.2f);
        animFloria.CrossFadeInFixedTime("Talking_Floria", 0.2f);
        textPhuDe.text = "Floria: Đúng vậy. Nhưng tôi không thể làm tổn thương người khác chỉ để xoa dịu nỗi đau của bản thân.";
        if (loaThoai != null && danhSachAmThanh[5] != null) { loaThoai.Stop(); loaThoai.clip = danhSachAmThanh[5]; loaThoai.Play(); }
        yield return new WaitForSeconds(thoiGianCho[5]);
        textPhuDe.text = ""; yield return new WaitForSeconds(0.5f);

        // --- CÂU 7: QUÁI VẬT NÓI ---
        animFloria.Play("Idle");
        animBoss.CrossFadeInFixedTime("BossTalking2", 0.2f);
        textPhuDe.text = "Quái vật: Có lẽ tôi đã sai. Sau tất cả những gì mình gây ra, tôi chẳng còn điều gì để theo đuổi nữa. Tôi đã quen với việc trở thành kẻ xấu đến mức quên mất mình từng là ai.";
        if (loaThoai != null && danhSachAmThanh[6] != null) { loaThoai.Stop(); loaThoai.clip = danhSachAmThanh[6]; loaThoai.Play(); }
        yield return new WaitForSeconds(thoiGianCho[6]);
        textPhuDe.text = ""; yield return new WaitForSeconds(0.5f);

        // --- CÂU 8: FLORIA NÓI ---
        animBoss.CrossFadeInFixedTime("Orc Idle", 0.2f);
        animFloria.CrossFadeInFixedTime("Talking_Floria", 0.2f);
        textPhuDe.text = "Floria: Tôi đang đi tìm một bà tiên có thể tạo ra đôi cánh mới. Có lẽ anh cũng nên đi tìm lại ý nghĩa cuộc sống của mình, thay vì bắt người khác chịu chung nỗi đau.";
        if (loaThoai != null && danhSachAmThanh[7] != null) { loaThoai.Stop(); loaThoai.clip = danhSachAmThanh[7]; loaThoai.Play(); }
        yield return new WaitForSeconds(thoiGianCho[7]);
        textPhuDe.text = ""; yield return new WaitForSeconds(0.5f);

        // --- CÂU 9: QUÁI VẬT NÓI CUỐI CÙNG ---
        animFloria.Play("Idle");
        animBoss.CrossFadeInFixedTime("BossTalking", 0.2f);
        textPhuDe.text = "Quái vật: Tôi đã sống quá lâu rồi, không còn đủ sức nữa. Nhưng cảm ơn cô, Floria. Cô đã cứu rỗi tâm hồn tôi. Chúc cô sớm tìm được điều mình đang kiếm tìm.";
        if (loaThoai != null && danhSachAmThanh[8] != null) { loaThoai.Stop(); loaThoai.clip = danhSachAmThanh[8]; loaThoai.Play(); }
        yield return new WaitForSeconds(thoiGianCho[8]);
        textPhuDe.text = ""; yield return new WaitForSeconds(0.5f);


        // ==========================================
        // PHẦN 3: CAMERA GIẬT LẠI -> ONG QUAY LƯNG ĐI
        // ==========================================
        uiKhungThoai.SetActive(false);
        animBoss.CrossFadeInFixedTime("Orc Idle", 0.2f);
        animFloria.Play("Idle");

        // 1. Trả Camera về góc nhìn toàn cảnh (Cam 1)
        if (camCutscene != null) camCutscene.SetActive(false);
        if (camChinh != null) camChinh.SetActive(true);

        // Chờ 1.5 giây để người chơi ngỡ ngàng với góc nhìn rộng
        yield return new WaitForSeconds(1.5f);

        // 2. Con Ong xoay người 180 độ (Quay lưng lại)
        if (ongCutscene != null)
        {
            ongCutscene.transform.Rotate(0, 180, 0);
            animBoss.CrossFadeInFixedTime("BossWalking", 0.2f); // Bật dáng đi bộ

            // 3. Con Ong lết bộ về lại DiemOngXuatHien
            if (diemOngXuatHien != null)
            {
                while (Vector3.Distance(new Vector3(ongCutscene.transform.position.x, 0, 0), new Vector3(diemOngXuatHien.position.x, 0, 0)) > 0.1f)
                {
                    ongCutscene.transform.position = Vector3.MoveTowards(
                        ongCutscene.transform.position,
                        new Vector3(diemOngXuatHien.position.x, ongCutscene.transform.position.y, ongCutscene.transform.position.z),
                        2.5f * Time.deltaTime // Tốc độ đi về (em có thể chỉnh số 2.5f này)
                    );
                    yield return null;
                }
            }

            // ==========================================
            // PHẦN 4: NỔ TUNG & KẾT THÚC SCENE
            // ==========================================
            if (prefabHieuUngNo != null)
            {
                // Tọa độ mới: Giữ nguyên X, Z nhưng lấy Y của con Ong trừ đi một khoảng (ví dụ 0.5f)
                Vector3 viTriNo = ongCutscene.transform.position;
                viTriNo.y -= 3f; // Số 0.5f này càng lớn thì hiệu ứng càng nằm thấp xuống dưới

                GameObject vuNo = Instantiate(prefabHieuUngNo, viTriNo, Quaternion.identity);
                Destroy(vuNo, 5f);
            }

            // Xóa sổ con Ong khỏi màn hình
            ongCutscene.SetActive(false);
        }

        // Chờ 2 giây để người chơi nhìn vụ nổ xong xuôi
        yield return new WaitForSeconds(2f);

        // Sập màn hình đen
        if (manHinhDen != null)
        {
            float tg = 0;
            while (tg < 1.5f)
            {
                tg += Time.deltaTime;
                manHinhDen.alpha = Mathf.Lerp(0, 1, tg / 1.5f);
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Vung_Chuyen_Scene");
    }
}