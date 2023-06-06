using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] float rotateSpeed = 6f;
    //Input System
    public PlayerInputActions playerInputActions;

    //Collect Trash
    [Header("Collect Trash")]
    public MyGrid grid;
    [SerializeField] float wipeSpeed = 1f;
    private bool isWiping = false;
    private float wipeFinishTime;
    private float wipePeriod;
    public Slider wipeProgressBar;

    //Trash processing
    public RawImage conveyorView;
    public ConveyorBelt conveyorBelt;
    private List<TrashStats> trashList = new List<TrashStats>();
    private bool isDeliveringTrash = false;

    //Audio
    [Header("Audio")]
    public AudioSource getCoinsSound;
    public AudioSource collectTrashSound;
    public AudioSource wipingSound;
    public AudioSource bumpSound;

    //Coin system
    [Header("Coin")]
    public Text coinCountText;
    public int coinCount = 0;
    //public Slider GoalBar;

    [Header("Others")]
    public Animator anim;
    public GameController game;
    public Rigidbody rb;
    public bool isAutoGameMode = true;

    //For user manual
    public bool isFirstTrash = true;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        rb = GetComponent<Rigidbody>();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        coinCount = 0;
        coinCountText.text = coinCount.ToString();

        wipeProgressBar.maxValue = 1;
        wipeProgressBar.value = 0;


        int mode = PlayerPrefs.GetInt("Mode");
        if (mode == 1)
        {
            isAutoGameMode = true;
        } else
        {
            isAutoGameMode = false;
        }
        if (isAutoGameMode)
        {
            conveyorView.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (game.isGameStart)
        {
            

            if (isWiping)
            {
                wipeProgressBar.value = (wipeFinishTime - Time.time) / wipePeriod;
                if (Time.time >= wipeFinishTime)
                {
                    isWiping = false;
                    wipeProgressBar.gameObject.SetActive(false);
                    anim.SetBool("Wiping", isWiping);
                }
                    
            } else
            {
                move();
            }

            if (trashList.Count > 0 && !isDeliveringTrash)
            {
                isDeliveringTrash = true;
                StartCoroutine(deliverTrash());
            }

            //CheatCode
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    updateCoin(100);
            //}

        }

    }


    public void updateCoin(int change)
    {
        coinCount += change;
        coinCountText.text = coinCount.ToString();
        //GoalBar.value = coinCount;
    }


    public void GetDetergent(float speedUpFactor)
    {
        wipeSpeed *= speedUpFactor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            Trash trash = collision.transform.GetComponent<Trash>();
            TrashStats trashStats = trash.trashStats;

            if (trashStats.needWipe)
            {
                isWiping = true;
                wipeProgressBar.value = 0;
                wipePeriod = trashStats.wipeTime / wipeSpeed;
                wipeFinishTime = Time.time + wipePeriod;
                wipeProgressBar.gameObject.SetActive(true);
                anim.SetBool("Wiping", isWiping);
                //wipingSound.Play();
            }
            if (isAutoGameMode)
            {
                if (trashStats.isRecyclable)
                {
                    GetRecyclable(trashStats);
                }
                else
                {
                    collectTrashSound.Play();
                }
            }
            else
            {
                if (trashStats.trashName == "Trash Bomb")
                {
                    GetRecyclable(trashStats);
                }
                else
                {
                    ProcessTrash(trashStats);
                }
            }


            grid.setGridStatus(collision.transform.position.x, collision.transform.position.y);
            Destroy(collision.gameObject);
        }
        else if(!collision.gameObject.CompareTag("Ground")&& !collision.gameObject.CompareTag("Citizen"))
        {
            bumpSound.Play();
        }
        
    }



    public void ProcessTrash(TrashStats trashStats) //Add trash to trash list
    {
        if (game.Level == 1 && !isAutoGameMode && isFirstTrash)
        {
            isFirstTrash = false;
        }
        trashList.Add(trashStats);
    }

    public IEnumerator deliverTrash() //Send trash to conveyor belt
    {
        conveyorBelt.GetTrash(trashList[0]);
        trashList.RemoveAt(0);
        yield return new WaitForSeconds(0.7f);
        if (trashList.Count > 0)
        {
            StartCoroutine(deliverTrash());
        }
        else
        {
            isDeliveringTrash = false;
        }
    }


    public void GetRecyclable(TrashStats trashStats)
    {
        updateCoin(trashStats.sellingPrice);
        //Play coin get sound
        getCoinsSound.pitch = 0.9f+trashStats.sellingPrice / 10f*0.5f;
        getCoinsSound.Play();
    }

    public void GetOtherTrash()
    {
        collectTrashSound.Play();
    }
    

    private void move()
    {

        //move
        float hAxis = Input.GetAxisRaw("Horizontal");// Input on x ("Horizontal")
        float vAxis = Input.GetAxisRaw("Vertical");// Input on z ("Vertical")
        Vector3 direction = vAxis * transform.forward;

        if (direction.magnitude >= 0.1f)
        {
            anim.SetFloat("Speed", 1);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }


        //transform.Rotate(0, hAxis * Time.deltaTime * rotateSpeed, 0);
        //transform.position += direction * Time.deltaTime;
        Vector3 rotationSpeed = new Vector3(0, rotateSpeed, 0);
        Vector2 inputs = new Vector2(hAxis, vAxis);
        Vector2 inputDirection = inputs.normalized;
        Quaternion deltaRotation = Quaternion.Euler(inputDirection.x * rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
        rb.MovePosition(rb.position + transform.forward * (walkSpeed) * inputDirection.y * Time.deltaTime);


        //rb.MovePosition(direction * Time.deltaTime + transform.position);




        //Input System
        //float movementInput = playerInputActions.Player.Move.ReadValue<float>();
        //float rotationInput = playerInputActions.Player.Rotate.ReadValue<float>();
        //Vector3 direction = movementInput * transform.forward;
        //if (direction.magnitude >= 0.1f)
        //{
        //    direction *= (walkSpeed+extraSpeed);
        //    anim.SetFloat("Speed", 1);
        //} else
        //{
        //    anim.SetFloat("Speed", 0);
        //}
        //transform.position += direction * Time.deltaTime;
        //transform.Rotate(0, rotationInput * Time.deltaTime * rotateSpeed, 0);
    }

    public void Buy(int cost)
    {
        updateCoin(-cost);
    }


}
