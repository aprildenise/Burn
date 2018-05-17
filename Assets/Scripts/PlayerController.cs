using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Defines the controls the player can take:
 * Movement (using the arrow keys/WASD keys)
 * Burn
 * Pause/unpause
 */

public class PlayerController : MonoBehaviour {

    private float moveSpeed;
    private Rigidbody2D player;
    bool isMoving = false;
    bool isPaused = false;


    // Use this for initialization
    void Start () {
        player = GetComponent<Rigidbody2D>();
        moveSpeed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        //Burn button
        //Pause button

        //Check for movement input
        if (Mathf.Abs(inputX) + Mathf.Abs(inputY) > 0 && isPaused == false)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Move the player if the player is putting movement input
        if (isMoving)
        {
            player.velocity = new Vector2(inputX * moveSpeed, inputY * moveSpeed);
        }
        else
        {
            player.velocity = new Vector2(0, 0);
        }



	}
}
