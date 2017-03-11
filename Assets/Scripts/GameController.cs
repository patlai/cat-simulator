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

	public int score;

	public Text inGameTimeText;
	public Text scoreText;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		updateTime ((Time.time) * timeRate + timeOffset);

		inGameTimeText.text = "Time: " + string.Format ("{0:00}:",hours) + string.Format("{0:00}:", minutes) + string.Format("{0:00}", seconds);
		scoreText.text = "Score: " + score;
	}

	public void updateTime(float time){
		this.hours = (int)time / 3600;
		if (hours > 24)
			hours = hours % 24;
		this.minutes = (int)time / 60 - this.hours *60;
		this.seconds = (int)time - this.minutes * 60 - this.hours * 3600;
		this.time = time;
	}
}
