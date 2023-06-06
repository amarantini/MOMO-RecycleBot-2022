using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameController game;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onRestart()
    {
        Time.timeScale = 1;
        AudioListener.volume = 1f;
        if (game.Level==1)
            SceneManager.LoadScene("Level_1");
        if (game.Level == 2)
            SceneManager.LoadScene("Level_2");
    }

    public void onExit()
    {
        Time.timeScale = 1;
        AudioListener.volume = 1f;
        SceneManager.LoadScene("Intro_Menu");
    }

    public void onNextLevel()
    {
        Time.timeScale = 1;
        AudioListener.volume = 1f;
        SceneManager.LoadScene("Level_2");
    }
}
