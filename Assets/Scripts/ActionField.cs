using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/* Defines the "Action Field" of the player. More on Action Field in documentation.
 * Action Field follows the Player and moves in a similar way to the Main Camera
 */
public class ActionField : MonoBehaviour {

    private float fieldHeight;
    private float fieldWidth;
    public Transform player;
    Vector3 center;

    int numGroundSprites; //current number of ground sprites
    int maxGroundSprites; //max number of ground sprites
    int maxGroundSpritesX; //max number of ground sprites on the X field
    int maxGroundSpritesY; //max number of ground sprites on the Y field

	// Use this for initialization
	void Start () {

        //initialize the action field
        FindFieldHeight();
        FindFieldWidth();

        //initialize ground sprite array and ground
        maxGroundSprites = (int)(fieldHeight * fieldWidth) / 32;
        maxGroundSpritesX = (int)fieldHeight / 32;
        maxGroundSpritesY = (int)fieldWidth / 32;
        GroundSprite[,] groundSprites = new GroundSprite[maxGroundSpritesY, maxGroundSpritesX];


	}
	


	// Update is called once per frame
	void Update () {
        center = player.position; //true center of the field
        Debug.Log("Action field coordinates: " + center);
	}



    //Gui to see the action field. Used for debugging
    private void OnGUI()
    {
        GUI.Box(new Rect(center.x, (center.y * -1), fieldWidth, fieldHeight), "Action Field");
    }


 
    /* Spawn ground sprite game objects, within the action field and destroy 
     * ground sprite game objects outside of the field.
     */
    private void UpdateGroundSprites()
    {
        //loop through the ground array to create ground sprites
        for (int i = 0; i < maxGroundSpritesX; i++)
        {
            for (int j = 0; j < maxGroundSpritesY; j++)
            {
                //Check if this sprite is within the action field
            }
        }
        
    }


    /* Find the correct value for the height of the action field
     * Action field must be a multiple of 32 in order to fit sprites
     * within the field
     */
    private void FindFieldHeight()
    {
        float minFieldHeight = Screen.height + 100;
        while (minFieldHeight % 32 != 0)
        {
            minFieldHeight++;
        }
        fieldHeight = minFieldHeight;
    }



    /* Find the correct value for the height of the action field
    * Action field must be a multiple of 32 in order to fit sprites
     * within the field
    */
    private void FindFieldWidth()
    {
        float minFieldWidth = Screen.width + 100;
        while (minFieldWidth % 32 != 0)
        {
            minFieldWidth++;
        }
        fieldWidth = minFieldWidth;
    }



}
