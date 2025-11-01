using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2000f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    [Header("Camera Settings")]
    public Transform cameraTransform;

    [Header("Animation Settings")]
    public string isMovingParameter = "isMoving";

    private CharacterController cc;
    private Animator animator;
    private Vector3 velocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cc.minMoveDistance = 0f;
    }

    void Update()
    {
        // 1️⃣ Detect input directly
        bool inputPressed = Input.GetKey(KeyCode.W) ||
                            Input.GetKey(KeyCode.A) ||
                            Input.GetKey(KeyCode.S) ||
                            Input.GetKey(KeyCode.D);

        // 2️⃣ Get raw input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        // 3️⃣ Camera-relative movement
        Vector3 moveDir = inputDir;
        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = cameraTransform.right;
            right.y = 0;
            right.Normalize();

            moveDir = (forward * v + right * h).normalized;
        }

        // 4️⃣ Rotate player
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 5️⃣ Move
        Vector3 move = moveDir * moveSpeed;

        // 6️⃣ Gravity
        if (!cc.isGrounded)
            velocity.y += gravity * Time.deltaTime;
        else
            velocity.y = -1f;

        move += new Vector3(0, velocity.y, 0);

        // 7️⃣ Apply movement
        cc.Move(move * Time.deltaTime);

        // 8️⃣ Update Animator based on input directly
        animator.SetBool(isMovingParameter, inputPressed);
    }
}
