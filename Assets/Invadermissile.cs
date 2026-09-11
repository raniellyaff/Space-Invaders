using UnityEngine;

public class InvaderMissile : MonoBehaviour
{
    public float speed = 6f;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;
        rb2d.linearVelocity = Vector2.down * speed;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // ===== JOGADOR =====
        if (coll.collider.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.LoseLife();
            }

            Destroy(gameObject);
        }

        // ===== PAREDE INFERIOR =====
        if (coll.collider.CompareTag("BottomWall"))
        {
            Destroy(gameObject);
        }
    }
}