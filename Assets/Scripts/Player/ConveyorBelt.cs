using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
{
    public List<GameObject> trash2dPrefabs;
    public List<string> trashNames;
    public Transform spawnPosition;
    private int trashIdx;
    public bool isLeft = true;
    public List<ConveyTrash> trash2ds = new List<ConveyTrash>();

    //Spit trash
    public List<GameObject> trashPrefabs;
    [SerializeField] float spitForce = 0.1f;
    public AudioSource wrongSound;

    public PlayerController player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trash2ds.Count > 0)
        {
            if (trash2ds[0] == null)
            {
                trash2ds.RemoveAt(0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isLeft = true;
                ProcessTrash();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                isLeft = false;
                ProcessTrash();
            }
        }
    }

    public void GetTrash(TrashStats trash)
    {
        trashIdx = trashNames.IndexOf(trash.trashName);
        if(trashIdx != -1)
        {
            GameObject trash2d = Instantiate(trash2dPrefabs[trashIdx], spawnPosition);
            ConveyTrash cd = trash2d.GetComponent<ConveyTrash>();
            cd.isLeft = isLeft;
            trash2ds.Add(cd);
        }
            
    }

    public void ProcessTrash()
    {
        trash2ds[0].isLeft = isLeft;
        trash2ds[0].MoveTrash();
        trash2ds.RemoveAt(0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            //Spit trash
            Trash trash = collision.gameObject.GetComponent<Trash>();
            int idx = trashNames.IndexOf(trash.trashStats.trashName);
            Vector3 spawnPosition = player.transform.position + player.transform.forward*4;
            GameObject newTrash = Instantiate(trashPrefabs[idx], spawnPosition, Quaternion.identity);
            newTrash.GetComponent<Rigidbody>().isKinematic = false;
            newTrash.GetComponent<Rigidbody>().AddForce((player.transform.forward + Vector3.up) * spitForce,ForceMode.VelocityChange);

            //Penalize player
            wrongSound.Play();
            player.updateCoin(-1);

            Destroy(collision.gameObject);
            
        }
    }

}
