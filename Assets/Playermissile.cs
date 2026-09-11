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

    void OnCollisionEnter2D(Collision2D coll)
    {
        // ===== INVASOR COMUM =====
        if (coll.collider.CompareTag("Invader"))
        {
            Invader invaderScript = coll.gameObject.GetComponent<Invader>();
            if (invaderScript != null && ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(invaderScript.points);
            }

            Destroy(coll.gameObject);
            Destroy(gameObject);
        }

        // ===== NAVE CHEFE =====
        if (coll.collider.CompareTag("Boss"))
        {
            Boss bossScript = coll.gameObject.GetComponent<Boss>();
            if (bossScript != null && ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(bossScript.points);
            }

            Destroy(coll.gameObject);
            Destroy(gameObject);
        }

        // ===== PAREDE SUPERIOR =====
        if (coll.collider.CompareTag("TopWall"))
        {
            Destroy(gameObject);
        }
    }
}