using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    public float walkSpeed = 3f;          // Tốc độ đi bộ
    public float runSpeed = 6f;           // Tốc độ chạy nhanh
    private float currentSpeed;
    private float moveInput;

    [Header("Hệ thống nhấn đúp để chạy (Dash/Run)")]
    public float doubleTapTimeThreshold = 0.3f; // Thời gian tối đa giữa 2 lần bấm (0.3 giây)
    private float lastTapTimeA;
    private float lastTapTimeD;
    private bool isRunning = false;

    [Header("Lực nhảy vật lý")]
    public float jumpUpForce = 6f;         // Lực nhảy tại chỗ (Jump1)
    public float jumpFarForceX = 5f;       // Lực đẩy ngang khi nhảy xa (Jump)
    public float jumpFarForceY = 7f;       // Lực đẩy cao khi nhảy xa (Jump)

    [Header("Kiểm tra mặt đất (Ground Check)")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    private Rigidbody rb;
    private Animator anim;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Kiểm tra xem có đang đứng trên mặt hoa cúc không
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);

        // Đọc phím bấm và xử lý Nhấn Đúp để Chạy
        HandleInput();

        // Cập nhật giá trị Speed cho Animator để kích hoạt chuyển đổi giữa Idle -> Walk -> Run
        if (moveInput == 0)
        {
            anim.SetFloat("Speed", 0f); // Về Idle
        }
        else
        {
            // Nếu đang chạy truyền hẳn 2f (>1.5), nếu đi bộ truyền 1f (>0.1)
            anim.SetFloat("Speed", isRunning ? 2f : 1f);
        }

        // XỬ LÝ NHẢY
        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (moveInput != 0)
            {
                // Nhảy xa qua hoa khác: Đẩy cả lực ngang (X) và lực cao (Y)
                float jumpDir = facingRight ? 1f : -1f;
                rb.linearVelocity = new Vector3(jumpDir * jumpFarForceX, jumpFarForceY, 0f);
                anim.SetTrigger("JumpFarTrigger");
            }
            else
            {
                // Nhảy tại chỗ (Jump1): Giữ nguyên X, chỉ đẩy thẳng đứng trục Y
                rb.linearVelocity = new Vector3(0f, jumpUpForce, 0f);
                anim.SetTrigger("JumpUpTrigger");
            }
        }
    }

    void FixedUpdate()
    {
        // Áp dụng vận tốc di chuyển ngang trục X dựa vào hướng Floria đang quay mặt mặt thực tế
        if (isGrounded && moveInput != 0)
        {
            float direction = facingRight ? -1f : 1f;
            rb.linearVelocity = new Vector3(direction * currentSpeed, rb.linearVelocity.y, 0f);
        }
        else if (isGrounded && moveInput == 0)
        {
            // Khi buông tay, triệt tiêu ngay vận tốc ngang để Floria dừng khựng lại mượt mà, không bị trượt lê
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    private void HandleInput()
    {
        moveInput = 0f;

        // NHẤN PHÍM D (ĐI SANG PHẢI)
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f;
            if (!facingRight) Flip(); // Nếu đang quay trái thì lật mặt sang phải
        }
        // NHẤN PHÍM A (ĐI SANG TRÁI)
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput = -1f;
            if (facingRight) Flip(); // Nếu đang quay phải thì lật mặt sang trái
        }

        // BẮT SỰ KIỆN NHẤN ĐÚP PHÍM ĐỂ CHẠY
        if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            float timeSinceLastTap = Time.time - lastTapTimeD;
            if (timeSinceLastTap <= doubleTapTimeThreshold)
            {
                isRunning = true;
                currentSpeed = runSpeed;
            }
            lastTapTimeD = Time.time;
        }

        if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            float timeSinceLastTap = Time.time - lastTapTimeA;
            if (timeSinceLastTap <= doubleTapTimeThreshold)
            {
                isRunning = true;
                currentSpeed = runSpeed;
            }
            lastTapTimeA = Time.time;
        }

        // Nếu buông hoàn toàn phím ra thì reset trạng thái chạy về đi bộ bình thường
        if (moveInput == 0)
        {
            isRunning = false;
            currentSpeed = walkSpeed;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        // Xoay quanh trục Y đúng 180 độ để lật hẳn mô hình nhân vật lại
        transform.Rotate(0f, 180f, 0f);
    }
}