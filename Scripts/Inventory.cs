using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	public GameObject[] inventoryArray;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddItem(GameObject gameObjectItem){
		int i = 0;

		foreach (var item in inventoryArray) {

			if (item == null) {
				inventoryArray.SetValue(gameObjectItem, i);
				return;
			}
			if (i < inventoryArray.Length-1) {
				i++;
			} else {
				Debug.LogError ("Нет места в инвентаре");
			} 
		}
	}

	public void RemoveItem(GameObject gameObjectItem){
		int i = 0;
		
		foreach (var item in inventoryArray) {
			
			if (item.gameObject == item) {
				inventoryArray.SetValue(null, i);
				return;
			}
			if (i < inventoryArray.Length-1) {
				i++;
			} else {
				Debug.LogError ("Данного предмета в инвентаре нет");
			} 
		}
	}


	public GameObject GetItemByComponentName(string component){
		int i = 0;
		
		foreach (var item in inventoryArray) {
			
			if (item!= null && item.GetComponent (component)) {
				return item.gameObject;
			}
			if (i < inventoryArray.Length-1) {
				i++;
			} else {
				Debug.LogError (component + " не найден!");
			}
		}
		return null;
	}
}
