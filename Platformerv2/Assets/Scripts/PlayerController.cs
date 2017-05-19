using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jump;

    public Transform topLeft;
    public Transform bottomRight;

    public GameObject boomerangPrefab;

    bool isDead;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    public LayerMask ground;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        isDead = false;
    }

    void FixedUpdate()
    {
        if (isDead)
            return;

        float h = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(h));

        if(h!=0)
        {
            if (h > 0)
                sr.flipX = false;
            else
                sr.flipX = true;
        }

        rb.velocity = new Vector2(h * speed, rb.velocity.y);

        bool isGrounded = Physics2D.OverlapArea(topLeft.position, bottomRight.position, ground);

        anim.SetBool("Jumping", !isGrounded);

        if(isGrounded && Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }
        // boomerang

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Object a =Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
            GameObject b = (GameObject)a;
            b.GetComponent<Rigidbody2D>().velocity = Vector2.right * 10;

        }


    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            BoxCollider2D enemyCollider = other.gameObject.GetComponent<BoxCollider2D>();

            float enemyTop = other.gameObject.transform.position.y + (enemyCollider.size.y / 2) + enemyCollider.offset.y;

            if (transform.position.y > enemyTop)
            {
                // kill enemy
                other.gameObject.GetComponent<EnemyController>().Die();

                rb.velocity = new Vector2(rb.velocity.x, jump);

            }
            else
            {
                // kill player
                StartCoroutine(playerDeath());

            }
        }
        else if (other.gameObject.CompareTag("LevelEnd"))
        {
            SceneManager.LoadScene(Application.loadedLevel + 1);
        }

    }

    IEnumerator playerDeath()
    {
        isDead = true;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.1f);

        rb.velocity = new Vector2(0, 15);
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(restartLevel());
    }

    IEnumerator restartLevel()
    {
        yield return new WaitForSeconds(1.9f);
        Application.LoadLevel(Application.loadedLevel);
    }

}
