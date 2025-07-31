using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerActionControls playerActionControls;
    private Rigidbody2D rb;
    [SerializeField] private float speed = 1, jumpForce = 1, groundRaycastLength = 10f;
    [SerializeField] private Transform raycastTransf;

    void Awake()
    {
        playerActionControls = new PlayerActionControls();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 moveInput = playerActionControls.Player.Move.ReadValue<Vector2>();

        if(moveInput != Vector2.zero && !TimeManager.instance.isRecording)
        {
            TimeManager.instance.isRecording = true;
        }

        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput.x * speed;
        rb.linearVelocity = velocity;

        bool isGrounded = CheckIsGrounded();

        if (playerActionControls.Player.Jump.triggered && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }

    private bool CheckIsGrounded()
    {
        bool isGrounded = false;
        int layerMask = LayerMask.GetMask("Floor");
        RaycastHit2D hit = Physics2D.Raycast(raycastTransf.position, Vector2.down, groundRaycastLength, layerMask, 0);
        Debug.DrawRay(raycastTransf.position, Vector2.down.normalized * groundRaycastLength, Color.red);

        if(hit.collider != null)
        {
            isGrounded = hit.collider.CompareTag("Floor");
        }
        
        return isGrounded;
    }

    private void OnEnable()
    {
        playerActionControls.Enable();
    }

    private void OnDisable()
    {
        playerActionControls.Disable();
    }

}
