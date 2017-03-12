using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalObject : MonoBehaviour {

	public GameController gameController;


	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (GetComponent<Transform> ().rotation.x) > 80) {
			gameController.score += 100;
		}	
	}
}
