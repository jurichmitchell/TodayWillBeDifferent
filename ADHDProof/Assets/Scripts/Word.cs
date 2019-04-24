using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word : MonoBehaviour {
	public bool usePlane = false; // DEBUG

	public Camera cam; // The main camera
	private GameObject parentWordContainer; // The parent GameObject for all words
	public Vector3 defaultPosition; // This is the position that the word defaults to (relative to it's word container parent)
	public Vector3 currentPosition; // This is where the word is currently located (relative to it's word container parent)
	//public Vector3 lastMousePos;
	//public Vector3 changeInMouse;
	bool dragging = false;
	public float returnSpeed = 0.5f;

    // Start is called before the first frame update
    void Start() {
		parentWordContainer = gameObject.transform.parent.gameObject;
		defaultPosition = gameObject.transform.localPosition;
		currentPosition = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update() {

		// If mouse is not down, we aren't dragging
		if (!Input.GetMouseButton(0)) {
			dragging = false;
		}
		if (dragging) {
			if (usePlane)
				getNewPosPlane();
			else
				getNewPos();			
		}
		else {
			// Return the word to it's default position
			if (currentPosition != defaultPosition) {
				currentPosition = Vector3.Lerp(currentPosition, defaultPosition, Time.deltaTime * returnSpeed);
			}
		}

		// Update this word's position
		gameObject.transform.localPosition = currentPosition;

		// Rotate this word towards the camera
		Vector3 towardsCam = cam.transform.position - gameObject.transform.position;
		Quaternion targetRot = Quaternion.LookRotation(towardsCam, Vector3.up);
		gameObject.transform.rotation = targetRot;
	}

	private void OnMouseOver() {
		if (Input.GetMouseButtonDown(0))
			dragging = true;
			//lastMousePos = Input.mousePosition;
		/*if (Input.GetMouseButton(0)) {
			//Vector3 currMousePos = Input.mousePosition;
			//changeInMouse = lastMousePos - currMousePos;
			//changeInMouse = new Vector3(changeInMouse.x / Screen.width, changeInMouse.y / Screen.height, 0);



			//currentPosition = gameObject.transform.position - changeInMouse;
			//lastMousePos = currMousePos;

			Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
			currentPosition = mousePos.GetPoint(2);
			
		}*/
	}

	// Calculates the new position of the word keeping the distance of the word from the camera
	// equal to the distance the parenWordContainer is from the camera. This results in a more
	// spherical movement of the word around the camera.
	void getNewPos() {
		// Return a ray from the camera towards the world space the mouse is located in
		//		NOTE: the ray's origin is the near plane of the camera, not the camera's position
		Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);

		// Get the distance from the ray's origin to parentWordContainer
		float dist = (parentWordContainer.transform.position - mousePos.origin).magnitude;
		// Get the world position of a point on the ray with this distance from the ray's origin
		Vector3 worldPoint = mousePos.GetPoint(dist);

		// Translate the world point relative to parentWordContainer's object space
		currentPosition = parentWordContainer.transform.InverseTransformPoint(worldPoint);

		Debug.DrawRay(mousePos.GetPoint(0), worldPoint - mousePos.GetPoint(0), Color.red); // DEBUG
	}

	// Calculates the new position of the word by constructing a plane at the position of
	// the parentWordContainer, then calculating the distance from the camera to this
	// plane. This allows the distance to vary, so the word will keep a constant z position
	// resulting in more planar motion.
	void getNewPosPlane() {
		// Return a ray from the camera towards the world space the mouse is located in
		Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);

		// Create a plane at the world position of the parentWordContainer with normal facing the camera
		Vector3 containerToCam = (parentWordContainer.transform.position - cam.transform.position);
		Plane distancePlane = new Plane(containerToCam.normalized, parentWordContainer.transform.position);

		// Figure out the distance from the camera to the plane following the mousePos ray
		float distance = 0;
		// Plane.Raycast stores the distance from mousePos's ray.origin to the plane in distance variable
		distancePlane.Raycast(mousePos, out distance);

		// Get the world position of the point where the ray intersects the plane
		Vector3 worldPoint = mousePos.GetPoint(distance);

		// Translate the world point relative to parentWordContainer's object space
		currentPosition = parentWordContainer.transform.InverseTransformPoint(worldPoint);

		Debug.DrawRay(mousePos.GetPoint(0), worldPoint - mousePos.GetPoint(0), Color.red); // DEBUG
	}
}
