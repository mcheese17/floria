using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FloriaBanSung : MonoBehaviour
{
    public Animator anim;
    public GameObject prefabVienDan;
    public Transform diemRaDan;

    [Header("Căn chỉnh Cảm giác Bắn")]
    public float doTreBan = 0.3f;
    public float thoiGianHoiChieu = 0.5f;

    // --- THÊM Ổ CẮM ÂM THANH ---
    [Header("Âm thanh Bắn")]
    public AudioSource loaCuaFloria;
    public AudioClip tiengBanSung;

    private bool dangBan = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame && !dangBan)
        {
            StartCoroutine(XuLyBanSung());
        }
    }

    IEnumerator XuLyBanSung()
    {
        dangBan = true;

        anim.Play("Shooting", -1, 0f);

        yield return new WaitForSeconds(doTreBan);

        // --- GỌI ÂM THANH BẮN SÚNG Ở ĐÂY ---
        if (loaCuaFloria != null && tiengBanSung != null)
        {
            loaCuaFloria.PlayOneShot(tiengBanSung);
        }

        GameObject dan = Instantiate(prefabVienDan, diemRaDan.position, diemRaDan.rotation);
        VienDan scriptDan = dan.GetComponent<VienDan>();

        if (transform.forward.x < -0.01f)
        {
            scriptDan.huongBay = Vector3.left;
            dan.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (transform.forward.x > 0.01f)
        {
            scriptDan.huongBay = Vector3.right;
            dan.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            scriptDan.huongBay = Vector3.right;
            dan.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        yield return new WaitForSeconds(thoiGianHoiChieu);
        dangBan = false;
    }
}