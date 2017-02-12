using UnityEngine;
using System.Collections;

public class Comfort : MonoBehaviour {

	public bool occupied;
	public float speed;

	public GameObject user;
	public GameObject userQueue;
	private Needs needs;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (occupied) {
			AddComfort();
		}

	}

	void OnTriggerEnter(Collider collider){

		if (collider.GetComponent<Needs>()) {
			user = collider.gameObject;
			needs = collider.gameObject.GetComponent<Needs>();
			occupied = true;
			userQueue = null;
			//TODO set animation, AddComfort() on end
		} 
	}

	void AddComfort(){
		if (needs.comfort < 1) {
			needs.comfort += speed * Time.deltaTime;
		} else if(needs.lookingFor == this) {
			needs.comfort = 1;
			needs.isBusy = false;
			needs.lookingFor = null;
			//TODO должен выйти как закночил
		}


	}

	void OnTriggerExit (Collider collider){
		if (collider.gameObject == user) {
			occupied = false;
			user = null;
			needs = null;
		}
	}
}
