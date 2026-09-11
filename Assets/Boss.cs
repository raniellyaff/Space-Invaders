using UnityEngine;

public class Boss : MonoBehaviour
{
    public int points = 50;
    public float speed = 3f;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = gameObject.AddComponent<Rigidbody2D>();
        }

        rb2d.bodyType = RigidbodyType2D.Kinematic;
        rb2d.gravityScale = 0f;
    }

    void Update()
    {
        // Anda 5 unidades de X por passo (aqui suavizado por Time.deltaTime)
        transform.position += Vector3.right * speed * Time.deltaTime;

        GameObject rightWall = GameObject.FindGameObjectWithTag("RightWall");
        if (rightWall != null && transform.position.x > rightWall.transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}