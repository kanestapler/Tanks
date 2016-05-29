using System;
using UnityEngine;

[Serializable]
public class TankManager {
    
	public Color playerColor;
	public Transform spawnPoint;
	[HideInInspector] public GameObject instance;
	[HideInInspector] public int playerNumber;
	[HideInInspector] public string coloredPlayerText;

	private int wins = 0;
	private TankMovement tankMovement;
	private TankShooting tankShooting;
	private GameObject canvasGameObject;

	public void Setup() {
		tankMovement = instance.GetComponent<TankMovement> ();
		tankShooting = instance.GetComponent<TankShooting> ();
		canvasGameObject = instance.GetComponentInChildren<Canvas> ().gameObject;

		tankMovement.playerNumber = playerNumber;
		tankShooting.playerNumber = playerNumber;

		coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">Player " + playerNumber.ToString() + "</color>";

		MeshRenderer[] meshes = instance.GetComponentsInChildren<MeshRenderer> ();

		for (int i = 0; i < meshes.Length; i++) {
			meshes [i].material.color = playerColor;
		}
	}

	public void DisableControl() {
		tankMovement.enabled = false;
		tankShooting.enabled = false;
		canvasGameObject.SetActive (false);
	}

	public void EnableControl() {
		tankMovement.enabled = true;
		tankShooting.enabled = true;
		canvasGameObject.SetActive (true);
	}

	public void Reset() {
		instance.transform.position = spawnPoint.position;
		instance.transform.rotation = spawnPoint.rotation;

		instance.SetActive (false);
		instance.SetActive (true);
	}

	public int GetWins() {
		return wins;
	}

	public void Won() {
		wins++;
	}

}
