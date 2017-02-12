using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

	public float needToRefuel;
	public Cistern carCistern;
	public Buyer owner;
	public int passangers = 1;

	public bool onPosition;
	public bool passangersInStore;

	[Header("passangers")]						//Точки спауна/уничтожения пассажиров
	public Transform driver;
	public Transform passanger1;
	public Transform passanger2;
	public Transform passanger3;

	void Start(){
		//carCistern = GetComponent<Cistern>();
		CalculateRefuel ();

	}

	void Update(){
		if (onPosition) {
			//Когда подъехал к колонке/стоянке

		}
	}

	//Вычисляет сколько нужно заправить
	void CalculateRefuel(){
		float refuelFactor;

		refuelFactor = Random.Range (0.1f, 1f);

		if (refuelFactor < 0.5f) {
				needToRefuel = carCistern.capacity;
		}else	needToRefuel = carCistern.capacity * refuelFactor;
		
	}

































}
	
