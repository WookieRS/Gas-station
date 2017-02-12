using UnityEngine;
using System.Collections;

public class Drink : MonoBehaviour {

	public float price;
	[Range(0.1f, 1f)]
	public float energy;
	public int drinkCapacity = 50;
	public int drinkCount = 5;
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


	//При повторном заходе срабатывает ложно, т.е. пока объект внутри триггера
	void OnTriggerExit (Collider collider){
		
		if (collider.gameObject == user) {
			occupied = false;
			user = null;
			needs = null;
			buying = false;
		}
	}

	void Buy(){
		if (needs) {
			if (needs.drink < 1) {
				if (drinkCount >= 1) {
					if (user.tag == "Buyer") {
						user.GetComponent<Buyer>().money -= price;
					}
					drinkCount--;
					needs.drink += energy;
					buying = false;
				} else{ 
					Debug.LogError ("Вода кончилась!");
				}
			} else {
				needs.drink = 1;
				needs.isBusy = false;
				needs.lookingFor = null;
			}
		}
	}
}
