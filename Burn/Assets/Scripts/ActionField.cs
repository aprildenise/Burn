using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/* Defines the "Action Field" of the player. More on Action Field in documentation.
 * Action Field follows the Player and moves in a similar way to the Main Camera
 */
public class ActionField : MonoBehaviour {

    private float fieldLength = 15f;
    private float fieldWidth = 10f;
    public Transform player;
    private bool inRange;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /* Gizmos to see the ActionField. Used for debugging.
     */
    private void OnDrawGizmosSelected()
    {
        if (player == null)
        {
            player = transform;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(fieldLength, fieldWidth, 0));
    }
    
}
