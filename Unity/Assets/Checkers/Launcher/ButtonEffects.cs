using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add Hover, Press, (but not DEpress) effects to a button
/// </summary>
/// <see href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseOver.html">MonoBehaviour.OnMouseOver()</see>
/// Debugging: <see href="https://forum.unity.com/threads/onmouseover-not-working.230829/">OnMouseOver not working</see>
/// Debugging: <see href="https://stackoverflow.com/questions/56418801/unity-onmouseenter-or-onmouseover-not-working">Unity OnMouseEnter() or OnMouseOver() not working</see>
[RequireComponent(typeof(AudioSource))]
public class ButtonEffects : MonoBehaviour
{

    [Header("Button Audio")]
    public AudioSource buttonAudio;
    public AudioClip audioHover;
    public AudioClip audioBack;

    void OnMouseOver()
    {
        Debug.Log("HOVER");
        buttonAudio.PlayOneShot(audioHover, 1F);
    }

    void OnMouseExit()
    {
        Debug.Log("NO hover");
        buttonAudio.PlayOneShot(audioBack, 1F);
    }
}
