using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class HeThongTroChuyen : MonoBehaviour
{
    [Header("UI & NPC")]
    public GameObject khungThoai;
    public TextMeshProUGUI textNoiDung;
    public Animator animNPC;
    public Animator animFloria;

    [Header("Dữ liệu câu thoại")]
    [TextArea] public string[] danhSachCauThoai;
    private int chiSoCau = -1;
    private bool dangTroChuyen = false;

    void Update()
    {
        // Nhấn E để bắt đầu, D hoặc Mũi tên phải để chuyển câu
        if (Input.GetKeyDown(KeyCode.E) && !dangTroChuyen) KichHoatHoiThoai();
        else if (dangTroChuyen && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))) ChuyenCauThoai();
    }

    void KichHoatHoiThoai()
    {
        dangTroChuyen = true;
        chiSoCau = 0;
        khungThoai.SetActive(true);
        animFloria.Play("Talking_Floria"); // Anim Floria nói
        HienThiCauHienTai();
    }

    void ChuyenCauThoai()
    {
        chiSoCau++;
        if (chiSoCau < danhSachCauThoai.Length)
        {
            HienThiCauHienTai();
        }
        else
        {
            KetThucHoiThoai();
        }
    }

    void HienThiCauHienTai()
    {
        textNoiDung.text = danhSachCauThoai[chiSoCau];
        // Logic đổi Anim: Câu chẵn NPC ngồi buồn, câu lẻ NPC ngồi nói
        if (chiSoCau % 2 == 0) animNPC.Play("Sitting");
        else animNPC.Play("SittingTalking");
    }

    void KetThucHoiThoai()
    {
        dangTroChuyen = false;
        khungThoai.SetActive(false);
        animNPC.Play("Sitting1"); // Trở về trạng thái tĩnh ban đầu
    }
}