using UnityEngine;
using System.Collections;

public class ConveyTrash : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float minY = -5.2f;
    private Vector3 newPosition;

    [SerializeField] float horizontalMove = 2;
    public bool isLeft = true;

    public bool isConveying = true;

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isConveying)
        {
            newPosition = transform.localPosition + Vector3.down * speed * Time.deltaTime;
            if (newPosition.y >= minY)
            {
                transform.localPosition = newPosition;
            } else
            {
                rb2d.gravityScale = 1;
            }
            
        }
        
    }

    public void MoveTrash()
    {
        newPosition = transform.localPosition + Vector3.down * speed * Time.deltaTime;
        if (isLeft)
            newPosition.x = -2;
        else
            newPosition.x = 2;
        rb2d.gravityScale = 1;
        isConveying = false;
        transform.localPosition = newPosition;
    }
}
