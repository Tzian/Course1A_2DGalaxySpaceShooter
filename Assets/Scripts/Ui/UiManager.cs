using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] Text scoreText;
	[SerializeField] Text highScoreText;
	[SerializeField] Image livesImage;
	[SerializeField] Text gameOverText;
	[SerializeField] Text restartLevelText;
	[SerializeField] Sprite[] livesSprites;
#pragma warning restore
	GameManager gameManager;
	int currentScore;

	void Start()
	{
		gameManager = GameObject.Find ("GameManager").GetComponent <GameManager>();
		if (gameManager == null)
		{
			Debug.LogError ("Game Manager is NULL");
		}
		int highScore = PlayerPrefs.GetInt("HighScore");
		Debug.Log ("Start gets high score of  " + highScore);
		highScoreText.text = "High Score:  " + highScore;
		Debug.Log ("high score text is showing " + highScoreText.text);
		gameOverText.gameObject.SetActive (false);
		restartLevelText.gameObject.SetActive (false);
		UpdateScoreText (0);
	}

	public void UpdateScoreText (int updatedScore)
	{
		currentScore = updatedScore;
		scoreText.text = "Score:  " + currentScore;
	}

	public void UpdateLivesImage (int currentLives)
	{
		int lives = currentLives;
		if (lives < 0)
		{
			lives = 0;
		}

		livesImage.sprite = livesSprites[lives];
		if (lives == 0)
		{
			StartCoroutine (DisplayGameOverText());
			restartLevelText.gameObject.SetActive (true);
			gameManager.GameOver();
		}
	}

	IEnumerator DisplayGameOverText()
	{
		while (true)
		{
			gameOverText.gameObject.SetActive (true);
			yield return new WaitForSeconds (0.8f);
			gameOverText.gameObject.SetActive (false);
			yield return new WaitForSeconds (0.5f);
		}
	}
}
