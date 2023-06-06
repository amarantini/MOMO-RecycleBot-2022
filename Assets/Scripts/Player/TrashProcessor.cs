using UnityEngine;
using System.Collections;

public class TrashProcessor : MonoBehaviour
{
    public bool isRecyclable;
    public PlayerController player;
    public AudioSource wrongSound;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        TrashStats trash = other.gameObject.GetComponent<Trash>().trashStats;
        if(trash.isRecyclable && isRecyclable)
        {
            player.GetRecyclable(trash);
        } else if (!trash.isRecyclable && !isRecyclable)
        {
            player.GetOtherTrash();
        } else
        {
            wrongSound.Play();
            player.updateCoin(-2);
        }
        Destroy(other.gameObject);
    }
}
