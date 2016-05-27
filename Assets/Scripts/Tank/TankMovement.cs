using UnityEngine;

public class TankMovement : MonoBehaviour
{
    
	public int playerNumber = 1;
	public float moveSpeed = 12.0f;
	public float turnSpeed = 75.0f;
	public AudioSource drivingSoundsAudioSource;
	public AudioClip drivingAudioClip;
	public AudioClip idleAudioClip;
	public float pitchRange = 0.2f;

	private string movementInputName;
	private string turnInputName;
	private Rigidbody rb;
	private float movementInputValue;
	private float turnInputValue;
	private float originalPitch;

	void Awake() {
		rb = GetComponent<Rigidbody> ();
	}

	void Start() {

		movementInputName = "Vertical" + playerNumber.ToString();
		turnInputName = "Horizontal" + playerNumber.ToString();

		originalPitch = drivingSoundsAudioSource.pitch;
	}

	void OnEnable() {
		rb.isKinematic = false;


	}

	void OnDisable() {
		rb.isKinematic = true;
	}

	void Update() {
		movementInputValue = Input.GetAxis (movementInputName);
		turnInputValue = Input.GetAxis (turnInputName);

		UpdateAudio ();
	}

	void UpdateAudio() {
		if (Mathf.Abs(movementInputValue) < 0.1f && Mathf.Abs(turnInputValue) < 0.1f) {//not moving
			if (drivingSoundsAudioSource.clip == drivingAudioClip) {
				drivingSoundsAudioSource.clip = idleAudioClip;
				drivingSoundsAudioSource.pitch = Random.Range (originalPitch - pitchRange, originalPitch + pitchRange);
				drivingSoundsAudioSource.Play ();
			}
		} else {
			if (drivingSoundsAudioSource.clip == idleAudioClip) {
				drivingSoundsAudioSource.clip = drivingAudioClip;
				drivingSoundsAudioSource.pitch = Random.Range (originalPitch - pitchRange, originalPitch + pitchRange);
				drivingSoundsAudioSource.Play ();
			}
		}
	}

	void FixedUpdate() {
		Move ();
		Turn ();
	}

	void Move() {
		Vector3 movement = transform.forward * movementInputValue * moveSpeed * Time.deltaTime;
		rb.MovePosition (rb.position + movement);
	}

	void Turn() {
		float turnValue = turnInputValue * turnSpeed * Time.deltaTime;
		Quaternion turn = Quaternion.Euler(0, turnValue, 0);
		rb.MoveRotation (rb.rotation * turn);
	}

}