using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CamBienBamMep : MonoBehaviour
{
    [Header("Liên kết Component")]
    public Animator animatorNhanVat;
    public Rigidbody rbNhanVat;
    public Collider capsuleNhanVat;

    // --- ĐÂY LÀ CHỖ ĐÃ SỬA ---
    // Gọi thẳng tên Script PlayerController thay vì Behaviour chung chung
    [Tooltip("Kéo cái Object Floria vào đây, nó sẽ tự tìm PlayerController")]
    public PlayerController scriptDiChuyenGoc;

    [Header("Cài đặt Trèo & Ép sát")]
    public float khoangCachHutSat = 0.2f;
    public float thoiGianTreo = 3.233f;
    public Vector3 khoangCachDichChuyen = new Vector3(0.1f, 1.2f, 0f);

    private bool dangBamMep = false;
    private bool dangTreoLen = false;

    void Update()
    {
        if (dangBamMep && !dangTreoLen)
        {
            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.wasPressedThisFrame ||
                    Keyboard.current.upArrowKey.wasPressedThisFrame)
                {
                    BatDauTreo();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ledge") && !dangBamMep && !dangTreoLen)
        {
            // Tắt đúng cái PlayerController, không đụng tới Animator nữa
            if (scriptDiChuyenGoc != null)
            {
                scriptDiChuyenGoc.enabled = false;
            }

            rbNhanVat.linearVelocity = Vector3.zero;
            rbNhanVat.isKinematic = true;
            if (capsuleNhanVat != null) capsuleNhanVat.enabled = false;

            rbNhanVat.transform.position += rbNhanVat.transform.forward * khoangCachHutSat;

            dangBamMep = true;
            animatorNhanVat.SetTrigger("Trigger_Bam");
        }
    }

    void BatDauTreo()
    {
        dangTreoLen = true;
        animatorNhanVat.SetTrigger("Trigger_Treo");
        StartCoroutine(DoiTreoXong());
    }

    IEnumerator DoiTreoXong()
    {
        yield return new WaitForSeconds(thoiGianTreo);

        Vector3 huongDiChuyen = rbNhanVat.transform.forward * khoangCachDichChuyen.x;
        Vector3 huongNhacLen = Vector3.up * khoangCachDichChuyen.y;
        rbNhanVat.transform.position += huongDiChuyen + huongNhacLen;

        rbNhanVat.isKinematic = false;
        if (capsuleNhanVat != null) capsuleNhanVat.enabled = true;

        // Bật lại PlayerController sau khi trèo xong
        if (scriptDiChuyenGoc != null)
        {
            scriptDiChuyenGoc.enabled = true;
        }

        dangBamMep = false;
        dangTreoLen = false;
    }
}