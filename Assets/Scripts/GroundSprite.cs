using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSprite : MonoBehaviour {

    public bool active = false; //true if gameobject exists, false if no gameobjects exists
    public Vector3 position = new Vector3(0,0,0); //coordinate position of the gameobject
    public GameObject reference = null; //reference to the gameobject of the sprite


    /* Contructor for the GroundSprite object. 
     * Input: position of where the sprite should be
     */
    public void NewGroundSprite(Vector3 position)
    {
        active = true;
        this.position = position;
        reference = InstantiateSprite(position);
    }



    /* Destroys the gameobject in this.GroundSprite and nulls field
     */
    public void DestroyGroundSprite()
    {
        active = false;
        position = new Vector3(0, 0, 0);
        Destroy(reference);
        reference = null;
    }


    /* Create the game object for this sprite, using the given
     * parameters.
     * Return the new gameobject
     */
    private GameObject InstantiateSprite(Vector3 position)
    {
        GameObject ground = new GameObject();
        ground.AddComponent<SpriteRenderer>();
        //Choose a sprite to use
        int temp = Random.Range(1, 4);
        string directory = "Sprites/ground" + temp + ".png";
        //get the sprite to use
        Sprite spriteImage = Resources.Load<Sprite>(directory);
        ground.GetComponent<SpriteRenderer>().sprite = spriteImage;

        //put the sprite in the given position
        ground.AddComponent<Transform>();
        ground.GetComponent<Transform>().position = position;

        return ground;
    }





   
}

