using UnityEngine;
using System.Collections;

public class CashBox : MonoBehaviour {

	private GameController gameController;

	public bool haveCashier;
	public bool ready = false;

	public Buyer[] queueArray;
	public int buyersInQueue;
	public bool fullQueue;
	public Transform freeTargetInQueue;

	public Light statusLight;

	// Use this for initialization
	void Start () {
		CalculateQueue();
	}
	
	// Update is called once per frame
	void Update () {
		statusLight.enabled = ready;
	}

	//Пересчет очереди
	public void CalculateQueue(){			
		if (buyersInQueue == queueArray.Length) {
			fullQueue = true;
		} else fullQueue = false;
		if (buyersInQueue == 0) {
			freeTargetInQueue.transform.localPosition = new Vector3(-1f,0); 
		} else {
			freeTargetInQueue.transform.localPosition = new Vector3(-1f-0.5f*buyersInQueue,0);
		}
		if (buyersInQueue < 0) {
			buyersInQueue = 0;
		}
	}

	//Добавить покупателя в конец очереди
	public void AddBuyerToQueue(Buyer buyer){
		queueArray[buyersInQueue] = buyer;
		buyer.positionInQueue = buyersInQueue;
		buyersInQueue++;
	}

	//Сдвинуть очередь вперед
	//TODO Должны не синхронно шагать, а с разными промежутками
	public void MoveQueue(){
		queueArray[0] = null;
		buyersInQueue--;

		if (buyersInQueue > 0) {
			foreach (var item in queueArray) {
				if (item && item.positionInQueue > 0) {
					
					GameObject buyerObject = item.gameObject;
					AICharacterControl buyerAI = buyerObject.GetComponent<AICharacterControl>();
					queueArray[item.positionInQueue-1] = item;
					queueArray [item.positionInQueue] = null;
					StartCoroutine(MoveBuyerOnce(buyerAI, 0.2f));		//Шагнет через время. Костыль через корутину. Не понятно работает ли вообще
					item.positionInQueue--;
				}
			}
		}

		CalculateQueue();
	}

	IEnumerator MoveBuyerOnce(AICharacterControl buyerAI, float delayTime){
		yield return new WaitForSeconds(delayTime);
		buyerAI.agent.stoppingDistance = 0.1f;
		buyerAI.agent.speed = 0.2f;
		buyerAI.StepForward(1f);
	}


	//Очистка очереди, наример если кассир отошел
	public void ClearQueue(){
		Debug.Log("clearQueue!");
		buyersInQueue = 0;
		int i = 0;
		foreach (var item in queueArray) {
			if (item) {
				item.ResetCashBox();
			}
			queueArray[i] = null;
			i++;
		}
	}
		





		

		
}
