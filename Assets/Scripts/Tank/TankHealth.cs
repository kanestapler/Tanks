using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
	public Color healthyColor = Color.green;
	public Color injuredColor = Color.red;
	public int maxHealth = 100;
	public GameObject tankExplosion;


	private AudioSource explosionAudio;
	private ParticleSystem explosionParticle;
	private Slider healthSlider;
	private Image fillImage;
	private int tankHealth;
	private bool isDead;
    
	void Awake() {
		healthSlider = GetComponentInChildren<Slider> ();
		fillImage = GameObject.Find ("Health Fill").GetComponent<Image> ();
		explosionParticle = Instantiate (tankExplosion).GetComponent<ParticleSystem> ();
		explosionAudio = explosionParticle.GetComponent<AudioSource> ();
		explosionParticle.gameObject.SetActive (false);
	}

	void OnEnable() {
		tankHealth = maxHealth;
		isDead = false;
		UpdateHealthUI ();
	}

	void UpdateHealthUI() {
		healthSlider.value = tankHealth;
		float healthRatio = ((float)tankHealth / (float)maxHealth);
		fillImage.color = Color.Lerp (injuredColor, healthyColor, healthRatio);
	}

	public void Damage(float dam){
		tankHealth -= Mathf.RoundToInt(dam);
		UpdateHealthUI();
		if (tankHealth <= 0 && !isDead) {
			Death ();
		}
	}

	public bool IsDead() {
		return isDead;
	}

	void Death() {
		isDead = true;
		explosionParticle.transform.position = transform.position;
		explosionParticle.gameObject.SetActive (true);
		explosionAudio.Play ();
		gameObject.SetActive (false);
	}

}