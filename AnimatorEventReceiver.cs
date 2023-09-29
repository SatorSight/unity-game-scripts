using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventReceiver : MonoBehaviour 
{   
    public event Action<AnimationEvent> OnFootstep;

    void OnFootstepa(AnimationEvent animationEvent) 
    {
        Debug.Log("PrintEvent: " + animationEvent + " called at: " + Time.time);
    }      
}