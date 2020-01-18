using System.Collections.Generic;
using UnityEngine;


public class EnemyTwinLasers : MonoBehaviour
{
	public List <GameObject> myLasers;

	public void OnEnable()
	{
		foreach (GameObject laser in myLasers)
		{
			laser.gameObject.SetActive (true);
		}
	}

	public void OnDisable()
	{
		transform.position = Vector3.zero;
		foreach (GameObject laser in myLasers)
		{
			if (laser.name == "EnemyLaserL")
			{
				laser.transform.position = new Vector3 (-0.125f, -0.83f, 0);
			}
			else if (laser.name == "EnemyLaserR")
			{
				laser.transform.position = new Vector3 (0.125f, -0.83f, 0);
			}
			else
			{
				Debug.Log ("we have a problem here");
			}

			laser.gameObject.SetActive (false);
		}
	}
}