using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
	public float maxLifetime = 3.0f;
	public float maxDamage = 100f;
	public float explosionForce = 1000f;
	public float explosionRadius = 5.0f;

	private int playerMask;
	private ParticleSystem shellExplosionParticle;
	private AudioSource shellExplosionAudio;

	void Start() {
		playerMask = LayerMask.GetMask ("Players");
		shellExplosionParticle = GetComponentInChildren<ParticleSystem> ();
		shellExplosionAudio = shellExplosionParticle.GetComponent<AudioSource>();
		Destroy (gameObject, maxLifetime);
	}

	void OnTriggerEnter(Collider other) {
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius, playerMask);

		for (int i = 0; i < colliders.Length; i++) {
			Rigidbody targetRB = colliders[i].GetComponent<Rigidbody> ();

			if (!targetRB)
				continue;
			
			targetRB.AddExplosionForce (explosionForce, transform.position, explosionRadius);

			TankHealth tankHealth = targetRB.GetComponent<TankHealth> ();
			if (!tankHealth)
				continue;

			tankHealth.Damage (CalculateDamage(targetRB.position));
		}

		//decouple particles from parent
		shellExplosionParticle.transform.parent = null;
		//play the particles and sound
		shellExplosionParticle.Play();
		shellExplosionAudio.Play ();
		//destroy after particles duration
		Destroy(shellExplosionParticle.gameObject, shellExplosionParticle.duration);
		//destory the shell
		Destroy(gameObject);
	}

	float CalculateDamage(Vector3 targetPosition) {

		//Get distance between target and shell
		Vector3 targetDistance = targetPosition - transform.position;
		//get magnitude of that distance
		float distanceMag = targetDistance.magnitude;
		//get ratio of radius-magnitude/radius
		float dmgRatio = (explosionRadius-distanceMag)/explosionRadius;
		//damage is ratio * maxdamage
		float damage = dmgRatio * maxDamage;
		//absolute value of damage
		damage = Mathf.Abs(damage);
		//return damage
		Debug.Log (damage);
		return damage;
	}
}