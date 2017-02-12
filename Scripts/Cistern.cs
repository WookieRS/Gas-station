using UnityEngine;
using System.Collections;

public class Cistern : MonoBehaviour {


	public float maxCapacity;
	public float filled;
	public float capacity;
	public enum FuelType {diesel, ai92, ai95, ai98}; //usa, canada(aki method): 87, 89, 93

	public FuelType fuelType;

	void Start () {
		capacity = maxCapacity - filled;
	}

	public void AddFuel(float value){
		if (value <= capacity) {
			filled += value;
			capacity = maxCapacity - filled;
		} else Debug.Log ("Не влазит");
	}

	public void ReduceFuel (float value){
		if (filled >= value) {
			filled -= value;
			capacity += value;
		} else Debug.Log ("Пусто");
	}
}
