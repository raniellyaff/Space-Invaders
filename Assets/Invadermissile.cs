using UnityEngine;

public class InvaderMissile : MonoBehaviour
{
    public float speed = 6f;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;

        // Pega a posição X do player UMA ÚNICA VEZ, no instante do disparo.
        // Depois disso o míssil não persegue mais, só desce reto.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 pos = transform.position;
            pos.x = player.transform.position.x;
            transform.position = pos;
        }

        rb2d.linearVelocity = Vector2.down * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ===== JOGADOR =====
        if (other.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.LoseLife();
            }

            Destroy(gameObject);
        }

        // ===== PAREDE INFERIOR =====
        else if (other.CompareTag("BottomWall"))
        {
            Destroy(gameObject);
        }
    }
}