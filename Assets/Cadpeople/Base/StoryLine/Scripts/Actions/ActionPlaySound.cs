using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cadpeople.Storyline;


public class ActionPlaySound : IAction
{


    public AudioClip audioclip;

    protected AudioSource audiosource;


    public override void Execute()
    {
        if (!audiosource) audiosource = GetComponent<AudioSource>();
        if (!audiosource) audiosource = gameObject.AddComponent<AudioSource>();

        audiosource.clip = audioclip;
        audiosource.Play();


    }


    

}
