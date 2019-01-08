using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionField : MonoBehaviour {

	//Global variables
    public Transform player;

	//sizes of the field
	private GameObject[,] groundTiles; 
    private Vector3 center; //the center of the action field (player's position)
    private float fieldHeight;
    private float fieldWidth;

    private float leftField;
    private float rightField;
    private float topField;
    private float bottomField;

	private int maxGroundTilesX; //max number of ground sprites on the X field
    private int maxGroundTilesY; //max number of ground sprites on the Y field

    //sizes of the player's pov

	private float sizeOfSprite;

    //for burnables
    private int burnableLayerMask;
    private GameObject[] burnables;
    public int maxBurnables;



	// Use this for initialization
	void Start () {

        center = player.position;
        sizeOfSprite = 1f; //dimensions of the sprites. They are 64 x 64 pixels
		
        //create the initial action field
		
		FindFieldHeight();
        FindFieldWidth();

        leftField = ((fieldWidth)) / 100 * -1;
        topField = ((fieldHeight)) / 100;
        rightField = center.x + ((fieldWidth)) / 100;
        bottomField = center.y - ((fieldHeight)) / 100;

		maxGroundTilesX = (int)(fieldWidth / 100f) * 2;
        maxGroundTilesY = (int)(fieldHeight / 100f) * 2;

		groundTiles = new GameObject[maxGroundTilesY, maxGroundTilesX];
		InitGroundTiles();

        //create the burnable objects
        burnableLayerMask = LayerMask.GetMask("Burnable");
        burnables = new GameObject[maxBurnables];
        InitBurnableObjects();


	}
	
	// Update is called once per frame
	void Update () {

		//move the actionfield as needed
		center = player.position; //true center of the field

        //Update the X and Y coordinates of the action field
        leftField = center.x - ((fieldWidth)) / 100;
        rightField = center.x + ((fieldWidth)) / 100;
        topField = center.y + ((fieldHeight)) / 100;
        bottomField = center.y - ((fieldHeight)) / 100;

        //Update the ground sprites within the action field
        UpdateGroundTiles();
        UpdateBurnables();
		
	}


    private void UpdateBurnables(){
        //loop through the burnables array to create new burnables
        for (int i = 0; i < maxBurnables; i++){

            Burnable component = burnables[i].gameObject.transform.GetChild(0).GetComponent<Burnable>();
            if (component.isBurning){
                //do not need to reposition it. just place a new one elsewhere
                Vector2 oldPosition = new Vector2(burnables[i].transform.position.x + 200f, burnables[i].transform.position.y);
                Vector2 newPosition = RepoBurnable(oldPosition);
                GameObject newBurnable = ChooseBurnable();
                newBurnable.GetComponent<Transform>().position = newPosition;

                burnables[i] = Instantiate(newBurnable, newPosition, Quaternion.identity, GameObject.Find("Burnables").transform) as GameObject;
                continue;
            }

            if (WithinActionField(burnables[i].transform.position)){
                //do nothing
            }
            else{
                //not within the action field
                //delete the ground sprite and place a new one that is within the field
                GameObject toDestroy = burnables[i];
                Vector2 newPosition = RepoBurnable(burnables[i].transform.position);

                GameObject newBurnable = ChooseBurnable();
                newBurnable.GetComponent<Transform>().position = newPosition;

                burnables[i] = Instantiate(newBurnable, newPosition, Quaternion.identity, GameObject.Find("Burnables").transform) as GameObject;

                Destroy(toDestroy);
            }
        }
    }


	private void UpdateGroundTiles(){
        //loop through the ground array to create ground sprites
        for (int i = 0; i < maxGroundTilesY; i++)
        {
            for (int j = 0; j < maxGroundTilesX; j++)
            {
                //Check if this sprite is within the action field
                if (WithinActionField(groundTiles[i,j].transform.position))
                {
                    //Within the action field
                    //Do nothing (may be changed later)
                }
                else
                {
                    //Not within the action field
                    //Delete the ground sprite and reposition it within the field
                    Vector3 newPosition = RepoGroundSprite(groundTiles[i, j].transform.position, i, j);
        

					Destroy(groundTiles[i, j].gameObject);

					//create the new tile and put it at the given position
					GameObject tile = ChooseGroundTile();
					tile.GetComponent<Transform>().position = newPosition;

					//add it to the groundTiles array
					//instantiate  into the scene
					groundTiles[i, j] = Instantiate(tile, newPosition, Quaternion.identity, GameObject.Find("Tiles").transform) as GameObject;
                }
            }
        }
	}



    /* Given a ground sprite that is OUTSIDE the action field, put it within the action field
     * BUT OUTSIDE the field of view of the player
     */
    private Vector2 RepoBurnable(Vector2 burnable){
        Vector2 newPosition = burnable;
        //randomly pic a place to spawn into the field
        float leftBorder = leftField;
        float rightBorder = rightField;
        float topBorder = topField;
        float bottomBorder = bottomField;

        int tries = 5;

        //burnable is beyond the leftfield. put it somewhere on the right
        if (burnable.x < leftField){
            //leftBorder = rightBorder;
            //rightBorder = rightBorder + 100;

            leftBorder = rightBorder;
        }

        //burnable is beyonf the rightfield. put it somewhere on the left
        else if (burnable.x > (center.x + ((fieldWidth)) / 100)){
            // leftBorder = leftBorder - 100;
            // rightBorder = leftBorder;
            rightBorder = leftBorder;
        }

        else{
            //nothing for now
        }

        //burnable is beyond the bottomfield. put it somewhere on the top
        if (burnable.y < bottomField){
            // bottomBorder = topBorder;
            // topBorder = topBorder + 100;

            bottomBorder = topField;
        }

        //burnable is beyond the topfield. put it somewhere on the bottom
        else if(burnable.y > topField){
            // topBorder = bottomBorder;
            // bottomField = bottomBorder - 100;

            topBorder = bottomBorder;
        }

        else{
            //nothing for now
        }

        bool validPosition = false;
        while (!validPosition && tries != 0){
            float randx = Random.Range(leftBorder, rightBorder);
            float randy = Random.Range(bottomBorder, topBorder);
            newPosition = new Vector2 (randx, randy);

            //check if there is no other burnable object already here
            Collider2D hit = Physics2D.OverlapCircle(newPosition, 1f, burnableLayerMask);
            if (hit == null){
                validPosition = true;
            }
            tries--;
            //if out of tries, can place outside the field and try again
            if (tries == 0){
                leftBorder += 100;
                rightBorder += 100;
                topBorder += 100;
                bottomBorder += 100;
                tries = 5;
            }
        }
        return newPosition;
    }





    /* Given a ground sprite that is OUTSIDE the action field, put it within the action field
     * Input: position of the ground sprite that is outside the action field, index of where the
     * sprite is in the ground sprite array in order to find the correct place to 
     * reposition in
     * Returns: new position of the ground sprite that is inside the action field
     */ 
    private Vector3 RepoGroundSprite(Vector3 tile, int i, int j)
    {
        Vector3 newPosition = tile;
        //Find where the sprite needs to be repositioned
        if (tile.x < leftField)
        {
            //sprite is beyond the left side of the field, can be put on the right side of the field
            //newPosition.x = rightField;
            j--;
            if (j < 0)
            {
                j = maxGroundTilesX - 1;
            }
            newPosition.x = groundTiles[i, j].transform.position.x + sizeOfSprite;
        }
        else if (tile.x > (center.x + ((fieldWidth)) / 100))
        {
            //sprite is beyond the right side of the field, can be put on the left side of the field
            //newPosition.x = leftField;
            j++;
            if (j > maxGroundTilesX - 1)
            {
                j = 0;
            }
            newPosition.x = groundTiles[i, j].transform.position.x - sizeOfSprite;
        }
        else
        {
            //nothing for now
        }
        if (tile.y < bottomField)
        {
            //sprite is beyond the bottom side of the field, can be put on the top side of the field
            //newPosition.y = topField;
            i++;
            if (i > maxGroundTilesY - 1)
            {
                i = 0;
            }
            newPosition.y = groundTiles[i, j].transform.position.y + sizeOfSprite;
        }
        else if (tile.y > topField)
        {
            //sprite is beyond the top side of the field, can be put on the bottom side of the field
            //newPosition.y = bottomField;
            i--;
            if (i < 0)
            {
                i = maxGroundTilesY - 1;
            }
            newPosition.y = groundTiles[i, j].transform.position.y - sizeOfSprite;
        }
        else
        {
            //nothing for now
        }

        //Debug.Log("new position: " + newPosition);
        return newPosition;
    }


	/* Check if a sprite is within the rectangular action fied by comparing the x and y coordinates
     * Input: Vector3 position of a specific sprite
     * Returns true if it is in the action field and false otherwise
     */
    private bool WithinActionField(Vector3 tile)
    {
        if (tile.x < leftField)
        {
            //sprite is beyond the left side of the field
            return false;
        }
        else if (tile.x > rightField)
        {
            //sprite is beyond the right side of the field
            return false;
        }
        else if (tile.y > topField)
        {
            //sprite is beyond the top side of the field
            return false;
        }
        else if (tile.y < bottomField)
        {
            //sprite is beyond the bottom side of the field
            return false;
        }

        //else, it is within range, return true
        return true;
    }


	/* Choose one of the program's 5 ground prefabs to load into the scene
	 * Return the prefab that was chosen
	 */
	private GameObject ChooseGroundTile(){

		GameObject tile = null;

		//randomize a tile to choose from
		int temp = Random.Range(1, 6);
		string dir = "Prefabs/ground" + temp;
		tile = Resources.Load<GameObject>(dir) as GameObject;

		return tile;
	}

	

    /* Spawn ground sprite game objects within the action field at start
     */
    private void InitGroundTiles()
    {
        //loop through the ground array to create ground tiles
        float offsetX = 0;
        float offsetY = 0;
        for (int i = 0; i < maxGroundTilesY; i++)
        {
            for (int j = 0; j < maxGroundTilesX; j++)
            {
                //add tiles to the action field
                if (groundTiles[i,j] == null) //if there is no groundTiles already in this index
                {

                    //find the position of where the sprite should go, according to the center
                    Vector3 groundPosition = center;
                    groundPosition.x = offsetX + leftField;
                    groundPosition.y = offsetY + topField;
                    offsetX += sizeOfSprite;

                    //create the new tile and put it at the given position
					GameObject tile = ChooseGroundTile();
					tile.GetComponent<Transform>().position = groundPosition;

					//add it to the groundTiles array
					//instantiate  into the scene
					groundTiles[i, j] = Instantiate(tile, groundPosition, Quaternion.identity, GameObject.Find("Tiles").transform) as GameObject;
                }
            }
            offsetX = 0;
            offsetY -= sizeOfSprite;
        }
    }


    private void InitBurnableObjects(){

        //randomly pic a place to spawn into the field
        for (int i = 0; i < maxBurnables; i++){
            bool validPosition = false;
            Vector2 position = new Vector2(0f,0f);
            while (!validPosition){
                float randx = Random.Range(leftField, rightField);
                float randy = Random.Range(bottomField, topField);
                position = new Vector2 (randx, randy);

                //check if there is no other burnable object already here
                Collider2D hit = Physics2D.OverlapCircle(position, 1f, burnableLayerMask);
                if (hit == null){
                    validPosition = true;
                }
            }
            //if this is a valid position, then place the burnable here
            GameObject burnable = ChooseBurnable();
            burnable.GetComponent<Transform>().position = position;

            //add to the burnables array
            burnables[i] = Instantiate(burnable, position, Quaternion.identity, GameObject.Find("Burnables").transform) as GameObject;

        }

    }


    private GameObject ChooseBurnable(){
        GameObject burnable = null;

        //return an object to choose from
        //int temp = Random.Range(1,4);
        int temp  = 1;
        string dir = "Prefabs/object" + temp;
        burnable = Resources.Load<GameObject>(dir) as GameObject;

        return burnable;
    }



	/* Find the correct value for the height of the action field
     * Action field must be a multiple of 64 in order to fit sprites
     * within the field
     */
    private void FindFieldHeight()
    {
        float minFieldHeight = Screen.height + 100;
        while (minFieldHeight % 100 != 0)
        {
            minFieldHeight++;
        }
        fieldHeight = minFieldHeight;
    }


	/* Find the correct value for the height of the action field
     * Action field must be a multiple of 64 in order to fit sprites
     * within the field
    */
    private void FindFieldWidth()
    {
        float minFieldWidth = Screen.width + 100;
        while (minFieldWidth % 100 != 0)
        {
            minFieldWidth++;
        }
        fieldWidth = minFieldWidth;
    }

}
