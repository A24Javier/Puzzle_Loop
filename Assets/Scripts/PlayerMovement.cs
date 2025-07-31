using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset inputActionMapping;
    private InputAction moveAction, jumpAction;
    private Rigidbody2D rb;

    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Detecci�n suelo")]
    [SerializeField] private Transform raycastTransf;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;

    [Header("Empuje")]
    [SerializeField] private float extraPushForce = 5f; 

    private Vector2 moveInput;
    private bool wantsToJump;

    void Awake()
    {
        inputActionMapping.Enable();
        rb = GetComponent<Rigidbody2D>();
        var map = inputActionMapping.FindActionMap("Player");
        moveAction = map.FindAction("Move");
        jumpAction = map.FindAction("Jump");
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (jumpAction.triggered)
        {
            wantsToJump = true;
        }
    }

    void FixedUpdate()
    {
        Vector2 vel = rb.linearVelocity;
        vel.x = moveInput.x * speed;
        rb.linearVelocity = vel;

        // Salto
        if (wantsToJump && IsGrounded())
        {
            Vector2 v = rb.linearVelocity;
            v.y = jumpForce;
            rb.linearVelocity = v;
        }

        wantsToJump = false;
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastTransf.position, Vector2.down, groundCheckDistance, groundMask);
        Debug.DrawRay(raycastTransf.position, Vector2.down * groundCheckDistance, Color.red);
        return hit.collider != null;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D otherRb = collision.rigidbody;
        if (otherRb != null && otherRb.bodyType == RigidbodyType2D.Dynamic && collision.gameObject.CompareTag("Pushable"))
        {
            Vector2 pushDir = (otherRb.position - rb.position).normalized;

            if (Mathf.Abs(moveInput.x) > 0.01f || Mathf.Abs(moveInput.y) > 0.01f)
            {
                Vector2 applied = new Vector2(pushDir.x, pushDir.y) * extraPushForce;
                otherRb.AddForce(applied, ForceMode2D.Force);
            }
        }
    }
}
