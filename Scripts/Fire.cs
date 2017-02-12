using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public GameObject firePrefab;
	public bool isBurn = false;
	private GameObject firePrefabClone = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (isBurn) {
			MakeFirePrefab();
		} else {
			DestroyFirePrefab();
		}
	}

	void MakeFirePrefab(){
		if (!firePrefabClone) {
			firePrefabClone = Instantiate (firePrefab, transform.position + Vector3.up * (transform.localScale.y),Quaternion.identity) as GameObject;
			firePrefabClone.transform.SetParent (gameObject.transform);
		}
	}

	void DestroyFirePrefab(){
		if (firePrefabClone) {
			Destroy (firePrefabClone);
		}
	}
}
