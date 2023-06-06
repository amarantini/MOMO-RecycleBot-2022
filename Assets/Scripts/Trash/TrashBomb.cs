using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBomb : MonoBehaviour
{
    [SerializeField] float life = 60f;

    public GameObject explosionPrefab;
    public GameObject explosionSoundPrefab;
    [SerializeField] float radius = 1f;
    [SerializeField] float explosionForce = 0.1f;
    public List<GameObject> trashPrefabs;
    [SerializeField] int maxTrashNum = 2;
    [SerializeField] int minTrashNum = 5;

    public LayerMask trashMask;

    private MyGrid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.FindWithTag("Grid").GetComponent<MyGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0)
        {
            Bomb();
        }
        
    }

    public void Bomb()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioSource explosionSound = Instantiate(explosionSoundPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
        explosionSound.Play();
        //game.decreaseTime();
        int trashNum = Random.Range(minTrashNum, maxTrashNum);
        for (int i = 0; i < trashNum; i++)
        {
            Instantiate(trashPrefabs[Random.Range(0, trashPrefabs.Count)], transform.position, Quaternion.identity);
        }
        Explode();
        Destroy(explosionSound.gameObject, 3);
        grid.setGridStatus(transform.position.x, transform.position.y);
        Destroy(gameObject);
    }

    void Explode()
    {
        //Get nearby objects
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius,trashMask);
        foreach (Collider nearbyObject in collidersToMove)
        {
            //Add force
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(explosionForce, transform.position, radius);
            }
        }
    }
}
