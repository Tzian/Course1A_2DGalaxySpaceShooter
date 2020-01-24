using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterShot : MonoBehaviour
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
                laser.transform.position = new Vector3 (-0.4f, 0.5f, 0);
            }
            else if (laser.name == "LaserRight")
            {
                laser.transform.position = new Vector3 (0.4f, 0.5f, 0);
            }
            else if (laser.name == "LaserFarLeft")
            {
                laser.transform.position = new Vector3 (-0.7f, 0.25f, 0);
            }
            else if (laser.name == "LaserFarRight")
            {
                laser.transform.position = new Vector3 (0.7f, 0.25f, 0);
            }
            else
            {
                Debug.Log ("we have a problem here");
            }

            laser.gameObject.SetActive (false);
        }
    }
}
