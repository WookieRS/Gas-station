using System.Collections;
using UnityEngine;

    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
   // [RequireComponent(typeof (ThirdPersonCharacter))]

public class AICharacterControl : MonoBehaviour{

    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public ThirdPersonCharacter character { get; private set; } // the character we are controlling
    public Transform target;                                    // target to aim for
	public Transform defaultTarget;

	public bool updateRotation;
	public bool manualControl = false;
	public bool isWait;
	public bool resetTimer;

	private Vector3 targetAngle;
	private Vector3 currentAngle;

	private Alarms alarms;
	private Fireman fireman;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();

		agent.updateRotation = updateRotation;
        agent.updatePosition = true;

		alarms = GameObject.FindObjectOfType<Alarms>().GetComponent<Alarms>();
		fireman = GetComponent<Fireman>();

		//avoidance priority set random 0 - 99
		agent.avoidancePriority = Random.Range(0,99);
    }


    private void Update() {
		if (target != null)
            agent.SetDestination(target.position);

		if (alarms.fireDetected) {											//Включается компонент пожарника если есть тревога
			fireman.enabled = true;
		} 

		if (manualControl) {
			if (Input.GetKey(KeyCode.Mouse0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				
				if (Physics.Raycast (ray,out hit, 100)) {
					agent.SetDestination (hit.point);
				}
			}
		}


        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);
        else
            character.Move(Vector3.zero, false, false);
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }

//	//Мгновенный поворот
//	private void LookAtTarget(Transform target){
//		//transform.LookAt (new Vector3(target.transform.position.x, 0, target.transform.position.z));
//	}
	
	//Плавный поворот - вроде работает
	public bool RotateToTarget(Transform target, float speed, float angle){
		targetAngle = target.transform.eulerAngles - new Vector3(0, angle, 0);
		currentAngle = transform.eulerAngles;
		
		currentAngle = new Vector3(
			0,
			Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * speed),
			0);
		
		transform.eulerAngles = currentAngle;
		
		float diffAngle = transform.eulerAngles.y - target.transform.eulerAngles.y + angle;
		diffAngle =  Mathf.Abs(diffAngle);
		diffAngle = diffAngle % 360;


		if (diffAngle <= 0.1f || diffAngle >= 359.9f) {						//Если поворот завершился
			 return true;
		}
		return false;
	}

	//Шаг вперед
	public void StepForward(float distance = 1f){
		defaultTarget.transform.position = transform.position;
		defaultTarget.Translate(Vector3.forward * distance);
		target = defaultTarget;
		defaultTarget.transform.parent = null;
	}

	void OnDestroy(){
		Destroy(defaultTarget.gameObject);
	}

	//Отойти на рандомное расстояние от текущей позиции
	public void MoveRandomPos(float radius = 3){
		float min = 1f;
		float max = radius;
		float randomSignX = Random.Range(0,2)*2-1;
		float randomSignZ = Random.Range(0,2)*2-1;
		float randomX = Random.Range(min,max) * randomSignX;
		float randomZ = Random.Range(min,max) * randomSignZ;


		Vector3 newPos = transform.position + new Vector3(randomX, 0 , randomZ);
		agent.SetDestination(newPos);
	}

	//Встать на расстоянии от цели
	public void StayAtTargetRadius(Transform target, float radius = 4){
		float min = 1f;
		float max = radius;
		float randomSignX = Random.Range(0,2)*2-1;
		float randomSignZ = Random.Range(0,2)*2-1;
		float randomX = Random.Range(min,max) * randomSignX;
		float randomZ = Random.Range(min,max) * randomSignZ;

		Vector3 newPos = target.transform.position + new Vector3(randomX, 0 , randomZ);
		agent.SetDestination(newPos);

	}

	//StartCoroutine(waiter(min, max));

	public bool WaitRandomSec(float min = 1f, float max = 5f){
		if (resetTimer) {
			StartCoroutine(Waiter(min, max));
		}
		return isWait;
	}

	IEnumerator Waiter(float min, float max){
		isWait = true;
		float waitTime = Random.Range(min,max);
		yield return new WaitForSeconds(waitTime);
		isWait = false;
		resetTimer = false;
	}

	



}
