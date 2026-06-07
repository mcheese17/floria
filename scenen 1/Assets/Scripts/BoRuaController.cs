using UnityEngine;
using System.Collections;

public class BoRuaController : MonoBehaviour
{
    [Header("Cài đặt Theo đuôi Floria")]
    public Transform floria;
    [Tooltip("Khoảng cách bọ rùa bay phía sau Floria (Trục X)")]
    public float khoangCachX = 2f;
    public float tocDoBay = 3f;

    [Header("Chỉnh góc xoay")]
    public float gocQuaySangPhai = -90f;
    public float gocQuaySangTrai = 90f;

    private float chieuSauZCoDinh;
    private float khoangCachYBanDau;

    [Header("UI Câu thoại (Cutscene)")]
    public GameObject khungThoaiBoRua;

    private bool dangTrongCutscene = false;
    private Transform diemNguiHoa;

    void Start()
    {
        // Khóa cứng chiều sâu Z
        chieuSauZCoDinh = transform.position.z;

        // Lưu lại độ cao Y mà em đã xếp ngoài Scene
        khoangCachYBanDau = transform.position.y - floria.position.y;

        if (khungThoaiBoRua != null) khungThoaiBoRua.SetActive(false);
    }

    void Update()
    {
        Vector3 viTriMucTieu;

        if (!dangTrongCutscene)
        {
            // --- TRẠNG THÁI 1: BAY THEO FLORIA ---
            float huongMatFloria = floria.forward.x > 0 ? 1f : -1f;
            float viTriXCanDen = floria.position.x - (huongMatFloria * Mathf.Abs(khoangCachX));
            float viTriYCanDen = floria.position.y + khoangCachYBanDau;

            viTriMucTieu = new Vector3(viTriXCanDen, viTriYCanDen, chieuSauZCoDinh);

            // TUYỆT CHIÊU SỬA LỖI LẬT MẶT: Luôn nhìn cùng hướng với Floria
            if (huongMatFloria > 0)
            {
                transform.rotation = Quaternion.Euler(0, gocQuaySangPhai, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, gocQuaySangTrai, 0);
            }
        }
        else
        {
            // --- TRẠNG THÁI 2: CUTSCENE NGỬI HOA ---
            viTriMucTieu = diemNguiHoa.position;

            // Lúc này tách đoàn đi riêng, nên bọ rùa sẽ tự xoay mặt theo hướng nó bay đến bông hoa
            if (viTriMucTieu.x > transform.position.x + 0.05f)
            {
                transform.rotation = Quaternion.Euler(0, gocQuaySangPhai, 0);
            }
            else if (viTriMucTieu.x < transform.position.x - 0.05f)
            {
                transform.rotation = Quaternion.Euler(0, gocQuaySangTrai, 0);
            }
        }

        // Dùng Lerp di chuyển mượt mà đến mục tiêu
        transform.position = Vector3.Lerp(transform.position, viTriMucTieu, tocDoBay * Time.deltaTime);
    }

    public void KichHoatBaoRuaNguiHoa(Transform viTriDiemNgui)
    {
        StartCoroutine(KichBanhDienHoat(viTriDiemNgui));
    }

    IEnumerator KichBanhDienHoat(Transform viTriDiemNgui)
    {
        dangTrongCutscene = true;
        diemNguiHoa = viTriDiemNgui;

        yield return new WaitForSeconds(1f);

        if (khungThoaiBoRua != null) khungThoaiBoRua.SetActive(true);

        yield return new WaitForSeconds(5f);

        if (khungThoaiBoRua != null) khungThoaiBoRua.SetActive(false);
        dangTrongCutscene = false; // Xong việc, tự động bay lại về sau lưng Floria
    }
}