using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour {

	public bool collisionDetected = false;

	void OnTriggerStay(Collider collider) {
		if (collider) {
			collisionDetected = true;
		} else {
			collisionDetected = false;
		}
	}

	void OnTriggerExit(Collider collider) {
			collisionDetected = false;
	}

}
