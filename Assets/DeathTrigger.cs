using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{

    public GameController gameController;
    public PlayerController playerController;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(playerController.gameObject))
        {
            playerController.StartCutscene("GettingShot");
        }
    }

}
