using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PrefabMaker : MonoBehaviour {

	public GameObject[] prefabs;			//Массив префабов
	public int selectedPrefab;				//выбранный в данный момент
	public Material ghostMaterial;			//Материал призрака
	public Color goodColor = Color.green;	
	public Color badColor = Color.red;

	public LayerMask raycastLayers = 1;		//с каким слоем проверяется пересечение
	private RaycastHit hit;					//Информация о попадании рейкаста

	private bool settingMode = false;	//Режим размещения префаба
	private bool crossingWithObj = false;	//Призрак пересекается с чем то

	private GameObject ghostClone;			//созданный клон
	private Vector3 rotate;					//Угол на который бы повернут призрак последний раз

	void Start(){

	}

	void Update(){
		
	  	if (settingMode && ghostClone) { 									//Если режим установки и был создан клон
			PrefabSettingMode ();					
		}
	}

	void PrefabSettingMode (){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);					//Пускаем луч по позиции мыши
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, raycastLayers)) {			//Проверка на пересечение луча с объектами нужного слоя

			if (EventSystem.current.IsPointerOverGameObject()){							//Отключение клона при наведении на UI
				ghostClone.SetActive(false);
				return;
			} else {
				ghostClone.SetActive (true);
			}

			float hitPointX = hit.point.x;												//Берем координаты хитпойнта..
			float hitPointY = ghostClone.transform.localScale.y / 2;
			float hitPointZ = hit.point.z;

			hitPointX = Mathf.Round (hitPointX);										//и округляем. Если надо, можно изменить коэффциент округления
			hitPointZ = Mathf.Round (hitPointZ);										//По высоте не обязательно

			ghostClone.transform.position = new Vector3 (hitPointX, hitPointY, hitPointZ);		//Задаем позицию по округленным координатам


			crossingWithObj = ghostClone.GetComponent<CollisionDetector> ().collisionDetected;	//Узнаем у призрака не пересекается ли он с кем-то
			checkCrossingWithObj ();

			if (Input.GetKeyDown (KeyCode.Mouse1)) {											//По правому клику поворот на 90 гр.
				rotate += new Vector3 (0, 90, 0);
				ghostClone.transform.Rotate (new Vector3 (0, 90, 0));
			}
			if (Input.GetKeyDown (KeyCode.Mouse0) && !crossingWithObj) {						//По левому переключаем режим и создаем новый клон
				settingMode = false;											
				GameObject kolClone = Instantiate (prefabs [selectedPrefab], ghostClone.transform.position, Quaternion.identity) as GameObject;
				kolClone.transform.Rotate (rotate);
				Destroy (ghostClone);
				CreateGhostClone ();
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				settingMode = false;
				Destroy (ghostClone);
			}
		}
	}

	void CreateGhostClone (){
		//создание призрака, изменение его компонентов 
 		//TODO: поправить костыль с созданием в левой точке
		//TODO: менять свойство на старте, размещать в массив клонов.
		ghostClone = Instantiate (prefabs[selectedPrefab], new Vector3(0, -100, 0), Quaternion.identity) as GameObject;
		ghostClone.name = "ghost";
		ghostClone.GetComponent<Renderer>().material = ghostMaterial;
		ghostClone.transform.localScale *= 1.01f;

		if (ghostClone.GetComponent<ColorChange>()) {
			ghostClone.GetComponent<ColorChange>().enabled = false;
		}
		if (ghostClone.GetComponent<FireSource>()) {
			ghostClone.GetComponent<FireSource>().enabled = false;
		}

		ghostClone.AddComponent<CollisionDetector>();
		ghostClone.AddComponent<Rigidbody>();
		ghostClone.GetComponent<Rigidbody>().isKinematic = true;
		BoxCollider ghostBox = ghostClone.GetComponent<BoxCollider>();
		ghostBox.isTrigger = false;
		ghostBox.size *= 0.9f;

		ghostClone.transform.Rotate(rotate);
		settingMode = true;
	}

	void checkCrossingWithObj (){
		if (crossingWithObj) {
			ghostClone.GetComponent<Renderer> ().material.SetColor ("_Color", badColor);
		}
		else {
			ghostClone.GetComponent<Renderer> ().material.SetColor ("_Color", goodColor);
		}
	}

	public void ChoosePrefab(int number){
		if (ghostClone) {
			Destroy (ghostClone);
		}
		selectedPrefab = number - 1;
		CreateGhostClone ();
	}






}



//Ray scrRay = Camera.main.ScreenPointToRay(Input.mousePosition)     - создаём луч, бьющий от координат мыши по координатам в игре
//(Physics.Raycast(scrRay, out hit, Mathf.Infinity, raycastLayers)) // бьём этим лучем в заданном выше направлении (т.е. в землю)
//Quaternion normana = Quaternion.FromToRotation(Vector3.up, hit.normal); //получаем нормаль от столкновения
//ghost.transform.position = hit.point; //задаём позицию призрака равной позиции точки удара луча по земле
//ghost.transform.rotation = normana; //тоже самое и с вращением, только не от точки, а от нормали
//GameObject tower = Instantiate(plasmaTower, ghost.transform.position, ghost.transform.rotation) as GameObject;
//Destroy(ghost); //уничтожаем призрак башни


//float squaredDistance = (Target.transform.position - mob.position).sqrMagnitude; //меряем дистанцию до цели по совету чуваков с хабра





