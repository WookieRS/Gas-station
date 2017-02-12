using UnityEngine;
using System.Collections;

public class Dispenser : MonoBehaviour {

	[Range(0.1f,10f)]
	public float speed;
	public bool fillInProcess;
	public bool waitingForPaid;
	public bool payed;
	public bool refillComplete;
	public float needToRefuel;

	public Car connectedCar;
	public Cistern[] cisternsArray;
	public Cistern currentCistern;


	private float fuelReserved = 100; 			//Пока здесь, потом через контроллер колонок/gamecontroller

	void Start () {
	
	}
	

	void Update () {
		if (connectedCar && !refillComplete) {
			if (!waitingForPaid && CheckFuelType ()  ){
				CheckReservedFuel ();
			}

			if (waitingForPaid && !fillInProcess) {
				WaitForPay ();
			}

			if (fillInProcess) {
				AddFuelAtCar ();
			}

		}
	}

	//Проверяем подключены ли цистрены с нужным топливом
	bool CheckFuelType(){						
		foreach (var cistern in cisternsArray) {
			if (cistern) {
				if (cistern.fuelType == connectedCar.carCistern.fuelType) {
					currentCistern = cistern;
					return true;
				}
			} 
		}
		Debug.Log ("К колонке не подключены цистерны с подходящим топливом");
		currentCistern = null;
		CarOut ();
		return false;
	}

	void CheckReservedFuel (){
		needToRefuel = connectedCar.needToRefuel;
		if (currentCistern.filled - (needToRefuel + fuelReserved) > 0) {
			fuelReserved += needToRefuel;
			//Debug.Log ("Топлива достаточно, ожидается оплата");
			waitingForPaid = true;
		}
		else {
			Debug.Log ("Топлива в цистерне недостаточно для заправки");
			CarOut ();
		}
	}

	//Ожидание оплаты
	void WaitForPay(){
		if (payed) {
			Debug.Log ("Оплачено!");
			fillInProcess = true;
		}
	}



	//Заливаем топливо
	void AddFuelAtCar(){
		float valuePerSec = speed * Time.deltaTime;
		if (fillInProcess) {
			Debug.Log ("Заливаем, осталось: " + needToRefuel);

			if (needToRefuel >= 1) {
				currentCistern.ReduceFuel (valuePerSec);
				connectedCar.carCistern.AddFuel (valuePerSec);
				needToRefuel -= valuePerSec;
			} else {
				needToRefuel = 0;
				refillComplete = true;
				fillInProcess = false;
				connectedCar.needToRefuel = 0;
				fuelReserved -= needToRefuel;
				Debug.Log ("Залито");
			}
		}
	}


	//Машина уезжает
	void CarOut(){
		Debug.Log ("Car out");		
	}


		

}
