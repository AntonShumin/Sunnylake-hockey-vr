using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_manager_audio : MonoBehaviour {

    public AudioClip[] m_clips;

	public void Play_oneshot(AudioSource source,AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void Play_oneshot(AudioSource source,int clip)
    {
        source.PlayOneShot(m_clips[clip]);
        source.loop = true;
    } 

    public void Play_loop(AudioSource source,int clip)
    {
        source.loop = true;
        source.clip = m_clips[clip];
        source.Play();
    }

    
}
