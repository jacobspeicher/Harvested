using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _character;

    public GameController _gameController;
    public GameObject DeathPoint;
    public PlayerMovement playerMovement;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _animator = _character.GetComponent<Animator>();
        playerMovement.acceptingInputs = true;
    }
    void Start()
    {
        Debug.Log(_animator);
    }

    private void Update()
    {

    }

    public void MovePlayer(Transform inTransform)
    {
        _controller.enabled = false;
        transform.position = inTransform.position;
        _controller.enabled = true;
    }

    public void StartCutscene(string scene)
    {
        playerMovement.acceptingInputs = false;
        if (scene.Equals("Dogs"))
        {
            _animator.SetTrigger("FieldReset");
        }
        if (scene.Equals("GettingShot"))
        {
            playerMovement.acceptingInputs = false;
            //_animator.SetTrigger("GoGetShot");
            StartCoroutine(GettingShot());
        }
    }

    IEnumerator GettingShot()
    {
        _animator.SetBool("Moving", true);
        for(Vector3 dist = Vector3.one; dist.magnitude > 0.2f; dist = (DeathPoint.transform.position - transform.position))
        {
            _controller.Move(dist.normalized * 2 * Time.deltaTime);
            //print("Walking there");
            yield return new WaitForEndOfFrame();
        }
        print("done walking");
        AudioManager.Instance.Stop("Footsteps");
        _animator.SetBool("Moving", false);
        _animator.SetTrigger("Stumble");
        _gameController.StartFarmerShooting();
    }

    public void sendPlayerToStart()
    {
        _animator.SetTrigger("PlayerToIdle");
        _gameController.SendPlayerToStart();
        playerMovement.acceptingInputs = true;
    }

    public void Kill()
    {
        StartCoroutine(KillPlayer());
    }

    public GameObject deathWindow;

    IEnumerator KillPlayer()
    {
        Color col = new Color(0, 0, 0, 0);
        for (int i = 0; i <= 100; i++)
        {
            col = new Color(0, 0, 0, Mathf.Lerp(0, 1, i / 100));
            deathWindow.GetComponent<Image>().color = col;
            yield return new WaitForEndOfFrame();
        }
        //deathWindow.GetComponent<DeathWindow>().DisplayElements();
        _gameController.NextLevel();
    }
}
