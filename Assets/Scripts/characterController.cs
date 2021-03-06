﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterController : MonoBehaviour {
	

	public Text ToolTipText;

	public Text PoopCountText;

	private Animator anim;

	//POOP
	private int startingPoopCount = 5;
	private int poopCount = 5;
	public GameObject poopObject;
	public Transform poopSpawn;

	//MOVEMENT
	public float speed;
	public float jumpHeight;
	private float jumpRate = 0.5f;
	private float nextJump = 0.0f;
	private float recoilFactor = -3;
	private bool canMove = true;

	public catHealth health;

	//SOUNDS
	public AudioSource meowSound;
	public AudioSource purrSound;
	public AudioSource poopSound;
	public AudioSource scratchSound;

	private float meowRate = 0.5f;
	private float nextMeow;

	public GameController gameController;

	public float sleepHours;

	public Image sleepImage;
	public Color flashColor = new Color(0f,0f,0f);
	private float sleepRate = 0.5f;
	private bool isAsleep = false;

	private bool canEat = false;


	private string[] goodMessages = new string[] {
		"mmm...I like",
		"Yes, that's good",
		"ehehehe",
		"Interesting",
		"One day I will rule the world",
		"I hope my slave will come home and feed me soon"
	};

	private string[] badMessages = new string[] {
		"NOOOO",
		"This is why we can't have nice things"
	};

	private string[] neutralMessages = new string[]{
		"Maybe the car ride DOESN'T end when I've meowed enough...",
		"Why does the human get mad when I wake him up? \n" +
		" He could just sleep during the day like me.",
		"Why do humans not lick their own genitals",
		"He still thinks I trip him by accident...",
		"I am beginning to doubt that the red dot cannot be caught...",

	};


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update () {
		

		if (gameController.hasGameEnded) {
			canMove = false;
		}	
		float translation = Input.GetAxis ("Vertical") * speed;
		float strafe = Input.GetAxis ("Horizontal") * speed;
		float initTrans = translation;
		float initStrafe = strafe;
		translation *= Time.deltaTime;
		strafe *= Time.deltaTime;
		if (canMove && gameController.BeginGame() ) {
			

			transform.Translate (strafe, 0, translation);
		}
			

		if (isAsleep) {
			sleepImage.color = flashColor;
		} else if(!isAsleep) {
			sleepImage.color = Color.Lerp (
				sleepImage.color,
				Color.clear,
				sleepRate * Time.deltaTime);
		}

		isAsleep = false;

		if (canEat) {
			ToolTipText.text = "Press 'E' to eat";
		} else {
			ToolTipText.text = "";
		}

		if(Input.GetKeyDown("escape"))
			Cursor.lockState = CursorLockMode.None;
//
//		if (Input.GetKeyDown ("space") && Time.time > nextJump) {
//			Jump ();
//			nextJump = Time.time + jumpRate;
//		}

		if (initTrans != translation || initStrafe != strafe)
		{
			anim.SetBool("jump", false);
			anim.SetBool("walk", true);
		}
		else
			anim.SetBool("walk", false);

		if (Input.GetKeyDown("escape"))
			Cursor.lockState = CursorLockMode.None;

		if (Input.GetKeyDown("space")&& Time.time > nextJump)
		{
			anim.SetBool("walk", false);
			anim.SetBool("jump", true);
			Jump ();	
			nextJump = Time.time + jumpRate;
		}
		if(GetComponent<Rigidbody>().velocity.y < 1.5)
		{
			//Debug.Log(GetComponent<Rigidbody>().velocity.y);
			anim.SetBool("jump", false);
		}

		if (Input.GetKeyDown ("f"))
			Meow ();
		else if (Input.GetKeyDown ("g"))
			purrSound.Play ();
		else if (Input.GetKeyDown ("u"))
			FallAsleep ();
		else if (Input.GetKeyDown ("e") && canEat)
			Eat ();
		else if (Input.GetKeyDown ("p") && canPoop())
			Poop ();

		PoopCountText.text = "Poops left: " + poopCount;



	}

	void OnCollisionEnter (Collision collision)  //Plays Sound Whenever collision detected
	{	

		if (collision.gameObject.tag != "Floor") {
			if (collision.gameObject.tag == "GoodObject") {
				gameController.score += 10;
				Meow ();
				int randomGood = Random.Range (0, goodMessages.Length);
				gameController.Alert (goodMessages [randomGood]);
			} else if (collision.gameObject.tag == "BadObject") {
				health.TakeDamage (5);
				int randomBad = Random.Range (0, badMessages.Length);
				gameController.Alert (badMessages [randomBad]);
				Recoil ();
			} else {
				int randomNeutral = Random.Range (0, neutralMessages.Length);
				gameController.Alert (neutralMessages [randomNeutral]);
			}
				

			if (collision.gameObject.tag == "Food") {
				canEat = true;
				Debug.Log ("can eat");
			}
				
			Debug.Log (collision.gameObject.tag);

			//Debug.Log (gameController.score);
		}
			

	}

	void OnCollisionExit(Collision collision){
		Debug.Log ("collision exit");
		canEat = false;
	}

//	void OnTriggerEnter(Collider other){
//		if (other.tag != "Floor") {
//			Meow ();
//		}
//	}

	void Meow()
	{
		if (Time.time > nextMeow) {
			nextMeow = Time.time + meowRate;
			meowSound.Play ();
		}

	}

	void Jump() { 
		//GetComponent<Animation>().Play ("jump_pose"); 
		GetComponent<Rigidbody>().velocity = new Vector3(
			GetComponent<Rigidbody>().velocity.x,
			GetComponent<Rigidbody>().velocity.y+jumpHeight,
			GetComponent<Rigidbody>().velocity.z);
	}

	void Recoil(){
		GetComponent<Rigidbody>().velocity = new Vector3(
			recoilFactor + GetComponent<Rigidbody>().velocity.x,
			GetComponent<Rigidbody>().velocity.y,
			recoilFactor + GetComponent<Rigidbody>().velocity.z);
		Debug.Log (GetComponent<Rigidbody> ().velocity);
	}


	void FallAsleep(){
		isAsleep = true;
		gameController.timeOffset += sleepHours * 3600;
		health.currentHealth = health.startingHealth;
		health.healthSlider.value = health.currentHealth;
	}

	public void Eat(){
		Debug.Log ("eat");
		Meow ();
		health.currentHealth = 100;
		health.healthSlider.value = health.currentHealth;
		poopCount = startingPoopCount;
		gameController.Alert ("Now I can poop more :3");
	}

	public void Poop(){
		if (poopCount > 0) {
			poopCount--;
			Instantiate(poopObject, GetComponent<Rigidbody>().position, GetComponent<Rigidbody>().rotation);
			gameController.score += 50;
			poopSound.Play();
		}
	}

	private bool canPoop(){
		return poopCount > 0;
	}

	public void Scratch(){
		gameController.score += 25;
		scratchSound.Play ();
		gameController.Alert ("Anything and everything is technically a scratching post");
	}




}
