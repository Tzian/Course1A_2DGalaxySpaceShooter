using System.Collections.Generic;
using UnityEngine;


public class EnemyTwinLasers : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] List <GameObject> myLasers;
#pragma warning restore
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
			switch (laser.name)
			{
				case "EnemyLaserL":
					laser.transform.position = new Vector3 (-0.125f, -0.83f, 0);
					break;

				case "EnemyLaserR":
					laser.transform.position = new Vector3 (0.125f, -0.83f, 0);
					break;

				default:
					Debug.Log ("we have a problem here");
					break;
			}

			laser.gameObject.SetActive (false);
		}
	}
}
