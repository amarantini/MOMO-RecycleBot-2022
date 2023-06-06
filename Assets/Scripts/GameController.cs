using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Playables;
using RengeGames.HealthBars;
using UnityEngine.SceneManagement;
using System.Timers;

public class GameController : MonoBehaviour
{
    [Header("Game loop")]
    public bool isGameStart = false;
    public bool isGameFinished = false;
    public bool gameWon = false;
    public bool gameLose = false;
    public int Level;
    public Text gameEndtext;
    public int winCoinCount;
    public bool isEasyMode;
    private float startTime;

    //Supervisor
    [Header("Supervisor")]
    public GameObject supervisor;
    public Transform supervisorSpawnPosition;
    [SerializeField] float supervisorSpawnPeriod = 100f;
    private int warningCount = 0;
    [SerializeField] int warningCountToLose = 3;
    [SerializeField] float time = 200f; //next supervisor spawn time
    

    [Header("UI")]
    public List<GameObject> warningIcons;
    public RectTransform map;
    private bool isMapLarge = false;
    public Text timeText;
    public Text inspectionText;
    public Text userManual;
    public Image arrow_1;
    public Image arrow_2;
    public Image arrow_3;
    public List<string> instructions;
    private int instructionIndex;
    [SerializeField] float waitSeconds = 3f;
    public Text warningText;
    public GameObject pauseMenu;
    public GameObject gameoverMenu;
    public UltimateCircularHealthBar timeBar;

    public AudioSource backgroundAudioSource;
    public AudioSource gameloseAudioSource;
    public AudioSource gamewonAudioSource;
    public PlayableDirector director;
    public PlayerController player;
    public Text highScoreText;
    public Text playerScoreText;
    private float highScore;
    public List<GameObject> UIs;


    // Start is called before the first frame update
    void Start()
    {
        director.Play();
        Time.timeScale = 1;
        AudioListener.volume = 1f;
        EventManager.current.onCutsceneEnd += GameStart;
        timeText.text = "Time: " + ((int)time).ToString();
        timeBar.SetPercent(1);
        time = supervisorSpawnPeriod;

        int mode = PlayerPrefs.GetInt("Mode");
        if (mode == 1)
        {
            isEasyMode = true;
        }
        else
        {
            isEasyMode = false;
        }
        if (Level == 1)
        {
            instructionIndex = 0;
        }
    }

    void GameStart()
    {
        isGameStart = true;
        startTime = Time.time;
        StartCoroutine(ShowTutorial());
    }

 

    public IEnumerator ShowTutorial()
    {
        userManual.text = instructions[0];
        yield return new WaitForSeconds(waitSeconds);
        if (Level == 2)
        {
            userManual.gameObject.SetActive(false);
            yield break;
        }

        userManual.text = instructions[1];
        bool getKey = false;
        while (!getKey)
        {
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S)
        || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W)))
                getKey = true;
            yield return null;
        }
           
        userManual.text = instructions[2];
        while (!Input.GetKey(KeyCode.Space))
            yield return null;
            instructionIndex += 1;

        userManual.text = instructions[3];
        while (!Input.GetKey(KeyCode.M))
            yield return null;
        //instruction 4 <- and ->
        if (!player.isAutoGameMode) //Converyor belt mode
        {
            userManual.text = "";
            while (player.isFirstTrash)
                yield return null;
            userManual.text = instructions[4];
            arrow_1.gameObject.SetActive(true);
            while (!(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
                yield return null;
        }

        for(int i=5; i< instructions.Count; i++)
        {
            if (i == 5)
            {
                arrow_2.gameObject.SetActive(true);
                arrow_1.gameObject.SetActive(false);
            }
            if (i == 6)
            {
                arrow_3.gameObject.SetActive(true);
                arrow_2.gameObject.SetActive(false);
            }
            else if (i == 7)
            {
                arrow_3.gameObject.SetActive(false);
            }
            userManual.text = instructions[i];
            yield return new WaitForSeconds(waitSeconds);
        }
        userManual.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameFinished && isGameStart)
        {

            
            

            //Superviosr
            if (!supervisor.activeSelf)
            {
                timeText.gameObject.SetActive(true);
                inspectionText.gameObject.SetActive(false);
                timeText.text = ((int)time).ToString();
                time -= Time.deltaTime;
                timeBar.SetPercent(time / supervisorSpawnPeriod);
            }
            if (time <= 0)
            {
                supervisor.transform.position = supervisorSpawnPosition.position;
                supervisor.SetActive(true);
                supervisor.GetComponent<SupervisorController>().movementSound.Play();
                time = supervisorSpawnPeriod;
                timeText.gameObject.SetActive(false);
                inspectionText.gameObject.SetActive(true);
            }

            //Game won or lose
            if (player.coinCount >= winCoinCount)
            {
                gameWon = true;
                isGameFinished = true;
            }
            else if (warningCount >= warningCountToLose)
            {
                StartCoroutine(DelayedGameLose());
            }
            if (isGameFinished)
            {
                //Switch Audio
                backgroundAudioSource.Stop();
                //Disable supervisor and warning
                supervisor.transform.Find("WarningAudioSource").GetComponent<AudioSource>().Stop();
                supervisor.transform.Find("DroneNoiseAudioSource").GetComponent<AudioSource>().Stop();
                supervisor.SetActive(false);

                //Enable menu
                gameoverMenu.SetActive(true);
                foreach(GameObject ui in UIs)
                {
                    ui.SetActive(false);
                }
                PauseGame();

            }
            if (gameWon)
            {
                gamewonAudioSource.Play();
                warningText.gameObject.SetActive(false);
                gameEndtext.gameObject.SetActive(true);


                if (Level == 1)
                {
                    gameEndtext.text = "Good Job!\nYou are promoted!";
                    PlayerPrefs.SetInt("levelPassed", Level + 1);
                    if (Mathf.Approximately(PlayerPrefs.GetFloat("Highscore_Level1"), 0) ||
                        PlayerPrefs.GetFloat("Highscore_Level1") > Time.time - startTime)
                    {
                        PlayerPrefs.SetFloat("Highscore_Level1", Time.time - startTime);
                    }
                    highScore = PlayerPrefs.GetFloat("Highscore_Level1");
                }
                if (Level == 2)
                {
                    Button nextLevel_button = gameoverMenu.transform.Find("Button_NextLevel").GetComponent<Button>();
                    nextLevel_button.interactable = false;
                    gameEndtext.text = "Thanks for your hardwork!\nEnjoy your retirement!";

                    if (Mathf.Approximately(PlayerPrefs.GetFloat("Highscore_Level2"), 0) ||
                        PlayerPrefs.GetFloat("Highscore_Level2") > Time.time - startTime)
                    {
                        PlayerPrefs.SetFloat("Highscore_Level2", Time.time - startTime);
                    }
                    highScore = PlayerPrefs.GetFloat("Highscore_Level2");
                    StartCoroutine(LoadCreditScene());
                }

                var ts = System.TimeSpan.FromSeconds(highScore);
                highScoreText.text = "Best Time  " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
                ts = System.TimeSpan.FromSeconds(Time.time - startTime);
                playerScoreText.text = "Your Time  " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

            }
            if (gameLose)
            {
                gameloseAudioSource.Play();
                warningText.gameObject.SetActive(true);
                warningText.text = "You are FIRED!";
                Button nextLevel_button = gameoverMenu.transform.Find("Button_NextLevel").GetComponent<Button>();
                nextLevel_button.interactable = false;
            }

            //UI key
            if (Input.GetKey(KeyCode.Escape))//Keyboard.current.escapeKey.isPressed)
            {
                pauseMenu.SetActive(true);
                AudioListener.volume = 0f;
                PauseGame();
            }

            if (Input.GetKeyDown(KeyCode.M))//Keyboard.current.mKey.wasPressedThisFrame)
            {
                if (isMapLarge)
                {
                    map.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    map.localScale = new Vector3(2, 2, 2);
                   
                }
                isMapLarge = !isMapLarge;
            }
        }

    }

    

    public IEnumerator LoadCreditScene()
    {
        yield return new WaitForSecondsRealtime(5);
        Time.timeScale = 1;
        SceneManager.LoadScene("Credits Scene");
    }

    public void IssueWarning()
    {
        warningIcons[warningCount].SetActive(true);
        warningCount += 1;
        //warningBar.value += 1;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    public IEnumerator DelayedGameLose()
    {
        yield return new WaitForSeconds(4);
        gameLose = true;
        isGameFinished = true;
    }

}
