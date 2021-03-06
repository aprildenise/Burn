﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour {

    //extra fields to go with the burnable objects prefabs
    public Transform player; //Transform of the player object
    //public string name; //optional
    public int dimensions; //dimensions of the object, usually the dimensions of the sprite

    private float interactionRadius = 2.0f;

    public float burnTime; //duration in which the object remains burning until it is deleted
    public bool isBurning = false; //if the object is burning

    public LayerMask notBurningLayer; //Used with raycasting. All BurnableObjects begin with this layer (set during initialization)
    public LayerMask burningLayer; //Used with raycasting. All BurnableObjects are put in this layer when burning
    //public LayerMask currentLayer;



    /* Update is called once per frame
     * Used to check for the burning input from the player.
     */
    void LateUpdate () {

        //Check if the player is in range and wants to begin burning this object
        //Get the distance from the player and the object
        float distance = Vector3.Distance(player.position, transform.position);
        bool inRange = false;
        if (distance <= interactionRadius)
        {
            //player is in range and can burn the object
            inRange = true;
            //Debug.Log("In range");
        }
        else
        {
            inRange = false;
        }

        //Check if the player hit the "burn/action" key
        if (inRange && Input.GetKeyDown("space") && isBurning == false)
        {
            Debug.Log("BURN!");
            Burn(); //Begin to burn the object
        }

	}


    /* Check for spread by checking if there is any other burnable object within the
     * interaction radius.
     */
    void Update()
    {
        if (isBurning)
        {
            Collider2D hit;

            //If the raycast colliders with another gameobject that is not burning
            //hit = Physics2D.CircleCast(playerPosition, interactionRadius, new Vector3(0, 0, 0), 0f, notBurningLayer);
            hit = Physics2D.OverlapCircle(gameObject.transform.position, interactionRadius, notBurningLayer.value);
            if (hit)
            {
                Debug.Log("Spreading...");
                //spread the fire to this object and make it burn as well
                GameObject thing = hit.transform.gameObject;
                BurnableObject burningThing;
                burningThing = thing.GetComponent<BurnableObject>();
                burningThing.burnTime += 5f;
                burningThing.Burn();

            }
            else
            {
                Debug.Log("No spread...");
            }
        }
    }

    

    /* Begin burning this gameobject
     */
    public void Burn()
    {
        changeLayer(9);
        isBurning = true;
        StartCoroutine("Wait");
        //Destroy this object once it is finished burning
    }



    /* Timer for Burn function. waits a definite number of seconds before the object
     * is completely burned and destroyed.
     */
    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(burnTime);
        Debug.Log("Finished burning.");
        Destroy(gameObject);
    }



    /* Helper function. Change the layer of the this gameobject
     * Input: layer where the gameobject is to go to
     */ 
    public void changeLayer(int newLayer)
    {
        gameObject.layer = newLayer;
    }

}
