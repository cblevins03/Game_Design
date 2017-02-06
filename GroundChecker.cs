using UnityEngine;
using System.Collections;

public class GroundChecker : MonoBehaviour {

    private Player player;
    private float restartLevelDelay = 1f;

	void Start ()
    {
        player = gameObject.GetComponentInParent<Player>();
	}

    void OnTriggerLeave2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        player.isGrounded = true;
	}

    void OnTriggerStay2D(Collider2D col)
    {
        player.isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        player.isGrounded = false;
    }
}
