using UnityEngine;
using System.Collections;

public class DaoDienCutsceneToanTap : MonoBehaviour
{
    [Header("Nhân Vật & Bọ Rùa")]
    public PlayerController scriptDiChuyen;
    public Animator animFloria;
    public BoRuaController scriptBoRua;

    [Header("Các Điểm Di Chuyển")]
    public Transform diemBoRuaNgui;
    public Transform diemFloriaDungHai;

    [Header("Góc Máy & Phim Ảnh")]
    public GameObject camZoomCutscene;
    public GameObject uiPhuDePhim;

    [Header("Âm Thanh & Thời Gian")]
    [Tooltip("Kéo Component AudioSource chứa giọng bọ rùa vào đây")]
    public AudioSource amThanhBoRua;
    [Tooltip("Thời gian đứng chờ nghe thoại (Căn chỉnh theo độ dài file âm thanh)")]
    public float thoiGianNgheThoai = 5f;

    private bool daKichHoat = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !daKichHoat)
        {
            daKichHoat = true;
            StartCoroutine(KichBanPhimDienAnh());
        }
    }

    IEnumerator KichBanPhimDienAnh()
    {
        // 1. KHÓA DI CHUYỂN, PHANH GẤP VÀ ÉP VỀ IDLE NGAY LẬP TỨC
        scriptDiChuyen.enabled = false;
        scriptDiChuyen.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        animFloria.SetFloat("Speed", 0f); // Sửa lỗi chạy tại chỗ: Ép chuyển sang Anim Idle

        // 2. ZOOM CAMERA 
        if (camZoomCutscene != null) camZoomCutscene.SetActive(true);

        // 3. BỌ RÙA BAY LÊN ĐIỂM NGỬI
        scriptBoRua.KichHoatBaoRuaNguiHoa(diemBoRuaNgui);

        // Chờ 1.5 giây cho camera zoom và bọ rùa bay tới vị trí nụ hoa
        yield return new WaitForSeconds(1.5f);

        // 4. HIỆN PHỤ ĐỀ VÀ PHÁT GIỌNG NÓI
        if (uiPhuDePhim != null) uiPhuDePhim.SetActive(true);
        if (amThanhBoRua != null) amThanhBoRua.Play();

        // Đứng im chờ mọi người nghe hết câu thoại
        yield return new WaitForSeconds(thoiGianNgheThoai);

        // 5. ĐỌC XONG -> TẮT PHỤ ĐỀ
        if (uiPhuDePhim != null) uiPhuDePhim.SetActive(false);

        // Chờ thêm 1.5 giây để bọ rùa bay nhường đường rồi Floria mới bước tới
        yield return new WaitForSeconds(1.5f);

        // 6. FLORIA ĐI BỘ LẠI GẦN CÂY SÚNG
        animFloria.SetFloat("Speed", 1f); // Bật Anim đi bộ

        while (Vector3.Distance(new Vector3(scriptDiChuyen.transform.position.x, 0, 0),
                                new Vector3(diemFloriaDungHai.position.x, 0, 0)) > 0.1f)
        {
            scriptDiChuyen.transform.position = Vector3.MoveTowards(
                scriptDiChuyen.transform.position,
                new Vector3(diemFloriaDungHai.position.x, scriptDiChuyen.transform.position.y, diemFloriaDungHai.position.z),
                1.5f * Time.deltaTime
            );
            yield return null;
        }

        // Đến nơi -> Chuyển sang Anim Idle
        animFloria.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(0.5f);

        // 7. CHUYỂN SANG ANIM LẤY SÚNG 
        animFloria.Play("ReachOut");
        yield return new WaitForSeconds(2f);

        // 8. HẾT CUTSCENE: TRẢ LẠI BÀN PHÍM
        if (camZoomCutscene != null) camZoomCutscene.SetActive(false);
        scriptDiChuyen.enabled = true;

        Destroy(this.gameObject);
    }
}