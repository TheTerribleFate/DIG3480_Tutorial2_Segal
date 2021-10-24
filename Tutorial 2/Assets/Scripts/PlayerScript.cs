using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text lives;
    private int scoreValue = 0;
    private int livesValue = 3;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public GameObject backgroundAudio;
    public AudioClip musicClipTwo;
    Animator anim; 
    private bool isOnGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask allGround;
    public float jumpForce;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        float hozMovement = Input.GetAxis("Horizontal");
        if(hozMovement != 0)
        {
            anim.SetInteger("State", 1);
        }
        else{
            anim.SetInteger("State", 0);
        }

        if (isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }

        if(isOnGround == true && anim.GetInteger("State") == 2 )
        {
            anim.SetInteger("State", 0);
        }

 

    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (hozMovement > 0 && facingRight == true)
        {
            Debug.Log ("Facing Right");
        }

        if (hozMovement < 0 && facingRight == false)
        {
            Debug.Log ("Facing Left");
        }

        if (verMovement > 0 && isOnGround == false)
        {
            Debug.Log ("Jumping");
        }

        else if (verMovement == 0 && isOnGround == true)
        {
            Debug.Log ("Not Jumping");
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if(scoreValue == 4)
            {
                transform.position = new Vector2(56.14f, -2.01f);
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
            }
            if(scoreValue >= 8)
            {
                winTextObject.SetActive(true);
                musicSource.clip = musicClipOne;
                musicSource.Play();
                Destroy(backgroundAudio);
                Destroy(this);
            }
        }
        
        else if(collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
            if(livesValue == 0)
            {
                loseTextObject.SetActive(true);
                musicSource.clip = musicClipTwo;
                musicSource.Play();
                Destroy(backgroundAudio);
                Destroy(this);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) 
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

}
