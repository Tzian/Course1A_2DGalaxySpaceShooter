using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
#pragma warning disable 0649
	
	[SerializeField] Text gameOverText;
	[SerializeField] Text restartLevelText;

	[Header("HUD Displays")]
	[SerializeField] Text scoreText;
	[SerializeField] Text highScoreText;
	
	[SerializeField] Image livesImage;
	[SerializeField] Image thrusterFuelImage;
	//[SerializeField] Image standardAmmoCountImage;
	//[SerializeField] Image specialAmmoCountImage;
	//TODO: the above will replace the below once we have the sprites ready
	[SerializeField] Text standardAmmoCountText;
	[SerializeField] Text specialAmmoCountText;

	[Header("Sprite Arrays")] 
	[SerializeField] Sprite[] livesSprites;
	[SerializeField] Sprite[] fuelSprites;
	[SerializeField] Sprite[] stdAmmoSprites;
	[SerializeField] Sprite[] spcAmmoSprites;

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
		highScoreText.text = highScore.ToString();
		gameOverText.gameObject.SetActive (false);
		restartLevelText.gameObject.SetActive (false);
		UpdateScoreText (0);
	}

	public void UpdateScoreText (int updatedScore)
	{
		currentScore = updatedScore;
		scoreText.text = currentScore.ToString();
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

	public void UpdateThrusterFuel (int thrusterFuel)
	{
		thrusterFuelImage.fillAmount = (float) thrusterFuel / 100;
	}

	public void UpdateStandardAmmoCount (int stdAmmoCnt)
	{
		standardAmmoCountText.text = stdAmmoCnt.ToString();
	}

	public void UpdateSpecialAmmoCount (int spcAmmoCnt)
	{
		specialAmmoCountText.text = spcAmmoCnt.ToString();
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
