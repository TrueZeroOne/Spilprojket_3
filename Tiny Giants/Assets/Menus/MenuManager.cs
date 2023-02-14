using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	private const int startMenu = 0, settingsMenu = 1, keybindMenu = 2, creditMenu = 3, startGame = 4;
	public void PlayGame() => SceneManager.LoadScene(startGame);
	public void SettingsMenu() => SceneManager.LoadScene(settingsMenu);
	public void KeybindsMenu() => SceneManager.LoadScene(keybindMenu);
	public void CreditsMenu() => SceneManager.LoadScene(creditMenu);
	public void StartMenu() => SceneManager.LoadScene(startMenu);
	public void QuitGame() => Application.Quit();
	private void Update()
	{
		int buildI = SceneManager.GetActiveScene().buildIndex;
		if (buildI is 1 or 2 or 3 && Keyboard.current.escapeKey.isPressed) StartMenu();
	}
}
