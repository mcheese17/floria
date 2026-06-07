using UnityEngine;
using TMPro;
using System.Collections;

public class DaoDienCutsceneNPC : MonoBehaviour
{
    [Header("Nhân Vật & Đạo Cụ")]
    public PlayerController scriptDiChuyen;
    public Animator animFloria;
    public Animator animNPC;

    [Header("Các Điểm Di Chuyển (NÂNG CẤP)")]
    public Transform diemFloriaDungNoiChuyen; // Kéo điểm đứng vào đây

    [Header("Góc Máy & Phim Ảnh")]
    public GameObject camZoomNPC;
    public GameObject uiPhuDePhim;
    public TextMeshProUGUI textPhuDe;
    public AudioSource nguonAmThanh;

    [Header("File Âm Thanh")]
    public AudioClip audioFloria1;
    public AudioClip audioNPC1;
    public AudioClip audioFloria2;
    public AudioClip audioNPC2;

    [Header("Thời gian chờ mỗi câu (Giây)")]
    public float timeCau1 = 3f;
    public float timeCau2 = 8f;
    public float timeCau3 = 2f;
    public float timeCau4 = 4f;

    [Header("Cài đặt Độ Mượt")]
    public float thoiGianChuyenAnim = 0.25f;
    public float thoiGianNghiGiuaCau = 0.5f;

    private bool daKichHoat = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !daKichHoat)
        {
            daKichHoat = true;
            StartCoroutine(ChayCutsceneNPC());
        }
    }

    IEnumerator ChayCutsceneNPC()
    {
        // 1. KHÓA DI CHUYỂN & ÉP ĐỨNG IM
        scriptDiChuyen.enabled = false;
        scriptDiChuyen.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        animFloria.SetFloat("Speed", 0f);

        // FIX LỖI CHỮ CŨ: Tẩy trắng dòng thoại trước khi bật UI
        if (textPhuDe != null) textPhuDe.text = "";

        // 2. CHUYỂN GÓC MÁY & HIỆN KHUNG PHỤ ĐỀ
        if (camZoomNPC != null) camZoomNPC.SetActive(true);
        if (uiPhuDePhim != null) uiPhuDePhim.SetActive(true);

        yield return new WaitForSeconds(1f); // Chờ camera zoom tới nơi

        // --- NÂNG CẤP: FLORIA TỰ ĐI BỘ VÀO VỊ TRÍ ---
        if (diemFloriaDungNoiChuyen != null)
        {
            animFloria.SetFloat("Speed", 1f); // Bật dáng đi bộ

            // Ép Floria tự động đi đến Điểm Đứng Nói Chuyện
            while (Vector3.Distance(new Vector3(scriptDiChuyen.transform.position.x, 0, 0),
                                    new Vector3(diemFloriaDungNoiChuyen.position.x, 0, 0)) > 0.1f)
            {
                scriptDiChuyen.transform.position = Vector3.MoveTowards(
                    scriptDiChuyen.transform.position,
                    new Vector3(diemFloriaDungNoiChuyen.position.x, scriptDiChuyen.transform.position.y, diemFloriaDungNoiChuyen.position.z),
                    1f * Time.deltaTime // Tốc độ đi bộ
                );
                yield return null;
            }

            // Đến nơi -> Chuyển sang Anim Idle một cách từ từ
            animFloria.SetFloat("Speed", 0f);
            yield return new WaitForSeconds(0.5f); // Đứng tĩnh lại 1 chút rồi mới mở lời
        }

        // --- CÂU 1: FLORIA NÓI ---
        animNPC.CrossFadeInFixedTime("Sitting Talking", thoiGianChuyenAnim);
        animFloria.CrossFadeInFixedTime("Talking_Floria", thoiGianChuyenAnim);
        textPhuDe.text = "Why are you so downcast?";
        nguonAmThanh.clip = audioFloria1; nguonAmThanh.Play();
        yield return new WaitForSeconds(timeCau1);

        // NGHỈ GIỮA NHỊP 1
        textPhuDe.text = "";
        yield return new WaitForSeconds(thoiGianNghiGiuaCau);

        // --- CÂU 2: NPC NÓI ---
        animFloria.CrossFadeInFixedTime("Locomotion", thoiGianChuyenAnim);
        animFloria.SetFloat("Speed", 0f);

        animNPC.CrossFadeInFixedTime("Sitting Talking", thoiGianChuyenAnim);
        textPhuDe.text = "Our crops are devastated. No matter what we do, we cannot drive them off. Please, lend us your aid. If this continues, we cannot survive.";
        nguonAmThanh.clip = audioNPC1; nguonAmThanh.Play();
        yield return new WaitForSeconds(timeCau2);

        // NGHỈ GIỮA NHỊP 2
        textPhuDe.text = "";
        yield return new WaitForSeconds(thoiGianNghiGiuaCau);

        // --- CÂU 3: FLORIA NÓI ---
        animNPC.CrossFadeInFixedTime("Sitting Talking", thoiGianChuyenAnim);
        animFloria.CrossFadeInFixedTime("Talking_Floria", thoiGianChuyenAnim);
        textPhuDe.text = "Alright, I’ll help you.";
        nguonAmThanh.clip = audioFloria2; nguonAmThanh.Play();
        yield return new WaitForSeconds(timeCau3);

        // NGHỈ GIỮA NHỊP 3
        textPhuDe.text = "";
        yield return new WaitForSeconds(thoiGianNghiGiuaCau);

        // --- CÂU 4: NPC NÓI ---
        animFloria.CrossFadeInFixedTime("Locomotion", thoiGianChuyenAnim);
        animFloria.SetFloat("Speed", 0f);

        animNPC.CrossFadeInFixedTime("Sitting Talking", thoiGianChuyenAnim);
        textPhuDe.text = "Just keep heading forward and you’ll see. The region is crawling with vermin.";
        nguonAmThanh.clip = audioNPC2; nguonAmThanh.Play();
        yield return new WaitForSeconds(timeCau4);

        // NGHỈ TRƯỚC KHI TẮT HẲN
        textPhuDe.text = "";
        yield return new WaitForSeconds(thoiGianNghiGiuaCau);

        // 3. KẾT THÚC CUTSCENE
        if (uiPhuDePhim != null) uiPhuDePhim.SetActive(false);
        if (camZoomNPC != null) camZoomNPC.SetActive(false);

        animNPC.CrossFadeInFixedTime("Sitting1", thoiGianChuyenAnim);
        scriptDiChuyen.enabled = true;

        Destroy(this.gameObject);
    }
}