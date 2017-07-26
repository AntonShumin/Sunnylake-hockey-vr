using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParticlePlayground;

public class script_particles : MonoBehaviour {

    //set vars
    private ParticleSystem m_text_hover;
    private ParticleSystem m_text_onetimer;
    private WaitForSeconds m_text_wait = new WaitForSeconds(0.5f);

    //particle playground
    private PlaygroundParticlesC m_goal;
    private PlaygroundParticlesC m_block;
    private PlaygroundParticlesC[] m_hots = new PlaygroundParticlesC[4];

    void Awake()
    {
        m_text_hover = transform.FindChild("Text_Hover").GetComponent<ParticleSystem>();
        m_text_onetimer = transform.FindChild("Text_Onetimer").GetComponent<ParticleSystem>();
    }

	// Use this for initialization
	void Start () {

        m_goal = GameObject.Find("particles_goal").GetComponent<PlaygroundParticlesC>();
        m_hots[1] = GameObject.Find("highlight_stick").GetComponent<PlaygroundParticlesC>();
        m_hots[2] = GameObject.Find("highlight_pad").GetComponent<PlaygroundParticlesC>();
        m_hots[3] = GameObject.Find("highlight_glove").GetComponent<PlaygroundParticlesC>();

    }

    public void Game_Event(string event_name)
    {
        switch (event_name)
        {
            case "hover start":
                StartCoroutine( Text_Wait() );
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
    }

    IEnumerator Text_Wait()
    {

        yield return m_text_wait;
        Game_Event("hover play");
    }
}
