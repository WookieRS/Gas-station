using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public float shootDistance = 10f;
	public float shootRate = .5f;
	//public ShootingScript shootingScript;

	private Animator anim;
	private UnityEngine.AI.NavMeshAgent navMeshAgent;
	private Transform targetedEnemy;
	private Ray shootRay;
	private RaycastHit shootHit;
	private bool walking = false;
	private bool enemyClicked;
	private float nextFire;
	public 	LayerMask shootableMask = 1;


	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator>();
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Input.GetButtonDown ("Fire2")) {
			if (Physics.Raycast (ray, out hit, 100)) {
				if (hit.collider.CompareTag ("Guard")) {
					targetedEnemy = hit.transform;
					enemyClicked = true;
				}
				else {
					walking = true;
					enemyClicked = false;
					navMeshAgent.destination = hit.point;
					navMeshAgent.Resume ();
				}

			}
		}
		if (enemyClicked) {
			MoveAndShoot();
		}
		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
			if (!navMeshAgent.hasPath || Mathf.Abs (navMeshAgent.velocity.sqrMagnitude) < float.Epsilon) 
			walking = false;
		}
		else {
			walking = true;
		}

		//anim.SetBool ("IsWalking", walking);											//Запуск анимации ходьбы
	}

	private void MoveAndShoot(){
		if (targetedEnemy == null)
			return;
			navMeshAgent.destination = targetedEnemy.position;
			if (navMeshAgent.remainingDistance >= shootDistance) {
				navMeshAgent.Resume ();
				walking = true;
			}

			if (navMeshAgent.remainingDistance < shootDistance) {
			transform.LookAt (targetedEnemy.transform.FindChild ("target"));			//Смотрит вниз почему-то
		
			Debug.Log(targetedEnemy.gameObject);
			Vector3 dirToShoot = targetedEnemy.transform.position - transform.position;
				if (Time.time > nextFire) {
					nextFire = Time.time + shootRate;
					//Shoot(dirToShoot); 									//отключил стрельбу
				}
				navMeshAgent.Stop ();
				walking = false;
			}
	}





	void Shoot (Vector3 shootDir){
		//Не работает, глянуть метод Shoot в ассете
		shootRay.origin = transform.position;
		shootRay.direction = shootDir;

		if (Physics.Raycast (shootRay, out shootHit, 100, shootableMask)) {
			Debug.Log ("Попал");
		}
	}























}
