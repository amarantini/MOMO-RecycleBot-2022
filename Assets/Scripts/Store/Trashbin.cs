using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbin : MonoBehaviour
{
    public float range = 5;
    public int sellPrice = 10;
    private PlayerController player;
    public bool isDown = false;
    public bool isBought = false;
    public bool isPlaced = false;
    DrawCircle dc;
    LineRenderer lr;
    MouseFollow mf;

    public GameObject buttons;
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        dc = GetComponent<DrawCircle>();
        lr = GetComponent<LineRenderer>();
        mf = GetComponent<MouseFollow>();
        //canvas.worldCamera = GameObject.FindGameObjectWithTag("UI Camera").GetComponent<Camera>();
        canvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (buttons.gameObject.activeSelf)
            buttons.transform.forward = -Camera.main.transform.forward;
    }

    private void OnMouseDown() //show or hide circle
    {
        if (isBought)
        {
            if (isPlaced)
            {
                dc.enabled = !dc.enabled;
                lr.enabled = !lr.enabled;
                buttons.gameObject.SetActive(!buttons.gameObject.activeSelf);
            }
            else
            {
                if (mf.isPlaceEmpty)
                {
                    mf.enabled = false;
                    isPlaced = true;
                    dc.enabled = false;
                    lr.enabled = false;
                    buttons.gameObject.SetActive(false);
                }
            }
        }
        
        
    }

    public void onMoveButton()
    {
        mf.enabled = true;
        isPlaced = false;
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public void onDeleteButton()
    {
        player.coinCount += sellPrice;
        player.coinCountText.text = player.coinCount.ToString();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isPlaced)
        {
            if (isDown && collision.gameObject.CompareTag("Player"))
            {
                isDown = false;
                transform.Rotate(-90, 0, 0);
            }
            if (!isDown && collision.gameObject.CompareTag("Trash"))
            {
                Trash trash = collision.transform.GetComponent<Trash>();
                if (trash.trashStats.isRecyclable)
                    player.GetRecyclable(trash.trashStats);
                Destroy(collision.gameObject, 1f);

            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlaced)
        {
            if (isDown && collision.gameObject.CompareTag("Player"))
            {
                isDown = false;
                transform.Rotate(-90, 0, 0);
            }
            if (!isDown && collision.gameObject.CompareTag("Trash"))
            {
                Trash trash = collision.transform.GetComponent<Trash>();
                if (trash.trashStats.isRecyclable)
                    player.GetRecyclable(trash.trashStats);
                Destroy(collision.gameObject, 1f);

            }
        }

    }

    public void KnockDown()
    {
        isDown = true;
        transform.Rotate(90, 0, 0);
    }

}
