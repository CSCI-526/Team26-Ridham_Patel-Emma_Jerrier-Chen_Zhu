using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpSpeed = 11f;

    private Rigidbody2D body;
    private BoxCollider2D playerBox;
    private float horizontal;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerBox = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && GetPlatform() != null)
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
    }

    private void FixedUpdate()
    {
        body.linearVelocity = new Vector2(horizontal * moveSpeed, body.linearVelocity.y);
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