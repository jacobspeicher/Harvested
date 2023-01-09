using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _start;

    [SerializeField] private GameObject cutscenePoint;

    public void ResetPlayer()
    {
        Debug.Log("reset player");
        //_player.MovePlayer(cutscenePoint.transform);
        _player.StartCutscene("Dogs");
        AudioManager.Instance.Play("DogsBarking");
        
    }

    public void SendPlayerToStart()
    {
        _player.MovePlayer(_start.transform);
        
    }
}
