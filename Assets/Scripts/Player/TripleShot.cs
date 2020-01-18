using System.Collections.Generic;
using UnityEngine;


public class TripleShot : MonoBehaviour
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
			if (laser.name == "LaserFront")
			{
				laser.transform.position = new Vector3 (0.003f, 0.64f, 0);
			}
			else if (laser.name == "LaserLeft")
			{
				laser.transform.position = new Vector3 (-0.468f, -0.177f, 0);
			}
			else if (laser.name == "LaserRight")
			{
				laser.transform.position = new Vector3 (0.472f, -0.177f, 0);
			}
			else
			{
				Debug.Log ("we have a problem here");
			}

			laser.gameObject.SetActive (false);
		}
	}
}