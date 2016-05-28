using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    
	public int playerNumber = 1;
	public float minShotForce = 15.0f;
	public float maxShotForce = 30.0f;
	public float maxChargeTime = 0.75f;
	public AudioSource shootingAudioSource;
	public AudioClip chargingAudioClip;
	public AudioClip firingAudioClip;
	public GameObject shell;

	private string fireButton;
	private float currentLaunchForce;
	private Slider aimSlider;
	private string playerFireButton;
	private float chargeRate;
	private Transform fireOrigin;

	void Awake() {
		aimSlider = transform.Find ("Canvas").transform.Find("AimSlider").GetComponent<Slider> ();
		fireOrigin = transform.Find ("FireOrigin").transform;

	}

	void OnEnable () {
		currentLaunchForce = minShotForce;
		aimSlider.value = minShotForce;
	}

	void Start(){
		playerFireButton = "Fire" + playerNumber.ToString ();
		chargeRate = (maxShotForce - minShotForce) / maxChargeTime;
	}

	void Update() {
		aimSlider.value = minShotForce;

		if (Input.GetButtonDown (playerFireButton)) {
			currentLaunchForce = minShotForce;
			shootingAudioSource.clip = chargingAudioClip;
			shootingAudioSource.Play ();
		} else if (Input.GetButton (playerFireButton)) {
			
			if (currentLaunchForce > maxShotForce) {
				currentLaunchForce = maxShotForce;
			} else {
				currentLaunchForce += chargeRate * Time.deltaTime;
			}
			aimSlider.value = currentLaunchForce;
		} else if (Input.GetButtonUp (playerFireButton)) {
			if (currentLaunchForce > maxShotForce) {
				currentLaunchForce = maxShotForce;
			}
			Fire();
		}
	}

	void Fire() {
		shootingAudioSource.clip = firingAudioClip;
		shootingAudioSource.Play ();
		GameObject shellFired = Instantiate (shell, fireOrigin.position, fireOrigin.rotation) as GameObject;
		shellFired.GetComponent<Rigidbody> ().velocity = currentLaunchForce * fireOrigin.forward;
		currentLaunchForce = minShotForce;
	}

}