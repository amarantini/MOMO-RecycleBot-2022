using UnityEngine;
using System.Collections;

public class MouseFollow : MonoBehaviour
{
    public PlayerController player;
    public MyGrid grid;
    public LayerMask groundMask;
    public bool isPlaceEmpty = true;
    private RaycastHit hit;
    private Ray ray;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<MyGrid>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        Vector3 mouseInput = Input.mousePosition;//player.playerInputActions.Player.MousePosition.ReadValue<Vector2>();
        ray = Camera.main.ScreenPointToRay(mouseInput);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask)){
            Vector3 newPosition = grid.GetNearestPointOnGrid(hit.point);
            newPosition.y = 0.3f;
            transform.position = newPosition;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Ground") && !other.gameObject.CompareTag("ChargingStation"))
        {
            isPlaceEmpty = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Ground") && !other.gameObject.CompareTag("ChargingStation"))
        {
            isPlaceEmpty = true;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!collision.gameObject.CompareTag("Ground"))
    //    {
    //        isPlaceEmpty = false;
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (!collision.gameObject.CompareTag("Ground"))
    //    {
    //        isPlaceEmpty = true;
    //    } 
    //}
}
