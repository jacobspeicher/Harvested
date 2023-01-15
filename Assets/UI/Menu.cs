using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject CreditMenu;
    private void Start()
    {
        /*
        AudioManager.Instance.Play("MenuMusic");
        try
        {
            AudioManager.Instance.gameObject.GetComponentInChildren<MusicHandler>().stopMusic();
        }
        catch { }
        */
        StartCoroutine(menuMusic());
    }

    IEnumerator menuMusic()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        AudioManager.Instance.Play("MenuMusic");
    }

    public void PlayButton()
    {
        Play();
    }

    public void Play()
    {
        StartCoroutine(startGame());
    }

    IEnumerator startGame()
    {
        yield return new WaitForEndOfFrame();
        AudioManager.Instance.Stop("MenuMusic");
        //AudioManager.Instance.gameObject.GetComponentInChildren<MusicHandler>().playMusic();
        MainMenu.SetActive(false);
        AudioManager.Instance.Play("Level1Music");
        SceneManager.LoadScene("Cornfield");
    }

    public void Credits()
    {
        MainMenu.SetActive(false);
        CreditMenu.SetActive(true);
    }

    public void ReturnToMain()
    {
        CreditMenu.SetActive(false);
        MainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
