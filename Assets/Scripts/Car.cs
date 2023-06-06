using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    //Motor
    private NavMeshAgent agent;
    public List<Transform> carPathNodes;
    public float driveSpeed = 2f;
    private int currentNode = 0;
    public AudioSource carSound;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        moveToTarget(carPathNodes[currentNode]);
        Vector3 currentPosition = transform.position;
        currentPosition.y = 0;
        if (Vector3.Distance(currentPosition, carPathNodes[currentNode].position)<1f)
        {
            if (currentNode < carPathNodes.Count - 1)
            {
                currentNode += 1;
            } else
            {
                currentNode = 0;
            }
        }
    }

    //private void FixedUpdate()
    //{
    //    checkPlayerInRange();
    //}

    ////Check Player in range
    //private void checkPlayerInRange()
    //{
    //    foreach (Collider col in Physics.OverlapSphere(transform.position, maxPlayerRange, playerMask))
    //    {
    //        carSound.Play();
    //        break;
    //    }

    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            carSound.Play();
        }
    }

    public void moveToTarget(Transform targetPosition)
    {
        //agent.isStopped = false;
        agent.speed = driveSpeed;
        agent.SetDestination(targetPosition.position);
    }

    public void stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

}
