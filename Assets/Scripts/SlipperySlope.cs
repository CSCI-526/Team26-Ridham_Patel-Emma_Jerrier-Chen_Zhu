using UnityEngine;

public class SlipperySlope : MonoBehaviour
{
    public float slideForce = 200f;
    public float maxSlideSpeed = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PushPlayer(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PushPlayer(collision);
    }

    private void PushPlayer(Collision2D collision)
    {
        Rigidbody2D player = collision.collider.attachedRigidbody;

        if (player == null || !player.CompareTag("Player"))
            player = collision.otherCollider.attachedRigidbody;

        if (player == null || !player.CompareTag("Player"))
            return;

        // Do not pull the player sideways during a jump.
        if (player.linearVelocity.y > 1f) return;

        float tilt = transform.right.y;
        if (Mathf.Abs(tilt) < 0.05f) return;

        float downhill = -Mathf.Sign(tilt);

        // Push left, even when the player is balanced on the tip.
        if (player.linearVelocity.x > -maxSlideSpeed)
        {
            player.AddForce(
                Vector2.left * slideForce * player.mass,
                ForceMode2D.Force
            );
        }
    }
}