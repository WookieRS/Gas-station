using UnityEngine;
using System.Collections;

public class Alarms : MonoBehaviour {


	public bool fireDetected;
	public GameObject fireSign;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("DetectFire", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DetectFire(){
		GameObject fire = GameObject.FindGameObjectWithTag ("Fire");
		if (fire) {
			fireDetected = true;
			fireSign.SetActive (true);
		} else {
			fireDetected = false;
			fireSign.SetActive (false);
		}

	}
}
