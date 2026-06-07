using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CoCheDuDay : MonoBehaviour
{
    [Header("Liên kết Component")]
    public Animator animatorNhanVat;
    public Rigidbody rbNhanVat;
    public PlayerController scriptDiChuyenGoc;

    [Header("Cài đặt Lắc & Nhảy")]
    public float lucVangDay = 80f;
    public float gocVangToiDa = 70f;
    public Vector2 lucNhayBuongDay = new Vector2(10f, 6f);

    [Header("Cài đặt Trèo Từng Nấc")]
    public float khoangCach1Nac = 0.3f;
    public float thoiGian1Nac = 0.3f;

    [Header("Cài đặt Nam Châm Hút Dây")]
    [Tooltip("Chỉnh X hoặc Z để tay Floria nắm vừa khít dây, tránh bị dây đâm xuyên người")]
    public Vector3 lechViTriBam = new Vector3(0f, 0f, 0f);

    private bool dangBamDay = false;
    private bool dangDichChuyenNac = false;

    private float thoiGianHoiDay = 0f;
    private Transform dayVuaBuong;

    private Transform trucDayHienTai;
    private float vanTocVang = 0f;
    private float gocHienTai = 0f;

    void Update()
    {
        if (trucDayHienTai != null)
        {
            float vangInput = 0f;
            if (dangBamDay)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) vangInput = 1f;
                else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) vangInput = -1f;
            }

            vanTocVang += vangInput * lucVangDay * Time.deltaTime;
            vanTocVang -= gocHienTai * 5f * Time.deltaTime;
            vanTocVang *= 0.98f;

            gocHienTai += vanTocVang * Time.deltaTime;
            gocHienTai = Mathf.Clamp(gocHienTai, -gocVangToiDa, gocVangToiDa);
            trucDayHienTai.localRotation = Quaternion.Euler(0, 0, gocHienTai);
        }

        if (!dangBamDay)
        {
            if (thoiGianHoiDay > 0) thoiGianHoiDay -= Time.deltaTime;
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            BuongDayNhay();
            return;
        }

        if (!dangDichChuyenNac)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                StartCoroutine(TrèoTừngNấc(1f));
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                StartCoroutine(TrèoTừngNấc(-1f));
            else
                animatorNhanVat.SetFloat("TocDoLeo", 0);
        }
    }

    IEnumerator TrèoTừngNấc(float huong)
    {
        dangDichChuyenNac = true;
        animatorNhanVat.SetFloat("TocDoLeo", huong);

        float tg = 0;
        Vector3 viTriBatDau = transform.localPosition;
        Vector3 viTriKetThuc = viTriBatDau + Vector3.up * huong * khoangCach1Nac;

        while (tg < thoiGian1Nac)
        {
            tg += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(viTriBatDau, viTriKetThuc, tg / thoiGian1Nac);
            yield return null;
        }

        transform.localPosition = viTriKetThuc;
        dangDichChuyenNac = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rope") && !dangBamDay)
        {
            if (other.transform.parent == dayVuaBuong && thoiGianHoiDay > 0f)
            {
                return;
            }
            BatDauBamDay(other.transform.parent);
        }
    }

    void BatDauBamDay(Transform trucDay)
    {
        dangBamDay = true;
        trucDayHienTai = trucDay;
        dangDichChuyenNac = false;

        if (scriptDiChuyenGoc != null) scriptDiChuyenGoc.enabled = false;

        rbNhanVat.linearVelocity = Vector3.zero;
        rbNhanVat.isKinematic = true;

        transform.SetParent(trucDayHienTai);

        // --- TUYỆT CHIÊU NAM CHÂM ---
        // Ép trục X và Z về tâm sợi dây (+ độ lệch), giữ nguyên trục Y (độ cao)
        transform.localPosition = new Vector3(lechViTriBam.x, transform.localPosition.y, lechViTriBam.z);
        // Ép thẳng lưng nhân vật, tránh bị nghiêng vẹo theo collider
        transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

        animatorNhanVat.Play("ClimbingRope");
        animatorNhanVat.SetFloat("TocDoLeo", 0);

        gocHienTai = trucDayHienTai.localEulerAngles.z;
        if (gocHienTai > 180) gocHienTai -= 360;
        vanTocVang = vanTocVang * 0.5f;
    }

    void BuongDayNhay()
    {
        dangBamDay = false;
        dayVuaBuong = trucDayHienTai;
        thoiGianHoiDay = 1.2f;

        StopAllCoroutines();
        transform.SetParent(null);

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        animatorNhanVat.Play("Jump");
        animatorNhanVat.SetFloat("TocDoLeo", 1);

        rbNhanVat.isKinematic = false;

        StartCoroutine(ThietQuanLuatVatLy());
    }

    IEnumerator ThietQuanLuatVatLy()
    {
        float tg = 0;
        float huongMatX = transform.forward.x > 0 ? 1f : -1f;
        Vector3 lucEpCung = new Vector3(huongMatX * Mathf.Abs(lucNhayBuongDay.x), lucNhayBuongDay.y, 0);

        while (tg < 0.2f)
        {
            tg += Time.fixedDeltaTime;
            rbNhanVat.linearVelocity = lucEpCung;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.3f);
        if (scriptDiChuyenGoc != null) scriptDiChuyenGoc.enabled = true;
    }
}