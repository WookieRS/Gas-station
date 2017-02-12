using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	public float price;
	[Range(0.1f, 1f)]
	public float energy;
	public int foodCapacity = 50;
	public int foodCount = 5;
	public bool occupied;
	public bool buying;

	private Needs needs;
	public GameObject user;
	public GameObject userQueue;

	void Update () {
		if (occupied && !buying) {
			buying = true;
			Invoke("Buy", 2);
		}
	}


	void OnTriggerEnter(Collider collider){

		if (!occupied && collider.GetComponent<Needs>()) {
			user = collider.gameObject;
			needs = collider.gameObject.GetComponent<Needs>();
			occupied = true;
			userQueue = null;
			//TODO buyfood animation, BuyDrink() on end
		} 
	}

	void Buy(){
		if (needs) {
			if (needs.food < 1) {						//вызывал ошибку из-за invokе. При проверке компонент уже отключен
				if (foodCount >= 1) {
					if (user.tag == "Buyer") {
						user.GetComponent<Buyer>().money -= price;
					}
					foodCount--;
					needs.food += energy;
					buying = false;
				} else{ 
					Debug.LogError ("Еда кончилась!");
				}
			} else {
				needs.food = 1;
				needs.isBusy = false;
				needs.lookingFor = null;
			}
		}
	}

	void OnTriggerExit (Collider collider){
		if (collider.gameObject == user) {
			occupied = false;
			user = null;
			needs = null;
			buying = false;
		}
	}
}
