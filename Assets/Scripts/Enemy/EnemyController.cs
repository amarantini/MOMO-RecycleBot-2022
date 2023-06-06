using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    public GameController game;

    public bool troublemaker = false;
    [SerializeField] float chanceToKnockdownTrashbin = 0.2f;
    [SerializeField] float chanceToThrowTrashBomb = 0.3f;
    public GameObject trashBombPrefab;

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

    [Header("Throw Trash")]
    public MyGrid grid;
    public List<GameObject> trashPrefabs;
    private float minThrowInterval;
    private float maxThrowInterval;
    [SerializeField] float minThrowIntervalHard = 12f;
    [SerializeField] float maxThrowIntervalHard = 20f;
    [SerializeField] float minThrowIntervalHard_troublemaker = 12f;
    [SerializeField] float maxThrowIntervalHard_troublemaker = 15f;
    [SerializeField] float minThrowIntervalEasy = 10f;
    [SerializeField] float maxThrowIntervalEasy = 15f;
    [SerializeField] float minThrowIntervalEasy_troublemaker = 7f;
    [SerializeField] float maxThrowIntervalEasy_troublemaker = 11f;
    [SerializeField] float maxThrowIntervalChange = 5;
    [SerializeField] float minThrowIntervalChange = -4;
    [SerializeField] float timeToReduceThrowInterval = 60;
    private float currThrowIntervalChange = 3;
    private float nextThrowIntervalDecreaseTime;
    //[SerializeField] int maxSearchRange = 10;
    private float nextThrowTime;
    private GameObject nextThrowTrashPrefab;
    private bool thrown = false;

    [SerializeField] float maxTrashBinrange = 20;
    private bool isAttractedByTrashbin = false;
    private Trashbin trashbin;
    public LayerMask trashbinMask;
    private Transform trashbinLocation;
    [SerializeField] float minDistanceToTrashbin = 3f;

    public List<int> trashNeedHigherY = new List<int>() { 4, 6, 7, 8, 9, 11 };
    private float trash_y;
    [SerializeField] int trash_range= 16;

    private float attractedByTrashbinTime;
    [SerializeField] float maxAttractedByTrashbinTime = 10f;
    public LayerMask nonGroundMask;
    [SerializeField] float radius = 0.5f;

    //Audio
    public AudioSource hurtSound;
    public AudioSource troubleMakingSound;




    // Start is called before the first frame update
    void Start()
    {
        currThrowIntervalChange = maxThrowIntervalChange;
        nextThrowIntervalDecreaseTime = Time.time + timeToReduceThrowInterval;

        game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        grid = GameObject.FindWithTag("Grid").GetComponent<MyGrid>();
        
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        spawnPoint = transform.position;

        if (game.isEasyMode)
        {
            if (troublemaker)
            {
                minThrowInterval = minThrowIntervalEasy_troublemaker;
                maxThrowInterval = maxThrowIntervalEasy_troublemaker;
            } else
            {
                minThrowInterval = minThrowIntervalEasy;
                maxThrowInterval = maxThrowIntervalEasy;
            }
        } else
        {
            if (troublemaker)
            {
                minThrowInterval = minThrowIntervalHard_troublemaker;
                maxThrowInterval = maxThrowIntervalHard_troublemaker;
            }
            else
            {
                minThrowInterval = minThrowIntervalHard;
                maxThrowInterval = maxThrowIntervalHard;
            }
        }
        

        if (game.Level == 1)
        {
            trash_range = 9;
        } else
        {
            trash_range = trashPrefabs.Count;
        }

        prepareNextTrash();


    }

    // Update is called once per frame
    void Update()
    {
        
        if (!game.isGameFinished && game.isGameStart)
        {
            if (Time.time >= nextThrowIntervalDecreaseTime)
            {
                currThrowIntervalChange -= 1;
                nextThrowIntervalDecreaseTime = Time.time + timeToReduceThrowInterval;
            }
            if (Time.time>=nextThrowTime )
            {
                thrown = false;
            }
            if (thrown)
            {
                if (!isPatrolling)
                    StartCoroutine(chosePatrolLocation());
            } else
            {
                checkTrashbinInRange();
                
                if (isAttractedByTrashbin)
                {
                    if(Time.time >= attractedByTrashbinTime + maxAttractedByTrashbinTime)
                    {
                        isAttractedByTrashbin = false;
                    }
                    PlaceTrashInTrashbin();
                    
                } else //if found no trashbin
                {
                    if(troublemaker && Random.Range(0f,1f)< chanceToThrowTrashBomb)
                    {
                        nextThrowTrashPrefab = trashBombPrefab;
                    }
                     PlaceTrashNear();
                    
                }
                    
            }
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }

    }

    //Check Trash Bin
    private void checkTrashbinInRange()
    {
        foreach (Collider col in Physics.OverlapSphere(transform.position, maxTrashBinrange, trashbinMask))
        {
            trashbin = col.transform.GetComponent<Trashbin>();
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (trashbin.isPlaced && trashbin.range >= distance && !trashbin.isDown)
            {
                isAttractedByTrashbin = true;
                attractedByTrashbinTime = Time.time;
                trashbinLocation = col.transform;
                return;
            }
            
        }
        isAttractedByTrashbin = false;

    }

    //Throw trash

    private void PlaceTrashInTrashbin()
    {
        move(trashbinLocation.position);
        float distance = Vector3.Distance(transform.position, trashbinLocation.position);
        if (distance <= minDistanceToTrashbin)
        {
            stay();

            Instantiate(nextThrowTrashPrefab, trashbinLocation.position + Vector3.up * 3f, Quaternion.identity).GetComponent<Rigidbody>().isKinematic = false;

            thrown = true;
            isPatrolling = false;

            //Set next time and item to throw
            prepareNextTrash();

            if (troublemaker && Random.Range(0f, 1f) < chanceToKnockdownTrashbin && !trashbin.isDown)
            {
                trashbin.KnockDown();
                troubleMakingSound.Play();
            }
        }
    }


    private void PlaceTrashNear()
    {
        if (!thrown)
        {
            stay();
            //Find grid position
            var finalPosition = grid.GetNearestPointOnGrid(transform.position);
            Collider[] overlappedObjects = Physics.OverlapSphere(finalPosition, radius, nonGroundMask);
            if (grid.isGridFree((int)finalPosition.x, (int)finalPosition.z)&& (overlappedObjects.Length == 0 || (overlappedObjects.Length==1&& overlappedObjects[0].name=="Terrain")))
            {
                finalPosition.y = trash_y;
                GameObject trash = Instantiate(nextThrowTrashPrefab, finalPosition, nextThrowTrashPrefab.transform.rotation);
                grid.setGridStatus(finalPosition.x, finalPosition.z);
            } 
              
            thrown = true;
            
            prepareNextTrash();
        }
        
    }


    private void prepareNextTrash()
    {
        //Set next time and item to throw
        nextThrowTime = Time.time + Random.Range(minThrowInterval, maxThrowInterval)
            +Mathf.Clamp(currThrowIntervalChange, minThrowIntervalChange, maxThrowIntervalChange);

        //Random Generate Trash type
        int idx = Random.Range(0, trash_range);
        if (trashNeedHigherY.Contains(idx))
            trash_y = 0.6f;
        else
            trash_y = 0.3f;
        nextThrowTrashPrefab = trashPrefabs[idx];
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
        StartCoroutine(chosePatrolLocation());
    }

    

    public void setGameController(GameController _gameController)
    {
        game = _gameController;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Claw") && Random.Range(0f, 1f) <0.3)
            hurtSound.Play();
    }


    //Movement ====================================
    public void move(Vector3 pos)
    {
        nullTarget();
        agent.speed = walkSpeed;
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    public void moveToTarget()
    {
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
