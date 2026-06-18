using UnityEngine;
using System.Collections;

public class BossPhunDoc : MonoBehaviour
{
    [Header("Cài đặt Phun Độc")]
    public GameObject prefabKichDoc;
    public Transform miengThuyTo;

    [Tooltip("Cứ bao nhiêu giây thì gốc cây phun độc 1 lần?")]
    public float thoiGianNghi = 4.5f;

    private bool dangChienDau = true;

    void Start()
    {
        // Bắt đầu vòng lặp nhổ độc
        StartCoroutine(KichBanPhunDoc());
    }

    IEnumerator KichBanPhunDoc()
    {
        while (dangChienDau)
        {
            // Nghỉ lấy hơi
            yield return new WaitForSeconds(thoiGianNghi);

            // Phun độc ra!
            if (prefabKichDoc != null && miengThuyTo != null)
            {
                Instantiate(prefabKichDoc, miengThuyTo.position, miengThuyTo.rotation);
            }
        }
    }
}