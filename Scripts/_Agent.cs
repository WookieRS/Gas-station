using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {

	public GameObject target;
	UnityEngine.AI.NavMeshAgent agent;


	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	

	void Update () {
		//agent.SetDestination (target.transform.position);
		if (Input.GetKey(KeyCode.Mouse0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray,out hit, 100)) {
				agent.SetDestination (hit.point);
			}
		}
	}
}
