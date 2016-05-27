using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{

	public bool lockRotation = true;

	private Quaternion startingRotation;

	void Start() {
		startingRotation = transform.parent.localRotation;
	}

	void Update() {
		if (lockRotation) {
			transform.rotation = startingRotation;
		}
	}

}
