using UnityEngine;
using System.Collections;

public class Spin2d : MonoBehaviour
{
    [SerializeField] float spinSpeed = 10;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f,spinSpeed * Time.deltaTime);
    }
}
