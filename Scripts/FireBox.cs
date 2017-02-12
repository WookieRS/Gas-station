using UnityEngine;
using System.Collections;

public class FireBox : MonoBehaviour {

	public GameObject extinguisher;
	public int extinguisherCapacity = 5;
	public int extinguisherCount = 5;
	bool triggerIsActive;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject TakeExtinguisher(){
		if (extinguisherCount >= 1) {
			extinguisherCount--;
			return extinguisher;
		} else{ 
			Debug.LogError ("Огнетушители кончились!");
			return null;
		}
	}

	int PullExtinguisher(int amount){
		int amountAdd = extinguisherCapacity - extinguisherCount;
		extinguisherCount += amountAdd;
		return amount - amountAdd;
	}

	//TODO проверка если коллайдер пожарный (класс, который пользуется огнетушителем
	void OnTriggerEnter (Collider collider){  

	}

	//TODO метод проверки нужен ли ему огнетушитель (isNeedExtinguisher)
	//TODO если нужен: запуск анимации взятия, начисление огнетушителя в инвентарь пожарного, уменьшение из бокса
}
