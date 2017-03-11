using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

	public GameController gameController;
	private int scaleFactor = 15;
	private int rotationOffset = 60;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var hours = gameController.time/3600;
		var currentRotation = GetComponent<Transform> ().rotation;
		GetComponent<Transform> ().rotation = Quaternion.Euler (new Vector3 (scaleFactor * hours - rotationOffset, currentRotation.y, currentRotation.z));
	}
}
