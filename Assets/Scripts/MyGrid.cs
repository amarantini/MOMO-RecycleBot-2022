using UnityEngine;
using System.Collections.Generic;

public class MyGrid : MonoBehaviour
{
    [SerializeField]
    private float size = 1f;
    [SerializeField] int maxX = 40;
    [SerializeField] int maxZ = 40;
    [SerializeField] float checkRadius;
    private List<List<bool>> gridStatus; //if occupied true, else false
    public bool findFreeGrid = false;
    public LayerMask layerMask;

    private void Start()
    {
        gridStatus = new List<List<bool>>();
        for (float x = 0; x < maxX; x += size)
        {
            gridStatus.Add(new List<bool>());
            for (float z = 0; z < maxZ; z += size)
            {
                gridStatus[gridStatus.Count - 1].Add(false);

            }
        }
    }

    public bool isGridFree(int x, int z)
    {
        Vector3 position = new Vector3(x, 0, z);
        position -= transform.position;

        int xCoord = Mathf.RoundToInt(position.x / size);
        int zCoord = Mathf.RoundToInt(position.z / size);
        if (xCoord < 0 || xCoord >= gridStatus.Count || zCoord < 0 || zCoord >= gridStatus[0].Count)
            return false;
        return !gridStatus[xCoord][zCoord];
    }

    public void setGridStatus(float x, float z)
    {
        Vector3 position = new Vector3(x, 0, z);
        position -= transform.position;

        int xCoord = Mathf.RoundToInt(position.x / size);
        int zCoord = Mathf.RoundToInt(position.z / size);

        gridStatus[xCoord][zCoord] = !gridStatus[xCoord][zCoord];
    }

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        //int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            0,
            (float)zCount * size);

        result += transform.position;
        result.y = 0;
        return result;
    }

    public Vector3 GetNearestFreePointOnGrid(Vector3 position,int maxSearchRange)
    {
        findFreeGrid = false;

        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        int x = xCount;
        int z = zCount;
        for (; x< xCount + maxSearchRange; x++)
        {
            for (; z < zCount + maxSearchRange; z++)
            {
                if (!gridStatus[x][z])
                {
                    findFreeGrid = true;
                    break;
                }
                    
            }
        }

        Vector3 result = new Vector3(
            (float)x * size,
            0f,
            (float)z * size);

        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = 0; x < maxX; x += size)
        {
            for (float z = 0; z < maxZ; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);

            }

        }
    }
}