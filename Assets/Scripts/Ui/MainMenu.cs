using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] GameObject settingsPanel;
	[SerializeField] GameObject mainPanel;
	[SerializeField] Dropdown difficultyDropdown;
	[SerializeField] Toggle screenShakeToggle;
#pragma warning restore

	void Awake()
	{
		if (!PlayerPrefs.HasKey ("GameDifficulty"))
		{
			difficultyDropdown.value = 0;
			PlayerPrefs.SetInt("GameDifficulty", 0);
		}
		else
		{
			difficultyDropdown.value = PlayerPrefs.GetInt ("GameDifficulty");
		}
		if (!PlayerPrefs.HasKey ("ScreenShakeOn"))
		{
			screenShakeToggle.isOn = true;
			PlayerPrefs.SetInt("ScreenShakeOn", 1);
		}
		else
		{
			int shakeOn = PlayerPrefs.GetInt ("ScreenShakeOn");
			screenShakeToggle.isOn = shakeOn == 1;
		}
		mainPanel.SetActive(true);
		settingsPanel.SetActive(false);
	}

	public void StartNewGame()
	{
		SceneManager.LoadScene (1); // 1 is our game scene
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void Settings()
	{
		settingsPanel.SetActive(true);
		mainPanel.SetActive(false);
	}

	public void CloseSettings()
	{
		PlayerPrefs.SetInt ("GameDifficulty", difficultyDropdown.value);
		if (screenShakeToggle.isOn)
		{
			PlayerPrefs.SetInt ("ScreenShakeOn", 1);
		}
		else
		{
			PlayerPrefs.SetInt ("ScreenShakeOn", 0);
		}
		settingsPanel.SetActive(false);
		mainPanel.SetActive(true);
	}
}
