using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Defines the controls the player can take:
 * Movement (using the arrow keys/WASD keys)
 * Burn
 * Pause/unpause
 */

public class PlayerController : MonoBehaviour {



    //for player movement
    private bool isMoving;
    private Vector2 lastMove;
    private float moveSpeed;
    private Rigidbody2D player;
    private Animator animator; 

    //for pausing
    private bool isPaused = false;


    // Use this for initialization
    void Start () {
        player = GetComponent<Rigidbody2D>();
        moveSpeed = 1.5f;
        animator = GetComponent<Animator>();
        isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {


        //check for movement input
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        //Burn button
        //Pause button

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
            lastMove = new Vector2(inputX, inputY);
        }
        else
        {
            player.velocity = new Vector2(0, 0);
        }


        //update the animator as needed
        animator.SetFloat("MoveX", inputX);
        animator.SetFloat("MoveY", inputY);
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);

	}
}
