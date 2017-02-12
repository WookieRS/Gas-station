using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public 	float cameraSpeed = 100.0f; //Скорость движения камеры
	private float cameraHeight;
	public 	float sensitivity = 10f;

	private Transform cachedTransform;

	public 	float minHeight = 3f;
	public 	float maxHeight = 20f;
		
	private void Awake(){
		cameraHeight = Camera.main.transform.position.y;
		cachedTransform = transform; //кэшируем трансформ по совету чувака с хабра

	}
	
	private void Update()	{


		float smoothCamSpeed = cameraSpeed * Time.smoothDeltaTime; //множим скорость перемещения камеры на сглаженную версию Time.deltaTime

		
		//При нажатии какой-либо из кнопки из WASD происходит перемещение в соответствующую сторону, нажания сразу двух кнопок также обрабатываются (WA будет двигать камеру вверх и влево), зажатие Shift при этом ускоряет передвижение.
		if (Input.GetKey(KeyCode.W)) {cachedTransform.position += new Vector3(0.0f, 0.0f, smoothCamSpeed);} //вверх
		if (Input.GetKey(KeyCode.A)) {cachedTransform.position += new Vector3(-smoothCamSpeed, 0.0f, 0.0f);} //налево
		if (Input.GetKey(KeyCode.S)) {cachedTransform.position += new Vector3(0.0f, 0.0f, -smoothCamSpeed);} //вниз
		if (Input.GetKey(KeyCode.D)) {cachedTransform.position += new Vector3(smoothCamSpeed, 0.0f, 0.0f);} //направо

		if (Input.GetKey(KeyCode.KeypadMinus)) 	{cachedTransform.position += new Vector3(0, smoothCamSpeed, 0.0f);}
		if (Input.GetKey(KeyCode.KeypadPlus)) 	{cachedTransform.position += new Vector3(0, -smoothCamSpeed, 0.0f);}

		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			cameraHeight -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
			cameraHeight = Mathf.Clamp (cameraHeight, minHeight, maxHeight);
			cachedTransform.position = new Vector3(cachedTransform.position.x, cameraHeight, cachedTransform.position.z);
		}


//		Либо через fov	
//		float fov = Camera.main.fieldOfView;
//		fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
//		Debug.Log (Input.GetAxis ("Mouse ScrollWheel"));
//		fov = Mathf.Clamp(fov, minFov, maxFov);
//		Camera.main.fieldOfView = fov;

	}

}
