using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class catHealth : MonoBehaviour {

	public int startingHealth = 100;
	public int currentHealth;
	public Slider healthSlider;
	public Image damageImage;

	public AudioClip deathClip;
	public AudioSource hitSound;
	public float flashSpeed = 5f;
	public float sleepSpeed = 0.2f;
	public Color flashColor = new Color(1f,0f,0f);
	public Color asleepColor = new Color (0f, 0f, 0f);


	Animator anim;
	AudioSource playerAudio;
	//PlayerMovement playerMovement;
	bool isDead;
	bool damaged;


	void Awake(){
		anim = GetComponent<Animator> ();
		playerAudio = GetComponent<AudioSource> ();
		currentHealth = startingHealth;
	}
		

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (damaged) {
			damageImage.color = flashColor;
		} else if(!damaged) {
			damageImage.color = Color.Lerp (
				damageImage.color,
				Color.clear,
				flashSpeed * Time.deltaTime);
		}



		damaged = false;
		if (Input.GetKeyDown ("h"))
			TakeDamage (5);
		
			

	}

	public void TakeDamage(int amount){
		damaged = true;
		currentHealth -= amount;
		healthSlider.value = currentHealth;
		hitSound.Play ();
		if (currentHealth <= 0 && !isDead)
			Death ();
			
	}

	void Death(){
		isDead = true;
		anim.SetTrigger ("Die");

		playerAudio.clip = deathClip;
		playerAudio.Play ();
	}

}
