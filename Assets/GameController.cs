using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _start;

    public void ResetPlayer()
    {
        Debug.Log("reset player");
        _player.MovePlayer(_start.transform);
    }
}
