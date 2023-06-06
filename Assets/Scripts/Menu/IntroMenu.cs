using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class IntroMenu : MonoBehaviour
{
    public GameObject LevelSelectionWindow;
    int levelPassed;
    public List<Button> levelButtons;

    // Use this for initialization
    void Awake()
    {
        if (!PlayerPrefs.HasKey("levelPassed"))
            PlayerPrefs.SetInt("levelPassed", 0);
        PlayerPrefs.SetInt("Mode", 0);
        
    }

    private void Start()
    {
        levelPassed = PlayerPrefs.GetInt("levelPassed");
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i <= levelPassed)
            {
                levelButtons[i].interactable = true;
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))//Reset levelPassed
        {
            PlayerPrefs.SetFloat("Highscore_Level1", float.MaxValue);
            PlayerPrefs.SetFloat("Highscore_Level2", float.MaxValue);
            PlayerPrefs.SetInt("levelPassed",0);
            for (int i = 0; i < levelButtons.Count; i++)
            {
                if (i <= levelPassed)
                {
                    levelButtons[i].interactable = true;
                }
                else
                {
                    levelButtons[i].interactable = false;
                }
            }
        }
    }

    public void OnPlayButton()
    {
        LevelSelectionWindow.SetActive(true);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnLevel1Button()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OnLevel2Button()
    {
        SceneManager.LoadScene("Level_2");
    }

    public void onEasyModeToggle(bool ison)
    {
        if(ison)
        {
            PlayerPrefs.SetInt("Mode", 1);//Easy mode
        }
            
    }

    public void onHardModeToggle(bool ison)
    {
        if (ison)
        {
            PlayerPrefs.SetInt("Mode", 0);//Hard mode
        }

    }

    public void onCreditsButton()
    {
        SceneManager.LoadScene("Credits Scene");
    }
}
