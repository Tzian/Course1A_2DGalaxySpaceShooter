﻿using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] Canvas pauseUi;
#pragma warning restore
	bool gameOver;
	int currentScore;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.R) && gameOver)
		{
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene (scene.buildIndex);
			gameOver = false;
		}

		if (Input.GetKeyDown (KeyCode.Escape))
		{
			if (Time.timeScale == 1)
			{
				Time.timeScale = 0;
				pauseUi.gameObject.SetActive (true);
				Cursor.lockState = CursorLockMode.None;
				return;
			}

			if (Time.timeScale == 0)
			{
				pauseUi.gameObject.SetActive (false);
				Cursor.lockState = CursorLockMode.Locked;
				Time.timeScale = 1;
			}
		}
	}

	public void GameOver()
	{
		gameOver = true;
	}

	public void ExitGame()
	{
		SaveHighScore();
		Application.Quit();
	}

	public void UpdateScore (int score)
	{
		currentScore = score;
		SaveHighScore();
	}
	
	void SaveHighScore()
	{
		int oldHighScore = PlayerPrefs.GetInt ("HighScore");

		if (currentScore > oldHighScore)
		{
			PlayerPrefs.SetInt ("HighScore", currentScore);
			PlayerPrefs.Save();
		}
	}
}
