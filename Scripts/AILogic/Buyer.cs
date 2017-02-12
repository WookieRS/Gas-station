using UnityEngine;
using System.Collections;

public class Buyer : MonoBehaviour {

	private AICharacterControl aiCharacterControl;
	private GameController gameController;
	private GameObject cashBoxObject;
	private CashBox cashBox;
	private CashBox minimalDistanceCashBox;
	private Needs needs;

	private float randomDistance;
	private float randomTimer;
	private float randomWalkSpeed;
	private bool moveQueue = false;

	public Car myCar;
	public float money;
	public bool readyToBuy;
	public bool inQueue;
	public int positionInQueue;
	public bool playBuyAnimation;
	public bool goingToCar;
	public bool setInCarAnimation;

	void Start () {
		aiCharacterControl = GetComponent<AICharacterControl>();
		gameController = FindObjectOfType<GameController>();
		needs = GetComponent<Needs>();
		randomDistance = Random.Range(2f, 5f);
		randomWalkSpeed = Random.Range(0.4f, 0.65f);
		ResetTimer();
	}

	void Update () {

		if (Input.GetKeyDown(KeyCode.R)) {
			aiCharacterControl.MoveRandomPos();
		}

		if (needs.allNeedsOK && !goingToCar) {
			readyToBuy = true;
			ReadyToBuy ();
		}



		if (goingToCar) {
			GoToCar();
		}
	}

	//Готов совершить покупку
	void ReadyToBuy ()	{
		
		if (!cashBox && gameController.openedCashBoxes >= 1) {
			FindWorkingCashBox ();
		}
		if (!inQueue && cashBox) {
			if (randomTimer < 0) {
				GoToQueue();
			} else {
				randomTimer -= Time.deltaTime;
			}





		}
		if (inQueue) {
			if (aiCharacterControl.RotateToTarget (cashBox.transform, 3, -90f)) {
				if (positionInQueue == 0) {
					//Debug.Log(gameObject.name + "- zero position");
					playBuyAnimation = true;
					Invoke ("GoToCar", 4);
				}
			}
		}
	}

	//Встает в ближайшую работающую кассу без очереди
	//TODO Если очередь полная, то встать где-то рядом с последним и ждать
	//TODO Занимать очередь в той, где меньше народа или ближайшей
	void FindWorkingCashBox(){
		CashBox[] cashBoxArray = gameController.cashBoxArray;

		float minimalDistance = 1000;

		foreach (var box in cashBoxArray) {
			if (box.ready && !box.fullQueue) {
				float distance = Vector3.Distance(box.transform.position, transform.position);

				if (distance < minimalDistance) {
					minimalDistanceCashBox = box;
					minimalDistance = distance;
				}
					
			} else {
				//Если не нашел ни одной работающей кассы
				cashBoxObject = null;
				cashBox = null;
			}
		}
		if (cashBoxArray.Length > 0) {
			cashBox = minimalDistanceCashBox;
			cashBoxObject = minimalDistanceCashBox.gameObject;
		}
	}
		

	public void GoToQueue(){
		aiCharacterControl.agent.speed = randomWalkSpeed;
		aiCharacterControl.agent.stoppingDistance = 0.3f;
		if (!cashBox.fullQueue && cashBox.ready) {
			aiCharacterControl.target = cashBox.freeTargetInQueue.transform;
	

			float remainingDistance = Vector3.Distance (transform.position, aiCharacterControl.target.transform.position);
			if (remainingDistance <= 8f) {
				cashBox.CalculateQueue();
			}
			if (remainingDistance <= 0.5f){
				inQueue = true;
				cashBox.AddBuyerToQueue(this);
				aiCharacterControl.target = null;
			}
			//очередь заняли пока шел
		} else {
			aiCharacterControl.agent.stoppingDistance = randomDistance;
			aiCharacterControl.target = null;
			aiCharacterControl.agent.ResetPath();
			FindWorkingCashBox();
			ResetTimer();
		}
	}

	void GoToCar(){

		if (!goingToCar) {
			readyToBuy 					= false;
			inQueue						= false;
			playBuyAnimation 			= false;
			if (!moveQueue) {
				moveQueue = true;
				cashBox.MoveQueue();
			}

			goingToCar = true;
		} else {
			//при выходе из очереди не должен толкать остальных, поэтому снижаем радиус агента и приоритет
			aiCharacterControl.agent.speed = 0.5f;
			aiCharacterControl.agent.radius = 0.01f;
			aiCharacterControl.agent.avoidancePriority = 99;
			aiCharacterControl.target = myCar.driver;
			float remainingDistance = Vector3.Distance (transform.position, aiCharacterControl.target.transform.position);
			if (remainingDistance <= 0.4f){
				Destroy(gameObject, 1);
			}
		}
	}



	void ResetTimer(){
		if (randomTimer <= 0) {
			randomTimer = Random.Range(1f, 8f);
		}

	}

	public void ResetCashBox(){
		cashBox = null;
		cashBoxObject = null;
		inQueue = false;
		positionInQueue = 0;
		minimalDistanceCashBox = null;
	}


		
		
}
