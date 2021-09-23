using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
    private float activeMoveSpeed;

    public bool canMove;

	public Rigidbody2D myRigidbody;
	public float jumpSpeed;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	public bool isGrounded;

	private Animator myAnim;
	public Vector3 respawnPosition;

	public LevelManager theLevelManager; //referencing with the level manager script

	public GameObject stompBox;

    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;
    public float invincibilityLength;
    private float invincibilityCounter;

    public AudioSource jumpSound;
    public AudioSource hurtSound;

    private bool onPlatform;
    public float onPlatformSpeedModifier;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> (); 
		myAnim = GetComponent<Animator> (); 
		respawnPosition = transform.position;
		theLevelManager = FindObjectOfType<LevelManager> ();

        activeMoveSpeed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
        /*creates a circle to check grounded or not through variables*/

        if (knockbackCounter <= 0 && canMove)
        {

            if(onPlatform)
            {
                activeMoveSpeed = moveSpeed * onPlatformSpeedModifier;

            }else
            {
                activeMoveSpeed = moveSpeed;
                canMove = true;

            }

            if (Input.GetAxisRaw("Horizontal") > 0f)
            {       //left right move functions
                myRigidbody.velocity = new Vector3(activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                myRigidbody.velocity = new Vector3(-activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {               //jump state

                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpSpeed, 0f);
                jumpSound.Play();
            }
            
        }

        if(knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;                         //knockbackCounter starts to count down here
            if(transform.localScale.x > 0) { 
                 myRigidbody.velocity = new Vector3(-knockbackForce, knockbackForce, 0f);
            }
            else
            {
                myRigidbody.velocity = new Vector3(knockbackForce, knockbackForce, 0f);
            }
        }

        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime; 

        }

        if(invincibilityCounter <= 0)
        {
            theLevelManager.invincible = false;

        }

		myAnim.SetFloat ("Speed", Mathf.Abs(myRigidbody.velocity.x)); //gets absolute value of speed for animation
		/*as walk animation will happen only speed is greater than 0.1, so we need absolute value*/
		myAnim.SetBool ("Grounded", isGrounded);

		if(myRigidbody.velocity.y < 0)
		{
			stompBox.SetActive (true);
		}
	}

    public void Knockback()                                      //knockback function if player is hurt
    {
        knockbackCounter = knockbackLength;
        invincibilityCounter = invincibilityLength;

        theLevelManager.invincible = true;                      //by default it is invincible
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "KillPlane") {
		
			//gameObject.SetActive (false);
			//transform.position = respawnPosition;
			theLevelManager.Respawn();  //Updated the  respawn function to level manager.
		}

		if (other.tag == "Checkpoint")
		{
			respawnPosition = other.transform.position; //respawn in checkpoint 
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "MovingPlatform") {

			transform.parent = other.transform;  // making the colliding other object's transform equal to Moving Object's transform

            onPlatform = true;
		}

	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.tag == "MovingPlatform") 
		{
			transform.parent = null;
            onPlatform = false;
		}

	}
}
