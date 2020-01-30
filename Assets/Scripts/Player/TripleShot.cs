using System.Collections.Generic;
using UnityEngine;


public class TripleShot : MonoBehaviour
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
		foreach (GameObject laser in myLasers)
		{
			switch (laser.name)
			{
				case "LaserFront":
					laser.transform.localPosition = new Vector3 (0.003f, 0.64f, 0);
					break;

				case "LaserLeft":
					laser.transform.localPosition = new Vector3 (-0.468f, -0.177f, 0);
					break;

				case "LaserRight":
					laser.transform.localPosition = new Vector3 (0.472f, -0.177f, 0);
					break;

				default:
					Debug.Log ("we have a problem here");
					break;
			}

			laser.gameObject.SetActive (false);
		}
	}
}
