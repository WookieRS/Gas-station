using UnityEngine;
using System.Collections;

public class Fireman : MonoBehaviour {


	private Alarms alarms;
	private Inventory firemanInventory;
	private AICharacterControl aiCharacterControl;
	private FireBox fireBox;

	public bool haveExtinguisher;
	public bool extinguishingInProcess;
	private GameObject extinguisherClone;

	void Start () {
		alarms = GameObject.FindObjectOfType<Alarms>().GetComponent<Alarms>();
		firemanInventory = GetComponent<Inventory>();
		aiCharacterControl = GetComponent<AICharacterControl>();
		CheckExtinguisher();

	}
	

	void Update () {

		if (alarms.fireDetected && !haveExtinguisher) {
			CheckExtinguisher();														//TODO проверять не каждый кадр, а по необходимости
			GoToFireBox();
		}
		if (alarms.fireDetected && haveExtinguisher) {
			CheckExtinguisher();
			if (!extinguishingInProcess) {
				GoToNearestFire ();
			} else {
				UseExtinguisher ();
			}
		}
		if (!alarms.fireDetected) {
			aiCharacterControl.target = null;										
			aiCharacterControl.agent.ResetPath();
			enabled = false;														//Компонент отключается если нет пожарной тревоги
		}
	}

	void CheckExtinguisher(){														//Проверяем, имеет ли огнетушитель с собой
		GameObject[] array = firemanInventory.inventoryArray;
		foreach (var item in array) {
			if (item != null && item.gameObject.name == "Extinguisher") {
				haveExtinguisher = true;
				return;
			} else 	haveExtinguisher = false;
		}
	}

	void GoToFireBox(){																//Побежать за новым
		aiCharacterControl.agent.speed = 1;
		GameObject fireBox = GameObject.FindObjectOfType<FireBox>().gameObject;
		aiCharacterControl.target = fireBox.transform.FindChild ("target").transform;
	}

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.name == "FireBox" && alarms.fireDetected && !haveExtinguisher) {
			fireBox = collider.gameObject.GetComponent<FireBox>();
			TakeExtinguisher ();
	
		}
	}

	void TakeExtinguisher(){
		firemanInventory.AddItem (fireBox.TakeExtinguisher ());
	}

	void GoToNearestFire(){
		if (!GameObject.FindGameObjectWithTag("Fire")) {
			return;
		}
		GameObject fire = GameObject.FindGameObjectWithTag ("Fire");
		if (fire) {
			aiCharacterControl.target = fire.transform;			
			float remainingDistance = Vector3.Distance (transform.position, fire.transform.position);

			if (remainingDistance <= 2 && aiCharacterControl.target.tag == "Fire") {
				aiCharacterControl.agent.updateRotation = true;
				aiCharacterControl.target = null;
				aiCharacterControl.agent.ResetPath();
				transform.LookAt (new Vector3(fire.transform.position.x, 0, fire.transform.position.z));
				extinguishingInProcess = true;
			}
		} else {
			Debug.Log("Уже потушен");
		}
	}

	void UseExtinguisher(){
		if (!extinguisherClone) {
			GameObject extinguisher = firemanInventory.GetItemByComponentName ("Extinguisher");
			extinguisherClone = Instantiate (extinguisher) as GameObject;
			extinguisherClone.transform.parent = transform;
			extinguisherClone.transform.localPosition = new Vector3 (0.137f, 0.641f, 0.138f);
			extinguisherClone.transform.localEulerAngles = new Vector3(-90,-90,0);
			StartCoroutine (ExtinguishingFinishInSec (extinguisher, 3f)); 				//TODO запуск анимации использования огнетушителя
		}
	}

	void ExtingushingFinished(GameObject extinguisher){
		extinguishingInProcess = false;
		firemanInventory.RemoveItem (extinguisher);
		haveExtinguisher = false;
		Destroy (extinguisherClone);
		GameObject fire = GameObject.FindGameObjectWithTag ("Fire");
		fire.transform.parent.GetComponent<Fire>().isBurn = false;
		alarms.DetectFire ();

	}

	IEnumerator ExtinguishingFinishInSec(GameObject extinguisher, float sec){
		yield return new WaitForSeconds(sec);
		ExtingushingFinished(extinguisher);
	}







































}
