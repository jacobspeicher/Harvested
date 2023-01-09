using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] PlayerController _player;

    int repeatAnimCount = 0;

    public void triggerReset()
    {
        /*
        if(repeatAnimCount < 1)
        {
            repeatAnimCount++;
            return;
        }
        repeatAnimCount = 0;
        */
        _player.sendPlayerToStart();
    }

}
