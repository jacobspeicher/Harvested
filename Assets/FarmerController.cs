using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : MonoBehaviour
{

    Animator animator;
    public GameController gameController;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartShooting()
    {
        animator.SetTrigger("StartShooting");
    }
    
    public void KillPlayer()
    {
        gameController.KillPlayer();
    }


}
