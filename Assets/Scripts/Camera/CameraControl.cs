using UnityEngine;

public class CameraControl : MonoBehaviour
{
    
	public float screenPadding = 4.0f;
	public float minSize = 6.5f;
	public float dampTime = 0.2f;
	public Transform[] targets;

	private Camera cam;
	private float zoomSpeed;
	private Vector3 moveVelocity;
	private Vector3 desiredPosition;

	void Awake() {
		cam = GetComponentInChildren<Camera> ();
	}

	void FixedUpdate() {
		Move ();
		Zoom ();
	}

	void Move() {
		FindAveragePosition ();
		transform.position = Vector3.SmoothDamp (transform.position, desiredPosition, ref moveVelocity, dampTime);
	}

	void FindAveragePosition() {
		Vector3 averagePosition = new Vector3 ();
		int numOfTargetsActive = 0;
		for (int i = 0; i < targets.Length; i++) {
			if (!targets [i].gameObject.activeSelf)
				continue;
			averagePosition += targets [i].position;
			numOfTargetsActive++;
		}
		if (numOfTargetsActive > 0) {
			averagePosition /= numOfTargetsActive;
		}
		averagePosition.y = transform.position.y;
		desiredPosition = averagePosition;
	}

	void Zoom() {
		float requiredSize = FindRequiredSize ();
		cam.orthographicSize = Mathf.SmoothDamp (cam.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
	}

	float FindRequiredSize() {
		float size = 0.0f;
		Vector3 desiredLocalPos = transform.InverseTransformPoint (desiredPosition);

		for (int i = 0; i < targets.Length; i++) {
			if (!targets [i].gameObject.activeSelf)
				continue;
			Vector3 targetLocalPos = transform.InverseTransformPoint (targets [i].position);
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));
			size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / cam.aspect);
		}

		size += screenPadding;

		size = Mathf.Max (size, minSize);
		return size;
	}

	public void SetStartPositionAndSize() {
		cam.orthographicSize = FindRequiredSize ();
		FindAveragePosition ();
		transform.position = desiredPosition;
	}
}