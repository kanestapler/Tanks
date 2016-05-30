using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

	private Canvas settingsCanvas;
	private GameManager gameManager;
	private List<Button> buttons = new List<Button>();

	void Awake () {
		settingsCanvas = GetComponentInChildren<Canvas> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		settingsCanvas.enabled = false;
		GetAllButtons ();
	}

	private void GetAllButtons() {
		Button[] allButtons = GetComponentsInChildren<Button> ();
		buttons.AddRange (allButtons);
	}

	public void OpenSettingsMenu() {
		settingsCanvas.enabled = true;
	}

	public void CloseSettingsMenu() {
		settingsCanvas.enabled = false;
	}

	public void SetColorPlayer1(Button button) {
		Color buttonColor = button.colors.normalColor;
		gameManager.players [0].playerColor = buttonColor;
		gameManager.players [0].refreshColors ();
	}
	public void SetColorPlayer2(Button button) {
		Color buttonColor = button.colors.normalColor;
		gameManager.players [1].playerColor = buttonColor;
		gameManager.players [1].refreshColors ();
	}

	public void DeactivateButtons() {
		for (int i = 0; i < buttons.Count; i++) {
			buttons [i].interactable = false;
		}
	}

	public void ActivateButtons() {
		for (int i = 0; i < buttons.Count; i++) {
			buttons [i].interactable = true;
		}
	}
}
