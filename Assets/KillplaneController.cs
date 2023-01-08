using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillplaneController : MonoBehaviour
{
    [SerializeField] private GameController _game;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            _game.ResetPlayer();
        }
    }
}
