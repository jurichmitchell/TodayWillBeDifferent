using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoints : MonoBehaviour
{
	public GameObject point1;
	public GameObject point2;
	public GameObject point3;
	public GameObject point4;
	private GameObject lookTarget; // Where we currently want the camera to be looking
	private GameObject posTarget; // Where we currently want the camera to be positioned

	public int rotSpeed = 5;
	public int moveSpeed = 5;

	// Start is called before the first frame update
	void Start() {
		lookTarget = gameObject;
		posTarget = gameObject;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1))
			lookTarget = point1;
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			lookTarget = point2;
		else if (Input.GetKeyDown(KeyCode.Alpha3))
			lookTarget = point3;
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			lookTarget = point4;
		else if (Input.GetKeyDown(KeyCode.Keypad1))
			posTarget = point1;
		else if (Input.GetKeyDown(KeyCode.Keypad2))
			posTarget = point2;
		else if (Input.GetKeyDown(KeyCode.Keypad3))
			posTarget = point3;
		else if (Input.GetKeyDown(KeyCode.Keypad4))
			posTarget = point4;

		/* Do we need to move? */
		Vector3 targetPosition = posTarget.transform.position;
		if (gameObject.transform.position != targetPosition)
			Move();

		/* Do we need to rotate */
		// If we aren't trying to look at the position we are currently in
		if (lookTarget.transform.position != gameObject.transform.position) {
			Quaternion targetRotation = Quaternion.LookRotation(lookTarget.transform.position - gameObject.transform.position, Vector3.up);
			// If we aren't already looking in the correct direction
			if (gameObject.transform.rotation != targetRotation)
				Rotate();
		}
	}

	void Rotate() {
		// The final direction vector we want to look in
		Vector3 direction = lookTarget.transform.position - gameObject.transform.position;
		// Get the camera's current rotation and the rotation if it was looking towards the target
		Quaternion currentRot = gameObject.transform.rotation;
		Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
		// Rotate the camera towards the target
		gameObject.transform.rotation = Quaternion.Slerp(currentRot, targetRot, rotSpeed * Time.deltaTime);
	}

	void Move() {
		// Get the camera's current position and the target position
		Vector3 currentPos = gameObject.transform.position;
		Vector3 targetPos = posTarget.transform.position;

		// Move the camera towards the target
		gameObject.transform.position = Vector3.Lerp(currentPos, targetPos, moveSpeed * Time.deltaTime);
	}
}
