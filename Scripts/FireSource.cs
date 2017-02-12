using UnityEngine;
using System.Collections;

public class FireSource : MonoBehaviour {

	/// <summary>
	/// Пока в радиусе есть объекты, для каждого генерирует силу воздействия (midfire) в зависимости от расстояния до объекта
	/// </summary>

	[Range(0,1)]
	public float power;
	private float radius;

	//TODO дописать метод горения. различная вероятность самовозгорание, либо от какого то объекта (максимальное увеличение огнеопасности)
	//Если пожар, то статус у всех сотрудников пожар, в зависимости от должности - разное поведение
	//пожарник: ищет огнетушитель, тушит
	//кассир: паникует/вызывает пожарных
	//охранник: вызывает пожарных
	//???



	void Start(){
		//Gizmos.DrawSphere (gameObject.transform.position, radius)
		radius = GetComponent<SphereCollider>().radius;
	}

	public float CalcMidFire(float distance){

		float midfire = ((radius - distance)*power)/radius;
		if (midfire < 0) {
			midfire = 0;
		}
		return midfire;
	}
}


