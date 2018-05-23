﻿using System.Collections;
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
    private float leftField;
    private float rightField;
    private float topField;
    private float bottomField;
    private Vector3 field; //Vector3 coordinates of the top left corner of the action field

    int numGroundSprites; //current number of ground sprites
    int maxGroundSprites; //max number of ground sprites
    int maxGroundSpritesX; //max number of ground sprites on the X field
    int maxGroundSpritesY; //max number of ground sprites on the Y field
    GroundSprite[,] groundSprites;

    float sizeOfSprite = 0.32f;


    //bool tester = false;


	// Use this for initialization
	void Start () {

        //initialize the action field
        FindFieldHeight();
        FindFieldWidth();
        leftField = ((fieldWidth)) / 100 * -1;
        topField = ((fieldHeight)) / 100;
        field = new Vector3(leftField, topField, 0);

        //Debug.Log("field: " + field);

        //initialize ground sprite array and ground
        maxGroundSprites = (int)((fieldHeight * fieldWidth) / 32f);
        maxGroundSpritesX = (int)(fieldWidth / 32f) * 2;
        maxGroundSpritesY = (int)(fieldHeight / 32f) * 2;

        groundSprites = new GroundSprite[maxGroundSpritesY, maxGroundSpritesX];
        InitializeGroundSprites();

	}
	



	// Update is called once per frame
	void Update () {
        center = player.position; //true center of the field

        //Update the X and Y coordinates of the action field
        leftField = center.x - ((fieldWidth)) / 100;
        rightField = center.x + ((fieldWidth)) / 100;
        topField = center.y + ((fieldHeight)) / 100;
        bottomField = center.y - ((fieldHeight)) / 100;
        field = new Vector3(leftField, topField, 0);

        //Debug.Log("field: " + field);

        //Update the ground sprites within the action field
        UpdateGroundSprites();


    }




    //Gui to see the action field. Used for debugging
    private void OnGUI()
    {
        GUI.Box(new Rect(center.x, (center.y * -1), fieldWidth, fieldHeight), "Action Field");
    }


 
    /* Destroy grounf sprites that are outside the action field and place new
     * ground sprites within the action field
     */
    private void UpdateGroundSprites()
    {
        //loop through the ground array to create ground sprites
        for (int i = 0; i < maxGroundSpritesY; i++)
        {
            for (int j = 0; j < maxGroundSpritesX; j++)
            {
                //Check if this sprite is within the action field
                if (WithinActionField(groundSprites[i,j].position))
                {
                    //Within the action field
                    //Do nothing (may be changed later)
                    //Debug.Log("witin field");
                }
                else
                {
                    //Not within the action field
                    //Delete the ground sprite and reposition it within the field
                    //Debug.Log("Need to repo sprite at: " + groundSprites[i,j].position);
                    Vector3 newPosition = RepoGroundSprite(groundSprites[i, j].position);
                    groundSprites[i, j].DestroyGroundSprite();
                    groundSprites[i, j].NewGroundSprite(newPosition);
                    //Debug.Log("New sprite placed at: " + groundSprites[i,j].position);
                    //tester = true;
                }
            }
        }
    }




    /* Check if a sprite is within the rectangular action fied by comparing the x and y coordinates
     * Input: Vector3 position of a specific sprite
     * Returns true if it is in the action field and false otherwise
     */
    private bool WithinActionField(Vector3 sprite)
    {
        if (sprite.x < leftField)
        {
            //sprite is beyond the left side of the field
            return false;
        }
        else if (sprite.x > rightField)
        {
            //sprite is beyond the right side of the field
            return false;
        }
        else if (sprite.y > topField)
        {
            //sprite is beyond the top side of the field
            return false;
        }
        else if (sprite.y < bottomField)
        {
            //sprite is beyond the bottom side of the field
            return false;
        }

        //else, it is within range, return true
        return true;
    }




    /* Given a ground sprite that is OUTSIDE the action field, put it within the action field
     * Input: position of the ground sprite that is outside the action field
     * Returns: new position of the ground sprite that is inside the action field
     */ 
    private Vector3 RepoGroundSprite(Vector3 sprite)
    {
        Vector3 newPosition = sprite;
        //Find where the sprite needs to be repositioned
        if (sprite.x < leftField)
        {
            //sprite is beyond the left side of the field, can be put on the right side of the field
            newPosition.x = rightField;
        }
        else if (sprite.x > (center.x + ((fieldWidth)) / 100))
        {
            //sprite is beyond the right side of the field, can be put on the left side of the field
            newPosition.x = leftField;
        }
        else
        {
            //nothing for now
        }
        if (sprite.y < bottomField)
        {
            //sprite is beyond the bottom side of the field, can be put on the top side of the field
            newPosition.y = topField;
        }
        else if (sprite.y > topField)
        {
            //sprite is beyond the top side of the field, can be put on the bottom side of the field
            newPosition.y = bottomField;
        }
        else
        {
            //nothing for now
        }

        //Debug.Log("new position: " + newPosition);
        return newPosition;
    }





    /* Spawn ground sprite game objects within the action field at start
     */
    private void InitializeGroundSprites()
    {
        //loop through the ground array to create ground sprites
        float offsetX = 0;
        float offsetY = 0;
        for (int i = 0; i < maxGroundSpritesY; i++)
        {
            for (int j = 0; j < maxGroundSpritesX; j++)
            {
                //add sprites to the action field
                if (groundSprites[i,j] == null) //if there is no groundSprite already in this index
                {

                    //find the position of where the sprite should go, according to the center
                    Vector3 groundPosition = center;
                    groundPosition.x = offsetX + leftField;
                    groundPosition.y = offsetY + topField;
                    offsetX += sizeOfSprite;

                    //create the new sprite
                    groundSprites[i, j] = new GroundSprite();
                    groundSprites[i,j].NewGroundSprite(groundPosition);
                    //Debug.Log("Initialized sprite at: " + groundSprites[i, j].position);
                }
            }
            offsetX = 0;
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
