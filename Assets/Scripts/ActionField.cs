using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/* Defines the "Action Field" of the player. More on Action Field in documentation.
 * Action Field follows the Player and moves in a similar way to the Main Camera
 */
public class ActionField : MonoBehaviour {


    //Global variables
    public Transform player;

    Vector3 center; //the center of the action field (player's position)
    private float fieldHeight;
    private float fieldWidth;
    private float fieldX; //the x coordinate of the top left corner of the action field
    private float fieldY; //the y coordinate of the top left corner of the action field

    int numGroundSprites; //current number of ground sprites
    int maxGroundSprites; //max number of ground sprites
    int maxGroundSpritesX; //max number of ground sprites on the X field
    int maxGroundSpritesY; //max number of ground sprites on the Y field
    GroundSprite[,] groundSprites;

    float sizeOfSprite = 0.32f;


	// Use this for initialization
	void Start () {

        //initialize the action field
        FindFieldHeight();
        FindFieldWidth();
        fieldX = ((fieldWidth)) / 100 * -1;
        fieldY = ((fieldHeight)) / 100;

        //initialize ground sprite array and ground
        maxGroundSprites = (int)((fieldHeight * fieldWidth) / 32f);
        maxGroundSpritesX = (int)(fieldWidth / 32f) * 2;
        maxGroundSpritesY = (int)(fieldHeight / 32f) * 2;

        Debug.Log("maxGroundSpritesX: " + maxGroundSpritesX);
        Debug.Log("maxGroundSpritesY: " + maxGroundSpritesY);
        Debug.Log("Fieldheight: " + fieldHeight);
        Debug.Log("fieldwidth: " + fieldWidth);

        groundSprites = new GroundSprite[maxGroundSpritesY, maxGroundSpritesX];
        InitializeGroundSprites();



	}
	


	// Update is called once per frame
	void Update () {
        center = player.position; //true center of the field

        //Update the X and Y coordinates of the action field
        fieldX = center.x - (fieldWidth / 2);
        fieldY = center.y + (fieldHeight / 2);

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


    /* Spawn ground sprite game objects within the action field at start
     */
    private void InitializeGroundSprites()
    {
        //loop through the ground array to create ground sprites
        float offsetX = sizeOfSprite;
        float offsetY = sizeOfSprite;
        for (int i = 0; i < maxGroundSpritesY; i++)
        {
            for (int j = 0; j < maxGroundSpritesX; j++)
            {
                //add sprites to the action field
                if (groundSprites[i,j] == null) //if there is no groundSprite already in this index
                {

                    //find the position of where the sprite should go, according to the center
                    Vector3 groundPosition = center;
                    groundPosition.x = offsetX + fieldX;
                    groundPosition.y = offsetY + fieldY;
                    offsetX += sizeOfSprite;

                    //create the new sprite
                    groundSprites[i, j] = new GroundSprite();
                    groundSprites[i,j].NewGroundSprite(groundPosition);
                }
            }
            offsetX = sizeOfSprite;
            offsetY -= sizeOfSprite;
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
