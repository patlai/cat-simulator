using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public float timeOffset = 3600*8;

	public int timeRate;
	private int hours;
	private int minutes;
	private int seconds;
	public float time;
	public float introTime = 3f;

	//SCORE
	public int score;
	private int mediumScoreThreshold = 500;
	private int highScoreThreshold = 1000;

	//TEXT
	public Text inGameTimeText;
	public Text scoreText;
	public Text alertText;
	public Text dialogText;
	private float alertSpeed = 1f;
	private float nextAlert = 0;
	public Color white = new Color(1f,1f,1f,1f);

	//STATUS ICONS
	public Image humanStatusImage;
	public Image dialogImage;
	public Sprite humanGreen;
	public Sprite humanYellow;
	public Sprite humanRed;

	public bool hasGameEnded = false;

	// Use this for initialization	
	void Start () {
		//PlayIntro ();
	}

	// Update is called once per frame
	void Update () {
		updateTime ((Time.time) * timeRate + timeOffset);
		inGameTimeText.text = "Time: " + string.Format ("{0:00}:",hours) + string.Format("{0:00}:", minutes) + string.Format("{0:00}", seconds);


		

		scoreText.text = "Happiness: " + score;
		if (score > highScoreThreshold)
			humanStatusImage.sprite = humanRed;
		else if (score > mediumScoreThreshold)
			humanStatusImage.sprite = humanYellow;
		else
			humanStatusImage.sprite = humanGreen;
		if (hours >= 18) {
			EndGame ();
		}
		FlashAlertText ();
	}

	public void updateTime(float time){
		this.hours = (int)time / 3600;
		if (hours > 24)
			hours = hours % 24;
		this.minutes = (int)time / 60 - this.hours *60;
		this.seconds = (int)time - this.minutes * 60 - this.hours * 3600;
		this.time = time;
	}

	public void Alert(string text){
		if (Time.time > nextAlert) {
			alertText.color = white;
			alertText.text = "\"" + text + "\"" ;
			nextAlert = Time.time + alertSpeed;
			Debug.Log ("Alert");
		}

	}

	void FlashAlertText(){
		//Debug.Log (alertText.color);
		alertText.color = Color.Lerp (
			alertText.color,
			Color.clear,
			alertSpeed * Time.deltaTime);
	}

	void EndGame(){
		hasGameEnded = true;
		dialogImage.color = new Color (0f, 0f, 0f, 1f);
		PlayOutro ();
	}

	public void PlayIntro(){
		if (Time.time < 3)
			dialogText.text = "Bye kitty! Be nice, I’ll see you later.";
		
			
	}

	public void PlayOutro(){
		if (score > highScoreThreshold) {
			dialogText.text = "...";
			var x =(MovieTexture)dialogImage.GetComponent<GUITexture> ().texture;
			x.Play ();
		}
			
		else if (score > mediumScoreThreshold)
			dialogText.text = "Oh my god. What happened here, did I get robbed? This is getting way too out of hand. You’re staying out tonight.";
		else
			dialogText.text = "Kitty!! What have you done! This is not nice… You little messy fur ball";
	}

	public bool BeginGame(){
		return Time.time > introTime;
	}
}
