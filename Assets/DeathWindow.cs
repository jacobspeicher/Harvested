using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWindow : MonoBehaviour
{

    public GameObject[] elements;

    public void DisplayElements()
    {
        foreach (GameObject element in elements)
        {
            element.SetActive(true);
        }
    }

}
