using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;

public class SupervisorController : MonoBehaviour
{
    private Animator anim;
    public GameController game;

    [Header("Patrol")]
    [SerializeField] Vector3 boxDimension = new Vector3(10,10,10);
    [SerializeField] float radius = 10f;
    public LayerMask trashMask;
    [SerializeField] int maxTrashCount = 15;
    

    private bool isIssuingWarning = false;
    [SerializeField] float stayTime = 3f;
    private float nextPatrolTime;
    public AudioSource warningSound;
    public Text warningText;
    [SerializeField] float warningPeriod = 5;
    private float nextWarningTime;
    public ParticleSystem warningRing;


    //Motor
    [Header("Movement")]
    private NavMeshAgent agent;
    [SerializeField] float walkSpeed = 8f;
    public List<Transform> carPathNodes;
    private int currentNode = 0;
    public AudioSource movementSound;

    //Debug
    public int trashCount;
    public float distance;

    

    // Use this for initialization

    void Start()
    {
        game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        movementSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!game.isGameFinished)
        {
            if (!isIssuingWarning)
            {
                Patrol();
                if (Time.time > nextWarningTime)
                    checkTrashDensity();
            }
            else
            {
                if (Time.time >= nextPatrolTime)
                {
                    
                    isIssuingWarning = false;
                    warningText.gameObject.SetActive(false);
                    warningRing.gameObject.SetActive(false);
                    nextWarningTime = Time.time + warningPeriod;
                }
            }
            if (game.isGameFinished)
            {
                warningSound.Stop();
            }
        } 
        

    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + Vector3.down * 10, radius);
    }

    private void Patrol()
    {
        moveToTarget(carPathNodes[currentNode]);
        Vector3 currentPosition = transform.position;
        currentPosition.y = carPathNodes[currentNode].position.y;
        distance = Vector3.Distance(currentPosition, carPathNodes[currentNode].position);
        if (distance < 2.5f)
        {
            if (currentNode < carPathNodes.Count - 1)
            {
                currentNode += 1;
            }
            else
            {
                //Reach last node, stop patrol
                stop();
                currentNode = 0;
                gameObject.SetActive(false);
                
            }
        }
    }

    private void checkTrashDensity()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + Vector3.down*10, radius,  trashMask);
        trashCount = hitColliders.Length;
        if (trashCount >= maxTrashCount)
        {
            //Issue warning
            stay();
            game.IssueWarning();
            isIssuingWarning = true;
            nextPatrolTime = Time.time + stayTime;
            warningSound.Play((ulong)stayTime);
            warningText.gameObject.SetActive(true);
            warningRing.gameObject.SetActive(true);
        }
    }

    public void moveToTarget(Transform targetPosition)
    {
        agent.isStopped = false;
        agent.speed = walkSpeed;
        agent.SetDestination(targetPosition.position);
    }

    public void stay()
    {
        agent.speed = 0f;
    }

    public void stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
}

