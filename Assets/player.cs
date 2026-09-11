using UnityEngine;

public class player : MonoBehaviour
{
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode shootKey = KeyCode.Space;

    public float speed = 8f;
    public float fireCooldown = 0.4f;

    [Header("Tiro")]
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;

    private Rigidbody2D rb2d;
    private float limiteEsquerdo;
    private float limiteDireito;
    private float lastShotTime = -999f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        GameObject leftWall = GameObject.Find("LeftWall");
        GameObject rightWall = GameObject.Find("RightWall");

        if (leftWall != null && rightWall != null)
        {
            float wallLeftPos = leftWall.transform.position.x;
            float wallRightPos = rightWall.transform.position.x;
            float wallLeftWidth = leftWall.transform.localScale.x;
            float wallRightWidth = rightWall.transform.localScale.x;

            float wallLeftEdge = wallLeftPos + (wallLeftWidth / 2);
            float wallRightEdge = wallRightPos - (wallRightWidth / 2);

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            float halfWidth = sr != null ? sr.bounds.extents.x : 0.52f;

            limiteEsquerdo = wallLeftEdge + halfWidth;
            limiteDireito = wallRightEdge - halfWidth;
        }
        else
        {
            limiteEsquerdo = -4.83f;
            limiteDireito = 4.83f;
        }

        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb2d.gravityScale = 0f;
    }

    void Update()
    {
        Vector2 vel = rb2d.linearVelocity;

        if (Input.GetKey(moveLeft) || Input.GetKey(KeyCode.LeftArrow))
        {
            vel.x = -speed;
        }
        else if (Input.GetKey(moveRight) || Input.GetKey(KeyCode.RightArrow))
        {
            vel.x = speed;
        }
        else
        {
            vel.x = 0;
        }

        rb2d.linearVelocity = vel;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteEsquerdo, limiteDireito);
        transform.position = pos;

        // Tiro automático: dispara sozinho, respeitando o fireCooldown
        TryShoot();
    }

    void TryShoot()
    {
        // Não atira antes do jogador clicar em Iniciar
        if (InvaderFormation.Instance == null || !InvaderFormation.Instance.IsActive) return;

        if (Time.time - lastShotTime < fireCooldown) return;
        if (missilePrefab == null) return;

        lastShotTime = Time.time;

        Vector3 spawnPos = missileSpawnPoint != null ? missileSpawnPoint.position : transform.position;
        Instantiate(missilePrefab, spawnPos, Quaternion.identity);
    }
}