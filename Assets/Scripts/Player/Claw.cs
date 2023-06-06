using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Claw : MonoBehaviour
{
    public GameObject claw;
    public GameObject clawHolder;
    public AudioSource shootClawSound;

    public float clawTravelSpeed;

    public bool fired;
    public bool hooked;
    public bool returning = false;

    public float maxDistance;
    private float currentDistance;

    public ClawDetector clawDetector;
    public AudioSource collectTrashSound;

    public PlayerController player;

    public Transform ropePosition;
    public LineRenderer rope;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        rope = claw.GetComponent<LineRenderer>();
        rope.positionCount = 2;
        rope.SetPosition(0, ropePosition.position);
        rope.SetPosition(1, claw.transform.position);

        claw.GetComponent<BoxCollider>().isTrigger = false;
    }


    // Update is called once per frame
    void Update()
    {
        //firing the claw
        if (Input.GetKey(KeyCode.Space) && fired == false)//Keyboard.current.spaceKey.wasPressedThisFrame
        {
            fired = true;
            claw.GetComponent<BoxCollider>().isTrigger = true;
            shootClawSound.Play();
        }

        //if (fired)
        //{
            
        rope.positionCount = 2;
        rope.SetPosition(0, ropePosition.position);
        rope.SetPosition(1, claw.transform.position);
            
        
        if (returning)
        {
            claw.transform.position = Vector3.MoveTowards(claw.transform.position, clawHolder.transform.position, clawTravelSpeed * Time.deltaTime);
            float distance = Vector3.Distance(clawHolder.transform.position, claw.transform.position);
            if (distance < 1f)
            {
                ReturnClaw();
            }
        } else if (fired&!hooked)
        {
            claw.transform.Translate(Vector3.forward * Time.deltaTime * clawTravelSpeed);
            currentDistance = Vector3.Distance(transform.position, claw.transform.position);
            if (currentDistance >= maxDistance)
            {
                returning = true;
            }
        }
        if (hooked)
        {
            claw.transform.position = Vector3.MoveTowards(claw.transform.position, clawHolder.transform.position, clawTravelSpeed*Time.deltaTime);
            float distance = Vector3.Distance(clawHolder.transform.position, claw.transform.position);
            if (distance < 1f)
            {
                ReturnClaw();
                for(int i=0; i< clawDetector.itemsOnhold.Count; i++)
                {
                    Trash item = clawDetector.itemsOnhold[i];
                    if (item != null)
                    {
                        if (player.isAutoGameMode)
                        {
                            if (!item.trashStats.isRecyclable)
                            {
                                collectTrashSound.Play();
                            }
                            else
                            {
                                player.GetRecyclable(item.trashStats);
                            }
                        } else
                        {
                            if (item.trashStats.trashName == "Trash Bomb")
                            {
                                player.GetRecyclable(item.trashStats);
                            }
                            else
                            {
                                player.ProcessTrash(item.trashStats);
                            }
                        }
                        Destroy(item.gameObject);
                    }
                }
                clawDetector.itemsOnhold.Clear();
                hooked = false;
            }
        }
    }

    public IEnumerator ProcessTrash(TrashStats trash)
    {
        yield return new WaitForSeconds(0.5f);
        player.ProcessTrash(trash);
    }

    void ReturnClaw()
    {
        claw.transform.position = clawHolder.transform.position;
        fired = false;
        returning = false;
        claw.GetComponent<BoxCollider>().isTrigger = false;
    }

    public void upgradeClaw(float upgradeFactor)
    {
        maxDistance *= upgradeFactor;
    }
}
