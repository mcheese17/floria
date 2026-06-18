using UnityEngine;
using System.Collections;

public class SauBoMatDat : MonoBehaviour
{
    [Header("Cài đặt Di chuyển")]
    public float tocDo = 3f;

    [Header("Cài đặt Bám Đất")]
    public LayerMask layerMatDat;
    public float doCaoBanTia = 2f;
    public float chieuDaiTia = 5f;

    [Header("Hiệu Ứng VFX")]
    public GameObject hieuUngBiBanChet;
    public GameObject hieuUngTauThoat;

    // --- THÊM Ổ CẮM ÂM THANH CHẾT ---
    [Header("Âm Thanh SFX")]
    public AudioClip tiengSauChet;

    private bool dangBoTron = false;

    void Start()
    {
        tocDo += Random.Range(-tocDo * 0.15f, tocDo * 0.15f);
    }

    void Update()
    {
        if (dangBoTron) return;

        transform.Translate(Vector3.right * tocDo * Time.deltaTime, Space.World);

        RaycastHit hit;
        Vector3 diemBatDau = transform.position + Vector3.up * doCaoBanTia;
        if (Physics.Raycast(diemBatDau, Vector3.down, out hit, chieuDaiTia, layerMatDat))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (dangBoTron) return;

        if (other.CompareTag("VienDan"))
        {
            FindAnyObjectByType<GameManager>().GhiNhanSauChet();

            if (hieuUngBiBanChet != null)
            {
                GameObject fx = Instantiate(hieuUngBiBanChet, transform.position, Quaternion.identity);
                Destroy(fx, 2f);
            }

            // --- GỌI ÂM THANH SÂU RÚ LÊN Ở ĐÂY ---
            if (tiengSauChet != null)
            {
                AudioSource.PlayClipAtPoint(tiengSauChet, transform.position);
            }

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<HeThongHoiSinh>().FloriaBiGiet();
        }
        else if (other.CompareTag("LoiThoat"))
        {
            StartCoroutine(HieuUngBienMat());
        }
    }

    IEnumerator HieuUngBienMat()
    {
        dangBoTron = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (hieuUngTauThoat != null)
        {
            Vector3 viTriTruocMat = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1.5f);
            GameObject fx = Instantiate(hieuUngTauThoat, viTriTruocMat, Quaternion.identity);
            Destroy(fx, 2f);
        }

        Renderer r = GetComponentInChildren<Renderer>();
        if (r != null)
        {
            r.material.color = Color.magenta;
        }

        FindAnyObjectByType<GameManager>().GhiNhanSauTauThoat();

        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
    }
}