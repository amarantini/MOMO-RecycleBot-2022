using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public Transform Player;
    public Transform Obstruction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        ViewObstructed();
    }

    void ViewObstructed()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, Player.position - transform.position , Color.green);
        if (Physics.Raycast(transform.position, Player.position - transform.position, out hit, 4.5f)
            && hit.collider.gameObject.tag != "Player"
            && hit.collider.gameObject.tag != "ChargingStation")
        {
            Obstruction = hit.transform;
            Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            //if (Vector3.Distance(Obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5f)
            //    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
  
        } else if (Obstruction!=null)
        {
            Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            //if (Vector3.Distance(transform.position, Target.position) < 4f)
            //    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
        }
    }



}
