using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    [SerializeField] float spinSpeed = 1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
