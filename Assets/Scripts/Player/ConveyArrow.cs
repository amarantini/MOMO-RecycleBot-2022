using UnityEngine;
using System.Collections;

public class ConveyArrow : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float minY = -2.7f;
    [SerializeField] float maxY = 4.7f;
    private Vector3 newPosition;


    // Update is called once per frame
    void Update()
    {    
        newPosition = transform.localPosition + Vector3.down * speed * Time.deltaTime;
        if (newPosition.y < minY)
        {
            newPosition.y = maxY;
        }
        transform.localPosition = newPosition;
    }

}
