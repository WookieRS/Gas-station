using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public CashBox[] cashBoxArray;
	public int cashBoxesCount;
	public int openedCashBoxes;




	void Awake(){
		CalcCashboxes();
	}

	void Start(){
		//Debug.Log(UnityEngine.Profiler.maxNumberOfSamplesPerFrame);
	}


	public void CheckFireRisk(){
		ColorChange.checkFireChance = !ColorChange.checkFireChance;
	}


	//Поиск всех касс, вызывается когда кассир подходит/отходит к кассе и при установке новой кассы
	public void CalcCashboxes(){
		cashBoxArray = GameObject.FindObjectsOfType<CashBox>();

		cashBoxesCount = cashBoxArray.Length;
		openedCashBoxes = 0;

		foreach (var item in cashBoxArray) {
			if (item.ready) {
				openedCashBoxes++;
			}	
		}

		//Debug.Log("calculate cashboxes, lenght is " + cashBoxesCount + "opened " + openedCashBoxes);
	}












}

	

	

