using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Physics Settings")]
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;
        Vector3 velocity = moveDirection * moveSpeed;

        // Preserve vertical velocity (falling/jumping)
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}