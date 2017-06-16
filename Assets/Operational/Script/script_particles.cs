using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_particles : MonoBehaviour {

    private ParticleSystem m_text_hover;
    private ParticleSystem m_text_onetimer;
    private WaitForSeconds m_text_wait = new WaitForSeconds(0.5f);

    void Awake()
    {
        m_text_hover = transform.FindChild("Text_Hover").GetComponent<ParticleSystem>();
        m_text_onetimer = transform.FindChild("Text_Onetimer").GetComponent<ParticleSystem>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
                m_text_onetimer.Play();
                break;

        }
    }

    IEnumerator Text_Wait()
    {

        yield return m_text_wait;
        Game_Event("hover play");
    }
}
