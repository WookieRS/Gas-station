using UnityEngine;
using System.Collections;

public class Cashier : MonoBehaviour {

	private GameController gameController;
	private AICharacterControl aiCharacterControl;
	private GameObject cashBoxObject;
	private CashBox minimalDistanceCashBox;
	private CashBox cashBox;
	private Needs needs;

	private float rotationSpeed = 3;

	public bool wantToWork;
	public bool stayingAtCashBox;
	public bool readyToSell;





	void Start () {
		aiCharacterControl = GetComponent<AICharacterControl>();
		gameController = FindObjectOfType<GameController>();
		needs = GetComponent<Needs>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (needs.allNeedsOK) {
			wantToWork = true;
			FindWorkPlace ();
		} else if(cashBox && wantToWork) {
			wantToWork = false;
			ResetWorkPlace ();
		}

	}


	void FindWorkPlace (){
		if (cashBoxObject && !readyToSell) {
			if (!stayingAtCashBox) {
				GoToCashBox ();
			}
			else
				if (!aiCharacterControl.agent.hasPath) {
					ClearTarget ();
					readyToSell = aiCharacterControl.RotateToTarget (cashBoxObject.transform, rotationSpeed, 90f);			//Поворот к кассе. костыль
					cashBox.ready = readyToSell;
					if (readyToSell) {
						gameController.CalcCashboxes();
					}

				}
		} else if(!readyToSell){
			FindFreeCashBox();
		}
	}


	//Ищет ближайшую кассу без кассира
	void FindFreeCashBox(){
		//Debug.Log(gameObject.name + " ищет свободную кассу");
		CashBox[] cashBoxArray = gameController.cashBoxArray;

		float minimalDistance = 1000;


		foreach (var box in cashBoxArray) {
			if (!box.haveCashier) {
				float distance = Vector3.Distance(box.transform.position, transform.position);

				if (distance < minimalDistance) {
					minimalDistanceCashBox = box;
					minimalDistance = distance;
				}

			} else if(cashBox != box) {
				cashBoxObject = null;
				cashBox = null;
			}
		}

		if (cashBoxArray.Length > 0) {
			cashBox = minimalDistanceCashBox;
			cashBoxObject = minimalDistanceCashBox.gameObject;
		}

		if (!cashBox) {
			//Если не нашел ни одной свободной кассы
		}
	}

	//Пройти за кассу
	void GoToCashBox(){
		aiCharacterControl.agent.speed = 0.5f;
		aiCharacterControl.target = cashBoxObject.transform.FindChild ("cashierTarget").transform;
		
		float remainingDistance = Vector3.Distance (transform.position, aiCharacterControl.target.transform.position);																								
		if (remainingDistance <= 8f && cashBox.haveCashier) {														//проверить не появился ли кто на кассе
			//остановиться и найти другую кассу
			Debug.Log("На кассе уже ктото есть!");
			aiCharacterControl.target = null;
			aiCharacterControl.agent.ResetPath();
			FindFreeCashBox();

		}

		if (remainingDistance <= 0.3f && aiCharacterControl.target.name == "cashierTarget"){
			stayingAtCashBox = true;
			cashBox.haveCashier = true;
		}
	}



	void ClearTarget(){
		aiCharacterControl.target = null;
	}
	
	void DisableNavMesh(){
		aiCharacterControl.agent.enabled = false;
	}

	public void ResetWorkPlace(){
		Debug.Log("reset workplace on " + gameObject.name);
		if (cashBoxObject) {
			
			cashBox.haveCashier = false;
			cashBox.ready = false;
			stayingAtCashBox = false;
			readyToSell = false;
			cashBox.ClearQueue();
			cashBox.CalculateQueue();
			cashBox = null;
			cashBoxObject = null;
			minimalDistanceCashBox = null;
		}
		gameController.CalcCashboxes();
	}




}
