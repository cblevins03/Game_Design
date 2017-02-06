using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private Rigidbody2D playerRigidBody;
    private Animator playerAnimator;

    public float movementSpeed;
    public float jumpPower;
    public float maxSpeed;
    public bool isGrounded;

    //private bool facingRight;

	// Use this for initialization
	void Start ()
    {
        //facingRight = true;
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
	}

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        playerAnimator.SetBool("isGrounded", isGrounded);

        playerAnimator.SetFloat("speed", Mathf.Abs(playerRigidBody.velocity.x));

        if(Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Creates a fake friction
        Vector3 easeVelocity = playerRigidBody.velocity;
        easeVelocity.y = playerRigidBody.velocity.y;
        easeVelocity.z = 0.0f;
        easeVelocity.x *= 0.75f;

        float horizontal = Input.GetAxis("Horizontal");

        //Creates a fake friction.  A 2D Physics object was added to all platforms with friction=0.
        //This stops the player from running into the walls and sticking.  Also stops them from sliding.
        if(isGrounded)
        {
            playerRigidBody.velocity = easeVelocity;
        }

        playerRigidBody.velocity = new Vector2(horizontal * movementSpeed, playerRigidBody.velocity.y);

        if (playerRigidBody.velocity.x > maxSpeed)
        {
            playerRigidBody.velocity = new Vector2(maxSpeed, playerRigidBody.velocity.y);
        }

        if (playerRigidBody.velocity.x < -maxSpeed)
        {
            playerRigidBody.velocity = new Vector2(-maxSpeed, playerRigidBody.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRigidBody.AddForce(Vector2.up * jumpPower);
        }

        //Move(horizontal);

        //Flip(horizontal);
	}

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    /*private void Move(float horizontal)
    {
        playerRigidBody.velocity = new Vector2(horizontal * movementSpeed, playerRigidBody.velocity.y);

        playerAnimator.SetFloat("speed", Mathf.Abs(horizontal));

        playerAnimator.SetBool("isGrounded", isGrounded);
    }

    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 playerScale = transform.localScale;

            playerScale.x *= -1;

            transform.localScale = playerScale;
        }
    }*/
}
