﻿using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] Canvas pauseUi;
#pragma warning restore
	bool gameOver;
	int currentScore;
	int gameDifficulty;

	void Awake()
	{
		pauseUi.enabled = false;
		Time.timeScale = 1;
		gameDifficulty = PlayerPrefs.GetInt ("GameDifficulty");
	}

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
			switch (Time.timeScale)
			{
				case 1:
					Time.timeScale = 0;
					pauseUi.enabled = true;
					Cursor.lockState = CursorLockMode.None;
					return;

				case 0:
					pauseUi.enabled = false;
					Cursor.lockState = CursorLockMode.Locked;
					Time.timeScale   = 1;
					break;
			}
		}
	}

	public void GameOver()
	{
		gameOver = true;
	}

	public void ResumeGame()
	{
		pauseUi.enabled = false;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale   = 1;
	}
	public void ExitToMainMenu()
	{
		SaveHighScore();
		SceneManager.LoadScene (0); // 0 is the main menu scene
	}

	public void UpdateScore (int score)
	{
		currentScore = score;
		SaveHighScore();
	}

	void SaveHighScore()
	{
		string key = "HighScore" + gameDifficulty;
		int oldHighScore = PlayerPrefs.GetInt (key);

		if (currentScore <= oldHighScore)
		{
			return;
		}
		PlayerPrefs.SetInt (key, currentScore);
		PlayerPrefs.Save();
	}
}
