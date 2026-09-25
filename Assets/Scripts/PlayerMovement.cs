//using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float acceleration = 60f;
    public float deceleration = 70f;

    [Header("Jump")]
    public float jumpSpeed = 11f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D body;
    private BoxCollider2D playerBox;
    private float horizontal;

    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool isGrounded;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerBox = GetComponent<BoxCollider2D>();
    }
    
    private void Start()
    {
    }
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        isGrounded = GetPlatform() != null;
        coyoteTimer = isGrounded ? coyoteTime : coyoteTimer - Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        if(jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        if(body.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        else if (body.linearVelocity.y < 0)
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

       // if (Input.GetKeyDown(KeyCode.Space) && GetPlatform() != null)
       //     body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
    }

    private void FixedUpdate()
    {
        float targetSpeed = horizontal * moveSpeed;
        float speedDiff = targetSpeed - body.linearVelocity.x;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        float movement = speedDiff * accelRate * Time.fixedDeltaTime;

        body.linearVelocity = new Vector2(body.linearVelocity.x + movement, body.linearVelocity.y);
    }

    public ColorBlock GetPlatform()
    {
        Bounds feet = playerBox.bounds;
        Vector2 point = new Vector2(feet.center.x, feet.min.y - 0.03f);
        Vector2 size = new Vector2(feet.size.x * 0.85f, 0.12f);

        Collider2D[] hits = Physics2D.OverlapBoxAll(point, size, 0f);

        foreach (Collider2D hit in hits)
        {
            ColorBlock block = hit.GetComponent<ColorBlock>();

            if (block != null && block.isPlatform)
                return block;
        }

        return null;
    }
}