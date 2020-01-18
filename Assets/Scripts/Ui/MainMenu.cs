using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
	public void StartNewGame()
	{
		SceneManager.LoadScene (1); // 1 is our game scene
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}