using UnityEngine;

public class InvaderMissile : MonoBehaviour
{
    public float speed = 6f;
    public bool isBossMissile = false;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;

        Vector2 direction = Vector2.down;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
        }

        rb2d.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
            {
                if (isBossMissile)
                    ScoreManager.Instance.KillPlayerInstant();
                else
                    ScoreManager.Instance.LoseLife();
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("BottomWall"))
        {
            Destroy(gameObject);
        }
    }
}