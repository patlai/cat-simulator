using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterController : MonoBehaviour {

	public float speed;
	public float jumpHeight;

	public catHealth health;

	public AudioSource meowSound;
	public AudioSource purrSound;

	public float meowRate;
	private float nextMeow;

	public GameController gameController;

	public float sleepHours;


	public Image sleepImage;
	public Color flashColor = new Color(0f,0f,0f);
	private float sleepRate = 0.5f;
	private bool isAsleep = false;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update () {
		float translation = Input.GetAxis ("Vertical") * speed;
		float strafe = Input.GetAxis ("Horizontal") * speed;
		translation *= Time.deltaTime;
		strafe *= Time.deltaTime;

		transform.Translate (strafe, 0, translation);



		if (isAsleep) {
			sleepImage.color = flashColor;
		} else if(!isAsleep) {
			sleepImage.color = Color.Lerp (
				sleepImage.color,
				Color.clear,
				sleepRate * Time.deltaTime);
		}

		isAsleep = false;

		if(Input.GetKeyDown("escape"))
			Cursor.lockState = CursorLockMode.None;

		if (Input.GetKeyDown("space")) 
			Jump();

		if (Input.GetKeyDown ("f"))
			Meow();
		else if (Input.GetKeyDown ("g"))
			purrSound.Play ();
		else if (Input.GetKeyDown("u"))
			FallAsleep();




	}

	void OnCollisionEnter (Collision collision)  //Plays Sound Whenever collision detected
	{	
		if (collision.gameObject.tag != "Floor") {
			if (collision.gameObject.tag == "GoodObject") {
				gameController.score += 10;
				Meow ();
			}
			else if (collision.gameObject.tag == "BadObject")
				health.TakeDamage (5);
			

			//Debug.Log (gameController.score);
		}
			

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



	void FallAsleep(){
		isAsleep = true;
		gameController.timeOffset += sleepHours * 3600;
	}


}
