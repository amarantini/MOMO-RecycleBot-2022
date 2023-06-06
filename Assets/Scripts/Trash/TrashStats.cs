using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Trash/trashStats")]
public class TrashStats : ScriptableObject
{
    public string trashName;
    public bool isRecyclable;
    public int sellingPrice;
    public bool needWipe;
    public float wipeTime;
}
