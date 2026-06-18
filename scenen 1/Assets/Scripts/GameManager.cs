using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Nhiệm vụ Boss")]
    public int soSauCanTieuDiet = 25;
    public int tongSoSauDuocPhepSpawn = 25;

    // BIẾN MỚI: Sổ ghi chép số lượng quái đã được máy đẻ ra sân
    public int tongSoSauDaSanXuat = 0;

    private int soSauDaGiet = 0;

    [Header("Hệ thống Trừng Phạt Ánh Sáng")]
    public Light denToanCanh;
    public Light denCayThuyTo;
    public GameObject cayThuyToGoc;

    public int gioiHanSongChuong = 5;
    private int soConDaSong = 0;

    private float dirLightMax = 0.8f;
    private float dirLightMin = 0.2f;
    private float pointLightMin = 100f;
    private float pointLightMax = 300f;

    private float mucTieuDirLight;
    private float mucTieuPointLight;

    private Vector3 scaleGocCuaCay;
    private Coroutine hieuUngHienTai;

    [Header("Cutscene Cuối Game")]
    public GameObject ongCutscene; // Kéo con ong tàng hình vào đây

    void Start()
    {
        mucTieuDirLight = dirLightMax;
        mucTieuPointLight = pointLightMin;

        if (denToanCanh != null) denToanCanh.intensity = mucTieuDirLight;
        if (denCayThuyTo != null) denCayThuyTo.intensity = mucTieuPointLight;

        if (cayThuyToGoc != null) scaleGocCuaCay = cayThuyToGoc.transform.localScale;
    }

    public void GhiNhanSauChet()
    {
        soSauDaGiet++;
        Debug.Log("🎯 Đã diệt: " + soSauDaGiet + " / " + soSauCanTieuDiet);

        if (soSauDaGiet >= soSauCanTieuDiet)
        {
            StartCoroutine(KichHoatChienThang());
        }
    }

    public void GhiNhanSauTauThoat()
    {
        soConDaSong++;

        if (soConDaSong >= gioiHanSongChuong)
        {
            Debug.Log("💀 Quá giới hạn phá hoại! Môi trường sụp đổ!");
            FindAnyObjectByType<HeThongHoiSinh>().FloriaBiGiet();
            return;
        }

        soSauCanTieuDiet++;
        tongSoSauDuocPhepSpawn += 2; // Phạt sổng 1 con thì máy được phép đẻ thêm 2 con

        float tyLePhat = (float)soConDaSong / gioiHanSongChuong;
        mucTieuDirLight = Mathf.Lerp(dirLightMax, dirLightMin, tyLePhat);
        mucTieuPointLight = Mathf.Lerp(pointLightMin, pointLightMax, tyLePhat);

        if (hieuUngHienTai != null) StopCoroutine(hieuUngHienTai);
        hieuUngHienTai = StartCoroutine(HieuUngBossAnSau());
    }

    IEnumerator HieuUngBossAnSau()
    {
        if (cayThuyToGoc != null && denCayThuyTo != null)
        {
            cayThuyToGoc.transform.localScale = scaleGocCuaCay * 1.15f;
            denCayThuyTo.intensity = pointLightMax + 200f;

            yield return new WaitForSeconds(0.15f);

            cayThuyToGoc.transform.localScale = scaleGocCuaCay;

            float startDir = denToanCanh.intensity;
            float startPoint = denCayThuyTo.intensity;
            float t = 0;

            while (t < 1f)
            {
                t += Time.deltaTime * 2f;
                if (denToanCanh != null) denToanCanh.intensity = Mathf.Lerp(startDir, mucTieuDirLight, t);
                if (denCayThuyTo != null) denCayThuyTo.intensity = Mathf.Lerp(startPoint, mucTieuPointLight, t);
                yield return null;
            }
        }
    }

    IEnumerator KichHoatChienThang()
    {
        Debug.Log("🏆 CHIẾN THẮNG! Khôi phục ánh sáng...");

        BossSpawner mayDeQuai = FindAnyObjectByType<BossSpawner>();
        if (mayDeQuai != null)
        {
            mayDeQuai.StopAllCoroutines();
        }

        // --- PHẦN THÊM MỚI 1: Tắt cây phun độc ---
        BossPhunDoc cayDoc = FindAnyObjectByType<BossPhunDoc>();
        if (cayDoc != null)
        {
            cayDoc.StopAllCoroutines();
        }
        // ----------------------------------------

        float startDir = denToanCanh != null ? denToanCanh.intensity : dirLightMax;
        float startPoint = denCayThuyTo != null ? denCayThuyTo.intensity : pointLightMin;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * 0.5f;
            if (denToanCanh != null) denToanCanh.intensity = Mathf.Lerp(startDir, dirLightMax, t);
            if (denCayThuyTo != null) denCayThuyTo.intensity = Mathf.Lerp(startPoint, pointLightMin, t);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Trọng tài chỉ gọi con Ong ra sân rồi nghỉ việc
        if (ongCutscene != null)
        {
            ongCutscene.SetActive(true);
        }
    }
}