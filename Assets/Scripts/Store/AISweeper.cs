using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;

public class AISweeper : MonoBehaviour
{
    //private Animator anim;
    public GameController game;
    private PlayerController player;

    //Motor
    [Header("Movement")]
    private NavMeshAgent agent;
    private Transform targetPosition;
    public float walkSpeed = 2f;


    //Patrol and look for player
    [Header("Patrol")]
    [SerializeField] float patrolRadius = 15f;
    [SerializeField] float minPatrolTime = 3f, maxPatrolTime = 5f;
    private bool isPatrolling = false;
    private Vector3 spawnPoint;

    [Header("Pickup Trash")]
    [SerializeField] float radarRange = 5;
    public LayerMask trashMask;
    //Collect Trash
    public MyGrid grid;
    [SerializeField] float wipeTime = 2f;
    private bool isWiping = false;
    private float wipeFinishTime;
    public Slider wipeProgressBar;

    public bool isPlaced = false;
    private float attractedByTrashTime;
    [SerializeField] float maxAttractedByTrashTime = 10f;
    private bool findTrash = false;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        agent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();
        spawnPoint = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<MyGrid>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!game.isGameFinished && isPlaced)
        {

            if (!isWiping)
            {
                if (isPatrolling)
                {
                    checkTrashInRange();
                }
                if (findTrash)
                {
                    if (Time.time >= attractedByTrashTime + maxAttractedByTrashTime)
                    {
                        findTrash = false;
                    }
                    else 
                        moveToTarget();
                } else 
                {
                    if (!isPatrolling)
                        StartCoroutine(chosePatrolLocation());
                }

            }
            else
            {
                stop();
                wipeProgressBar.value += Time.deltaTime;
                if (Time.time >= wipeFinishTime)
                {
                    isWiping = false;
                    wipeProgressBar.value = 0;
                    wipeProgressBar.gameObject.SetActive(false);
                }

            }
            //if (agent.velocity.magnitude >= 0.1f)
            //{
            //    anim.SetFloat("Speed", 1);
            //}
            //else
            //{
            //    anim.SetFloat("Speed", 0);
            //}
        }

    }

    private void LateUpdate()
    {
        if(wipeProgressBar.gameObject.activeSelf)
            wipeProgressBar.transform.forward = -Camera.main.transform.forward;
    }

    //Check Trash in range
    private void checkTrashInRange()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, radarRange, trashMask))
        {
            targetPosition = col.transform;
            findTrash = true;
            attractedByTrashTime = Time.time;
            isPatrolling = false;
            return;
        }
        findTrash =  false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlaced && collision.gameObject.CompareTag("Trash"))
        {
            Trash trash = collision.transform.GetComponent<Trash>();
            TrashStats trashStats = trash.trashStats;
            if (trashStats.isRecyclable)
            {
                player.GetRecyclable(trashStats);
            }
            if (trashStats.needWipe)
            {
                stop();
                isWiping = true;
                wipeFinishTime = Time.time + wipeTime;
                wipeProgressBar.gameObject.SetActive(true);
            }
            grid.setGridStatus(collision.transform.position.x, collision.transform.position.y);
            Destroy(collision.gameObject);
            findTrash = false;
            StartCoroutine(chosePatrolLocation());
        }
    }





    public IEnumerator chosePatrolLocation()
    {
        isPatrolling = true;
        //gets point within the patrol radius of the enemy, by at the same height (Y)
        Vector3 offset = Random.insideUnitSphere * patrolRadius;
        offset.y = 0;
        offset += spawnPoint;//transform.position;
        move(offset);
        yield return new WaitForSeconds(Random.Range(minPatrolTime, maxPatrolTime));
        if(!findTrash)
            StartCoroutine(chosePatrolLocation());
    }



    public void setGameController(GameController _gameController)
    {
        game = _gameController;
    }


    //Movement ====================================
    public void move(Vector3 pos)
    {
        nullTarget();
        agent.isStopped = false;
        agent.speed = walkSpeed;
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    public void moveToTarget()
    {
        agent.isStopped = false;
        agent.speed = walkSpeed;
        agent.SetDestination(targetPosition.position);
    }

    public void stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public void stay()
    {
        agent.speed = 0f;
    }

    public void setTarget(Transform t)
    {
        targetPosition = t;
    }

    public void nullTarget()
    {
        targetPosition = null;
    }
}
