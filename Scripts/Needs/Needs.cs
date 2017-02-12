using UnityEngine;
using System.Collections;

	
public class Needs : MonoBehaviour {

	private Buyer buyer;
	private AICharacterControl ai;
	private Cashier cashier;

	//private float goodK = 0.9f;
	private float lowK = 0.2f; //можно сделать рандомным в зависимости от опыта
	//private float criticalK = 0.1f; - при опускании до критического - бежит в туалет

	private bool isBuyer;
	public bool isBusy = false;
	public bool allNeedsOK = true;

	[Range(0,1)]
	public float food;
	[Range(0,1)]
	public float drink;
	[Range(0,1)]
	public float comfort;
	[Range(0,1)]
	public float wc;
	public Component lookingFor;

	private float randomNeedsModifer;

	private Food[] foodArray;
	private Food minimalDistanceFood;
	private int foodCount;
	private int freeFoodCount;
	public bool cantFindFood;

	private Drink[] drinksArray;
	private Drink minimalDistanceDrinks;
	private int drinksCount;
	private int freeDrinksCount;
	public bool cantFindDrinks;

	private WC[] wcArray;
	private WC minimalDistanceWC;
	private int wcCount;
	private int freeWCCount;
	public bool cantFindWC;

	private Comfort[] comfortArray;
	private Comfort minimalDistanceComfort;
	private int comfortCount;
	private int freeComfortCount;
	public bool cantFindComfort;


	void Start () {
		Initialization();

		if (GetComponent<Buyer>()) {
			isBuyer = true;
			buyer = GetComponent<Buyer>();
			ai = GetComponent<AICharacterControl>();
		} else {
			isBuyer = false;
			if (gameObject.tag == "Cashier") {
				cashier = GetComponent<Cashier>();
				ai = GetComponent<AICharacterControl>();
			}
		}
	}


	void Update () {
		if (!isBusy){
			CheckNeeds();
		}

		if (!allNeedsOK) {
			FindNeeds();
		}
	}

	//проверка всех нужнд. У покупаетеля не проверяется комфорт
	public void CheckNeeds(){
		ai.agent.speed = 0.5f;

		if (isBuyer) {
			if (food > lowK && drink > lowK && wc > lowK) {
				allNeedsOK = true;

			} else {
				allNeedsOK = false;
				isBusy = true;
			}
		}	

		else if (food > lowK && drink > lowK && wc > lowK && comfort > lowK) {
			allNeedsOK = true;

		} else if(food <= lowK || drink <= lowK || wc <= lowK || comfort <= lowK){
			allNeedsOK = false;
			isBusy = true;
			cashier.ResetWorkPlace();
		}
	}

	void FindNeeds(){
		if (lookingFor) {
			ai.target = lookingFor.transform;
			//			Поидее нужна проверка не занята ли текущая цель
			//			float remainingDistance = Vector3.Distance (transform.position, ai.target.transform.position);
			//			if (remainingDistance <= 3f) {
			//				CalcFoods();
			//			}

		} else {

			if (!isBuyer && comfort <= lowK) {								//buyers comfort don't check 
				if (comfortCount > 0) {
					CalcComfort();
					lookingFor = minimalDistanceComfort;
				} else {
					cantFindComfort = true;
					//NO FOOD ATTENTION!
				}
			} 
			if (food <= lowK) {
				if (foodCount > 0) {
					CalcFoods();
					lookingFor = minimalDistanceFood;
				} else {
					cantFindFood = true;
					//NO FOOD ATTENTION!
				}
			} 
			if (drink <= lowK) {
				if (drinksCount > 0) {
					CalcDrinks();
					lookingFor = minimalDistanceDrinks;
				} else {
					cantFindDrinks = true;
					//NO DRINKS ATTENTION!
				}
			}
			if (wc <= lowK) {
				if (wcCount > 0) {
					CalcWC();
					lookingFor = minimalDistanceWC;
				} else {
					cantFindWC = true;
					//NO WC ATTENTION!
				}
			}
		}
	}


	//TODO работает с едой, сделать аналогично с остальными нуждами или попробовать объединить в один метод
	//TODO пересчитывать массив при добавлении/удалении источников еды
	public void CalcFoods(){
		foodArray = GameObject.FindObjectsOfType<Food>();
		foodCount = foodArray.Length;

		float minimalDistance = 1000;

		foreach (var item in foodArray) {
			if (!item.occupied) {
				float distance = Vector3.Distance(item.transform.position, transform.position);

				if (distance < minimalDistance && item.userQueue == null) {
					cantFindFood = false;
					minimalDistanceFood = item;
					minimalDistance = distance;
					item.userQueue = gameObject;
				}
			} else if (item.userQueue == gameObject) {
				return;
			} else {
				//все занято, или нет ни одной
				minimalDistanceFood = null;
				lookingFor = null;
				ai.target = null;
				ai.agent.ResetPath();
			}
		}
	}

	//TODO пересчитывать массив при добавлении/удалении источников еды
	public void CalcDrinks(){
		drinksArray = GameObject.FindObjectsOfType<Drink>();
		drinksCount = drinksArray.Length;

		float minimalDistance = 1000;

		foreach (var item in drinksArray) {
			if (!item.occupied) {
				float distance = Vector3.Distance(item.transform.position, transform.position);

				if (distance < minimalDistance && item.userQueue == null) {
					cantFindDrinks = false;
					minimalDistanceDrinks = item;
					minimalDistance = distance;
					item.userQueue = gameObject;
				}
			} else if (item.userQueue == gameObject) {
				return;
			} else {
				//все занято, или нет ни одной
				minimalDistanceDrinks = null;
				lookingFor = null;
				ai.target = null;
				ai.agent.ResetPath();
			}
		}
	}

	public void CalcWC(){
		wcArray = GameObject.FindObjectsOfType<WC>();
		wcCount = wcArray.Length;

		float minimalDistance = 1000;

		foreach (var item in wcArray) {
			if (!item.occupied) {
				float distance = Vector3.Distance(item.transform.position, transform.position);

				if (distance < minimalDistance && item.userQueue == null) {
					cantFindWC = false;
					minimalDistanceWC = item;
					minimalDistance = distance;
					item.userQueue = gameObject;
				}
			} else if (item.userQueue == gameObject) {
				return;
			} else {
				//все занято, или нет ни одной
				minimalDistanceWC = null;
				lookingFor = null;
				ai.target = null;
				ai.agent.ResetPath();
			}
		}
	}

	public void CalcComfort(){
		comfortArray = GameObject.FindObjectsOfType<Comfort>();
		comfortCount = comfortArray.Length;

		float minimalDistance = 1000;

		foreach (var item in comfortArray) {
			if (!item.occupied) {
				float distance = Vector3.Distance(item.transform.position, transform.position);

				if (distance < minimalDistance && item.userQueue == null) {
					cantFindComfort = false;
					minimalDistanceComfort = item;
					minimalDistance = distance;
					item.userQueue = gameObject;
				}
			} else if (item.userQueue == gameObject) {
				return;
			} else {
				//все занято, или нет ни одной
				minimalDistanceComfort = null;
				lookingFor = null;
				ai.target = null;
				ai.agent.ResetPath();
			}
		}
	}



	//Не получается объединить массивы из-за разных типов. Попробовать с ArrayList
	//	public void CalcNeeds(NeedsType type){
	//
	//
	//		//Component[] array;
	//		//ArrayList array = new ArrayList();
	//
	//		if (type == NeedsType.drinks) {
	//			needsArray = GameObject.FindObjectsOfType<Drink>();
	//			Drink[] array = needsArray as Drink[];
	//			drinksCount = needsArray.Length;
	//
	//		} else if(type == NeedsType.food) {
	//			needsArray = GameObject.FindObjectsOfType<Food>();
	//			Food[] array = needsArray as Food[];
	//			foodCount = needsArray.Length;
	//		}
	//
	//			float minimalDistance = 1000;
	//
	//			foreach (var item in array) {
	//				if (!item.occupied) {
	//					float distance = Vector3.Distance(item.transform.position, transform.position);
	//
	//					if (distance < minimalDistance && item.userQueue == null) {
	//						minimalDistanceNeeds = item;
	//						minimalDistance = distance;
	//						item.userQueue = gameObject;
	//					}
	//				} else if (item.userQueue == gameObject) {
	//					return;
	//				} else {
	//					//все занято, или нет ни одной
	//					minimalDistanceNeeds = null;
	//					lookingFor = null;
	//					ai.target = null;
	//					ai.agent.ResetPath();
	//				}
	//			}
	//
	//	}


	void Initialization(){
		foodArray = GameObject.FindObjectsOfType<Food>();
		foodCount = foodArray.Length;

		drinksArray = GameObject.FindObjectsOfType<Drink>();
		drinksCount = drinksArray.Length;

		comfortArray = GameObject.FindObjectsOfType<Comfort>();
		comfortCount = comfortArray.Length;

		wcArray = GameObject.FindObjectsOfType<WC>();
		wcCount = wcArray.Length;


		if (isBuyer) {
			food 	= Random.Range(0.01f, 1f);
			drink 	= Random.Range(0.01f, 1f);
			wc		= Random.Range(0.01f, 1f);
		} else {
			food 	= Random.Range(0.7f, 1f);
			drink 	= Random.Range(0.7f, 1f);
			wc		= Random.Range(0.8f, 1f);
			comfort	= Random.Range(0.8f, 1f);
		}

		randomNeedsModifer = Random.Range(0.001f, 0.04f);
		InvokeRepeating("UpdateNeeds", 5, 3);


	}

	void UpdateNeeds(){
		food 	-= 	randomNeedsModifer * 0.3f;
		drink 	-= 	randomNeedsModifer;
		comfort -= 	randomNeedsModifer * 0.4f;
		wc 		-= 	randomNeedsModifer * 0.1f;
	}









}

