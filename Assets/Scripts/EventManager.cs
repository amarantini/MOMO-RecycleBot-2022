using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;

public class EventManager : MonoBehaviour
{
    public static EventManager current;
    public PlayableDirector director;

    private void Awake()
    {
        if (current != this && current != null)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }
    }

    




    void OnEnable()
    {
        director.stopped += OnPlayableDirectorStopped;
    }

    public Action onCutsceneEnd;
    public void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            //Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            if (onCutsceneEnd != null)
            {
                onCutsceneEnd();
            }
        }
            
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }

}
