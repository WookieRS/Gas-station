using UnityEngine;
using System.Collections;

public class Extinguisher : MonoBehaviour {


	private ParticleSystem.EmissionModule emissionModule;

	public bool extinguisherOn = false;

	// Use this for initialization
	void Start () {
		emissionModule = GetComponentInChildren<ParticleSystem>().emission;
	}
	
	// Update is called once per frame
	void Update () {
		if (extinguisherOn) {
			emissionModule.enabled = true;
		} else {
			emissionModule.enabled = false;
		}
	}
}
