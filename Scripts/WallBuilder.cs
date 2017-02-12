using UnityEngine;
using System.Collections;

public class WallBuilder : MonoBehaviour {

	public GameObject stick;
	public LayerMask raycastLayers = 1;		//с каким слоем проверяется пересечение
	private RaycastHit hit;					//Информация о попадании рейкаста
	public bool buildingMode = true;	//Режим размещения стен
	//private bool crossingWithObj = false;	//Стена пересекается с чем то



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (buildingMode) { 									//Если режим установки и был создан клон
			WallBuildingMode ();					
		}
	}

	void WallBuildingMode (){

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);					//Пускаем луч по позиции мыши
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, raycastLayers)) {			//Проверка на пересечение луча с объектами нужного слоя

			if (stick.activeInHierarchy) {
				float hitPointX = hit.point.x;												//Берем координаты хитпойнта..
				float hitPointY = stick.transform.localScale.y / 2;
				float hitPointZ = hit.point.z;
				
				hitPointX = Mathf.Round (hitPointX);										//и округляем. Если надо, можно изменить коэффциент округления
				hitPointZ = Mathf.Round (hitPointZ);										//По высоте не обязательно
				
				stick.transform.position = new Vector3 (hitPointX, hitPointY, hitPointZ);		//Задаем позицию по округленным координатам

			} else {
				MakeStick(hit.transform.position);
			}


		}


	}

	void MakeStick(Vector3 position){

		stick = Instantiate (stick, position, Quaternion.identity) as GameObject;
	}



















}
