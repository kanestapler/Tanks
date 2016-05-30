using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int roundsToWin = 5;
	public float startDelay = 3f;
	public float endDelay = 3f;
	public GameObject tank;
	public TankManager[] players;

	private CameraControl cameraControl;
	private Text messageText;
	private WaitForSeconds startWait;
	private WaitForSeconds endWait;
	private int roundNumber = 0;
	private TankManager roundWinner;
	private TankManager gameWinner;
	private SettingsManager settingsManager;
	private Button playButton;
	private Button settingsButton;

	void Awake() {
		cameraControl = GameObject.Find ("Light Rig").GetComponent<CameraControl> ();
		messageText = GameObject.Find ("MessageText").GetComponent<Text> ();
		settingsManager = GameObject.Find ("SettingsManager").GetComponent<SettingsManager> ();
		playButton = GameObject.Find ("PlayButton").GetComponent<Button>();
		settingsButton = GameObject.Find ("SettingsButton").GetComponent<Button>();

		startWait = new WaitForSeconds (startDelay);
		endWait = new WaitForSeconds (endDelay);

		SpawnAllTanks ();
		SetCameraTargets ();
		PreGame ();
	}

	void SpawnAllTanks() {
		for (int i = 0; i < players.Length; i++) {
			players [i].instance = Instantiate (tank, players [i].spawnPoint.position, players [i].spawnPoint.rotation) as GameObject;
			players [i].playerNumber = i + 1;
			players [i].Setup ();
		}
	}

	void SetCameraTargets() {
		for (int i = 0; i < players.Length; i++) {
			cameraControl.AddTankToCamera (players [i].instance.transform);
		}
	}

	void PreGame() {
		ShellExplosion.maxDamage = 0.0f;
	}

	void ExitPreGame() {
		ShellExplosion.maxDamage = ShellExplosion.DAMAGE;

	}

	IEnumerator GameLoop() {
		yield return RoundStarting ();
		yield return RoundPlaying ();
		yield return RoundEnding ();

		if (gameWinner == null) {
			StartCoroutine (GameLoop ());
		} else {//Game was won
			ActivateAllButtons();
			roundNumber = 0;
		}
	}

	IEnumerator RoundStarting() {
		//reset all tanks
		ResetAllTanks();
		//disable tank control
		DisableAllTankControl();
		//Set camera position
		cameraControl.SetStartPositionAndSize();
		//increment round num
		roundNumber++;
		//set text to round #
		messageText.text = "Round " + roundNumber.ToString();
		yield return startWait;
	}

	IEnumerator RoundPlaying() {
		
		EnableAllTankControl ();
		messageText.text = string.Empty;
		while (!OneTankLeft ()) {
			yield return null;
		}
	}

	IEnumerator RoundEnding() {
		DisableAllTankControl ();
		roundWinner = GetRoundWinner();
		if (roundWinner != null)
			roundWinner.Won ();
		gameWinner = GetGameWinner ();

		string message = GetEndMessage ();
		messageText.text = message;

		yield return endWait;
	}

	string GetEndMessage() {
		string returnMessage = string.Empty;

		if (roundWinner != null) {//someone won the round
			returnMessage = roundWinner.coloredPlayerText + " wins the round!";
		} else {
			returnMessage = "Draw!";
		}

		returnMessage += "\n\n\n";

		for (int i = 0; i < players.Length; i++) {
			returnMessage += players[i].coloredPlayerText + " wins: " + players[i].GetWins().ToString() + "\n";
		}

		if (gameWinner != null) {//someone won the game
			returnMessage = roundWinner.coloredPlayerText + " WINS THE GAME!";
		}

		return returnMessage;
	}

	bool OneTankLeft() {
		int tanksAlive = 0;
		for (int i = 0; i < players.Length; i++) {
			if (players [i].instance.activeSelf) {
				tanksAlive++;
			}
		}
		return !(tanksAlive > 1);
	}

	TankManager GetRoundWinner() {
		for (int i = 0; i < players.Length; i++) {
			if (players [i].instance.activeSelf) {
				return players [i];
			}
		}
		return null;
	}

	TankManager GetGameWinner() {
		for (int i = 0; i < players.Length; i++) {
			if (players [i].GetWins () >= roundsToWin) {
				Debug.Log ("Winner");
				return players [i];
			}
		}
		return null;
	}

	void ResetAllTanks() {
		for (int i = 0; i < players.Length; i++) {
			players [i].Reset ();
		}
	}

	void DisableAllTankControl() {
		for (int i = 0; i < players.Length; i++) {
			players [i].DisableControl ();
		}
	}

	void EnableAllTankControl() {
		for (int i = 0; i < players.Length; i++) {
			players [i].EnableControl ();
		}
	}

	public void Play() {
		ExitPreGame ();
		DeactivateAllButtons ();
		StartCoroutine(GameLoop());
	}

	void DeactivateAllButtons() {
		playButton.enabled = false;
		playButton.GetComponentInChildren<Text> ().text = "";
		settingsButton.enabled = false;
		settingsButton.GetComponentInChildren<Text> ().text = "";
		settingsManager.DeactivateButtons();
	}

	void ActivateAllButtons() {
		playButton.enabled = true;
		playButton.GetComponentInChildren<Text> ().text = "Play";
		settingsButton.enabled = true;
		settingsButton.GetComponentInChildren<Text> ().text = "Settings";
		settingsManager.ActivateButtons();
	}
}