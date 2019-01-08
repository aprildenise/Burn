using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour {

	//references
	private Animator animator;
	private GameObject parent;
	public Light light1;
	public Light light2;
	public Light light3;


	//burning operations
	private int burnableLayerMask;
	private bool canBurn;
	public bool isBurning;
	private bool light1On;
	private bool light2On;
	private bool light3On;


	// Use this for initialization
	void Start () {
		parent = gameObject.transform.parent.gameObject;
		animator = parent.GetComponent<Animator>();
		canBurn = false;
		isBurning = false;
		light1On = false;
		light2On = false;
		light3On = false;
		burnableLayerMask = LayerMask.GetMask("Burnable");
	}
	
	// Update is called once per frame
	void Update () {

		//begin burning
		if (Input.GetButtonDown("Jump") && canBurn){
			StartBurning();
		}

		if (isBurning){
			CheckBurning();
		}
	}


	public void StartBurning(){
		if (!isBurning){
			canBurn = false;
			isBurning = true;
			animator.enabled = true;
			if (!light1On){
				light1On = true;
				light1.enabled = true;
			}
		}
	}


	private void CheckBurning(){
		//Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Tree1_Burn2"));

		if (!light2On && animator.GetCurrentAnimatorStateInfo(0).IsName("Tree1_Burn2")){
			light2On = true;
			light2.enabled = true;
		}

		if (!light3On && animator.GetCurrentAnimatorStateInfo(0).IsName("Tree1_Burn3")){
			light3On = true;
			light3.enabled = true;
			SpreadBurn();
		}

		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 40f){
			Destroy(parent);
		}
	}


	private void SpreadBurn(){
		Collider2D[] hit = Physics2D.OverlapCircleAll(parent.transform.position, 2f, burnableLayerMask);
		foreach (Collider2D target in hit){
			//Debug.Log(target.gameObject.name);
			//skip the target that points to this gameobject
			if (target.gameObject == this.gameObject){
				continue;
			}
			else{
				if (target.gameObject.tag == "Burnable"){
					Burnable burnable = target.gameObject.transform.GetChild(0).GetComponent<Burnable>();
					burnable.StartBurning();
				}
			}
		}

	}


	private void OnTriggerStay2D(Collider2D other){
		if (!isBurning && other.gameObject.tag == "Player"){
			canBurn = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			canBurn = false;
		}
	}
}
