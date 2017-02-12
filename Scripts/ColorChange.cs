using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {

	//Вешать на префабы, которым требуется проверять вероятность пожара

	public float fireChance = 0;
	public static bool checkFireChance = false;

	public Color fireColor;
	private Texture blankTexture = null;
	private Texture defaultTexture;
	private Color defaultColor;
	private Renderer rend;

	private Fire fire;

	void Start () {
		fire = GetComponent<Fire>();
		rend = GetComponent<Renderer>();
		defaultTexture = rend.material.mainTexture;
		defaultColor = rend.material.color;
		InvokeRepeating ("CalcFireSourcesSum", 1, 1);
	}

	void Update () {
		if (checkFireChance) {
			SetMaterialColor (fireColor);
		} else {
			SetDefaultColor ();
		}
		SetFireStatus ();
	}

	void SetMaterialColor(Color newColor){
		Color matColor = Color.Lerp (Color.white, newColor, fireChance);
		rend.material.SetColor ("_Color", matColor);
		rend.material.SetTexture ("_MainTex", blankTexture);
	}

	void SetDefaultColor(){
		rend.material.SetColor ("_Color", defaultColor);
		rend.material.SetTexture ("_MainTex", defaultTexture);
	}

	void CalcFireSourcesSum(){
		
		float fireSum = 0;
		var fireSourcesList = FindObjectsOfType<FireSource>();

		foreach (FireSource fireSource in fireSourcesList) {
			fireSum += fireSource.CalcMidFire(Vector3.Distance(fireSource.transform.position, gameObject.transform.position));
		}
		if (fireSum >1) {
			fireSum = 1;
		}else if (fireSum <0) {
			fireSum = 0;
		}

		fireChance = fireSum;
	}

	public void SetFireStatus(){
		if (fireChance > 0.8f) {
			fire.isBurn = true;
		}

	}
		






















}
