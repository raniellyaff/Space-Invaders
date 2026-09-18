using UnityEngine;

public class Boss : MonoBehaviour
{
    public int points = 50;
    public float speed = 3f;
    public int health = 2;

    public static bool IsBossAlive { get; private set; } = false;

    [Header("Tiro do Boss")]
    public GameObject bossMissileTemplate;
    public float minShootInterval = 1f;
    public float maxShootInterval = 2.5f;

    private float shootTimer = 0f;
    private float nextShootInterval;
    private Rigidbody2D rb2d;
    private int direction = 1; // 1 = direita, -1 = esquerda

    void Awake()
    {
        IsBossAlive = true;
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
            rb2d = gameObject.AddComponent<Rigidbody2D>();

        rb2d.bodyType = RigidbodyType2D.Kinematic;
        rb2d.gravityScale = 0f;

        nextShootInterval = Random.Range(minShootInterval, maxShootInterval);
    }

    void Update()
    {
        // Movimento horizontal
        transform.position += Vector3.right * speed * direction * Time.deltaTime;

        // ===== Limites laterais (paredes) =====
        GameObject leftWall = GameObject.FindGameObjectWithTag("LeftWall");
        GameObject rightWall = GameObject.FindGameObjectWithTag("RightWall");

        if (leftWall != null && rightWall != null)
        {
            float leftEdge = leftWall.transform.position.x + (leftWall.transform.localScale.x / 2);
            float rightEdge = rightWall.transform.position.x - (rightWall.transform.localScale.x / 2);

            Vector3 pos = transform.position;

            if (pos.x <= leftEdge)
            {
                pos.x = leftEdge;
                direction = 1;
            }
            else if (pos.x >= rightEdge)
            {
                pos.x = rightEdge;
                direction = -1;
            }

            transform.position = pos;
        }

        // ===== Tiro =====
        shootTimer += Time.deltaTime;
        if (shootTimer >= nextShootInterval)
        {
            shootTimer = 0f;
            nextShootInterval = Random.Range(minShootInterval, maxShootInterval);

            if (bossMissileTemplate == null) return;

            GameObject missile = Instantiate(bossMissileTemplate, transform.position, Quaternion.identity);
            missile.SetActive(true);

            InvaderMissile im = missile.GetComponent<InvaderMissile>();
            if (im != null) im.isBossMissile = true;
        }
    }

    public void TakeHit()
    {
        health--;
        Debug.Log("Boss levou tiro! Health restante = " + health);
        if (health <= 0)
            Destroy(gameObject);
    }

    void OnDestroy()
    {
        IsBossAlive = false;
    }
}