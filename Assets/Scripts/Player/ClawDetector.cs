using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawDetector : MonoBehaviour
{
    public MyGrid grid;
    public Claw claw;
    public List<Trash> itemsOnhold;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (claw.fired )
        {
            if(other.gameObject.CompareTag("Trash"))
            {
                Trash trash = other.transform.GetComponent<Trash>();
                if (!itemsOnhold.Contains(trash))
                {
                    TrashStats trashStats = trash.trashStats;
                    if (!trashStats.needWipe)
                    {
                        claw.hooked = true;
                        grid.setGridStatus(other.transform.position.x, other.transform.position.y);
                        other.transform.parent = transform;
                        itemsOnhold.Add(trash);
                    }
                }
            } else
            {
                claw.returning = true;
            }
            
        }
    }

}
