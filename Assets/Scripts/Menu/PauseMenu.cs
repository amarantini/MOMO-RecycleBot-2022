using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private AudioSource[] allAudioSources;
    public GameController game;
    private bool isMusicOff = false;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        Time.timeScale = 1;
        if (!isMusicOff)
            AudioListener.volume = 1f;
        this.gameObject.SetActive(false);
    }

    public void Quit()
    {
        //Application.Quit();
        SceneManager.LoadScene("Intro_Menu");
    }

    public void Music()
    {
        isMusicOff = !isMusicOff;
    }

    public void Restart()
    {
        if (game.Level == 1)
            SceneManager.LoadScene("Level_1");
        if (game.Level == 2)
            SceneManager.LoadScene("Level_2");
    }
}
