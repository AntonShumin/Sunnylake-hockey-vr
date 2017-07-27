using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticlePlayground;


public class script_particles : MonoBehaviour
{

    //set vars
    private ParticleSystem m_text_hover;
    private ParticleSystem m_text_onetimer;
    private WaitForSeconds m_text_wait = new WaitForSeconds(0.5f);

    //particle playground
    private PlaygroundParticlesC m_goal;
    private PlaygroundParticlesC m_block;
    private PlaygroundParticlesC[] m_hots = new PlaygroundParticlesC[4];
    private AudioSource[] m_hots_audioSource = new AudioSource[4];
    private VRTK.script_grabbable[] m_hots_grabbable = new VRTK.script_grabbable[4];
    private script_manager_audio m_manager_audio;

    void Awake()
    {
        m_text_hover = transform.FindChild("Text_Hover").GetComponent<ParticleSystem>();
        m_text_onetimer = transform.FindChild("Text_Onetimer").GetComponent<ParticleSystem>();
    }

    // Use this for initialization
    void Start()
    {

        m_goal = GameObject.Find("particles_goal").GetComponent<PlaygroundParticlesC>();
        m_hots[1] = GameObject.Find("highlight_stick").GetComponent<PlaygroundParticlesC>();
        m_hots[2] = GameObject.Find("highlight_pad").GetComponent<PlaygroundParticlesC>();
        m_hots[3] = GameObject.Find("highlight_glove").GetComponent<PlaygroundParticlesC>();

        //hot Audio source
        m_hots_audioSource[1] = GameObject.Find("model_goalie_stick_v1").GetComponent<AudioSource>();
        m_hots_audioSource[2] = GameObject.Find("Goalie_Pad_Hand").GetComponent<AudioSource>();
        m_hots_audioSource[3] = GameObject.Find("dummy_glove").GetComponent<AudioSource>();

        //hot haptics
        m_hots_grabbable[1] = m_hots_audioSource[2].GetComponent<VRTK.script_grabbable>(); // stick and pad share the same grabbable
        m_hots_grabbable[2] = m_hots_audioSource[2].GetComponent<VRTK.script_grabbable>();
        m_hots_grabbable[3] = m_hots_audioSource[3].GetComponent<VRTK.script_grabbable>();

        //
        m_manager_audio = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_audio>();

    }

    public void Game_Event(string event_name)
    {
        switch (event_name)
        {
            case "hover start":
                StartCoroutine(Text_Wait());
                break;
            case "hover play":
                m_text_hover.Play();
                m_text_hover.GetComponent<AudioSource>().Play();
                m_text_onetimer.Play();
                break;
            case "hover stop":
                m_text_hover.Stop();
                m_text_hover.GetComponent<AudioSource>().Stop();
                break;
            case "onetimer":
                m_text_onetimer.Play();
                break;
            case "goal":
                m_goal.Emit(true);
                break;

        }
    }

    public void Hot_highlight(int index, bool onoff)
    {
        m_hots[index].Emit(onoff);
        if (onoff == true)
        {
            m_manager_audio.Play_loop(m_hots_audioSource[index], 0);
            m_manager_audio.Play_oneshot(m_hots_audioSource[index], 2);
            m_hots_grabbable[index].Vibrate(1f, 2f);
        } else
        {
            m_hots_audioSource[index].Stop();
            m_manager_audio.Play_oneshot(m_hots_audioSource[index], 2);
            m_hots_grabbable[index].Vibrate(0.5f, 0.5f);
        }

    }

    IEnumerator Text_Wait()
    {

        yield return m_text_wait;
        Game_Event("hover play");
    }
}

