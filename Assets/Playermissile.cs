using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;
        rb2d.linearVelocity = Vector2.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ===== INVASOR COMUM =====
        if (other.CompareTag("Invader"))
        {
            Invader invaderScript = other.GetComponent<Invader>();
            if (invaderScript != null && ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(invaderScript.points);
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        // ===== NAVE CHEFE =====
        else if (other.CompareTag("Boss"))
        {
            Boss bossScript = other.GetComponent<Boss>();
            if (bossScript != null && ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(bossScript.points);
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        // ===== PAREDE SUPERIOR =====
        else if (other.CompareTag("TopWall"))
        {
            Destroy(gameObject);
        }

        // Observação: mísseis do inimigo (tag não verificada aqui) passam direto,
        // sem interagir com o míssil do player.
    }
}