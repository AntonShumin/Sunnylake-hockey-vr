using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class script_manager_ui_world : MonoBehaviour {

    public AudioClip[] m_sounds;

    private script_manager_gameplay_cannon m_manager_gameplay_cannon;
    private GameObject m_count_positive;
    private GameObject m_count_negative;
    private GameObject m_big_title;
    private GameObject m_countdown;
    private script_particles m_particles;
    private AudioSource m_sound_source;
    private GameObject m_camera_rig;
    private Vector3 m_camera_rig_ui_offset;

    private string m_last_event = "";
    private string m_timer_event_string = "";
    private bool m_timer_active = false;
    private float m_timer_value = 0f;

    private GameObject m_ui_main;
    private GameObject m_summary;
    private GameObject[] m_summary_scores;
    private GameObject m_summary_record;
    private GameObject[] m_goalie_equipment = new GameObject[7];
    public GameObject[] m_laster_pointer = new GameObject[2];

    //cached 
    private Vector3 c_position;
    private float c_float;


    void Awake()
    {
        m_count_positive = transform.Find("Count_positive").gameObject;
        m_count_negative = transform.Find("Count_negative").gameObject;
        m_big_title = transform.Find("Big_title").gameObject;
        m_countdown = transform.Find("Countdown").gameObject;
        m_manager_gameplay_cannon = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay_cannon>();
        m_particles = GameObject.Find("Particles_UI").GetComponent<script_particles>();
        m_sound_source = GetComponent<AudioSource>();
        

        m_summary = GameObject.Find("Game Summary");
        m_summary_scores = new GameObject[8];
        m_summary_scores[0] = GameObject.Find("sum_score");
        m_summary_scores[1] = GameObject.Find("sum_saves");
        m_summary_scores[2] = GameObject.Find("sum_allowed");
        m_summary_scores[3] = GameObject.Find("sum_time");
        m_summary_scores[4] = GameObject.Find("sum_games");
        m_summary_scores[5] = GameObject.Find("sum_total_saves");
        m_summary_scores[6] = GameObject.Find("sum_position");
        m_summary_scores[7] = GameObject.Find("sum_record");
        //m_summary_scores[8] = GameObject.Find("sum_position");
        m_summary_record = GameObject.Find("sum_popup_record");
        m_summary_record.SetActive(false);

        //UI Main
        m_ui_main = GameObject.Find("User Interface");
        m_camera_rig = GameObject.Find("[CameraRig]").gameObject;
        m_camera_rig_ui_offset = m_ui_main.transform.position - m_camera_rig.transform.position;

        
    }

	// Use this for initialization
	void Start () {
        m_count_positive.SetActive(false);
        m_count_negative.SetActive(false);
        m_big_title.SetActive(false);
        m_countdown.SetActive(false);
        m_summary.SetActive(false);

        //goalie equipment
        m_goalie_equipment[0] = GameObject.Find("model_glove");
        m_goalie_equipment[1] = GameObject.Find("model_glove/Colliders");
        m_goalie_equipment[2] = GameObject.Find("model_goalie_stick_v1");
        m_goalie_equipment[3] = GameObject.Find("model_goalie_stick_v1/collider2");
        m_goalie_equipment[4] = GameObject.Find("model_pad/glove");
        m_goalie_equipment[5] = GameObject.Find("model_pad/IGH_glove_DX");
        m_goalie_equipment[6] = GameObject.Find("model_pad/pad_collider");

        //laser pointer
        /*
        m_laster_pointer[0] = GameObject.Find("ViveControllerLaserPointerLeft");
        m_laster_pointer[1] = GameObject.Find("ViveControllerLaserPointerRight");
        */
        m_laster_pointer[0].SetActive(false);
        m_laster_pointer[1].SetActive(false);


    }

    // Update is called once per frame
    void Update () {
        if (m_timer_active) Timer_rundown();
	}

    public void Game_Events(string event_name)
    {
        m_last_event = event_name;
        switch(event_name)
        {
            case ("reset cannon scores"):
                Reset_scores();
                m_count_positive.SetActive(true);
                m_count_negative.SetActive(true);
                break;
            case ("replay cannon"):
                Hide_Summary();
                m_manager_gameplay_cannon.Game_Event("cannon");
                break;
            case ("home from summary"):
                Hide_Summary();
                Show_main_ui();
                break;
            case ("giant text finish - cannon"):
            case ("next wave start"):
                m_manager_gameplay_cannon.Game_Event(event_name);
                break;
            case ("text hide animation done"):
                m_big_title.GetComponent<DOTweenAnimation>().DORewind();
                m_countdown.GetComponent<DOTweenAnimation>().DORewind();
                m_big_title.SetActive(false);
                m_countdown.SetActive(false);
                break;
            case ("summary hide"):
                m_summary.GetComponent<DOTweenAnimation>().DORewind();
                m_summary.SetActive(false);
                break;
            case ("ui hover sound"):
                m_sound_source.PlayOneShot(m_sounds[4]);
                break;
            case ("button press"):
                m_sound_source.PlayOneShot(m_sounds[5]);
                break;
            case ("particle goal"):
                m_particles.Game_Event("goal");
                break;
            case ("summary record"):
                Summary_Record();
                break;
        }
        
    }


    private void Reset_scores()
    {
        m_count_positive.GetComponent<TextMeshProUGUI>().text = "0";
        m_count_negative.GetComponent<TextMeshProUGUI>().text = "0";
    }

    private void Timer_rundown()
    {
        m_timer_value -= Time.deltaTime;
        if(m_timer_value <= 0 )
        {
            m_timer_value = 0;
            m_timer_active = false;
            m_big_title.GetComponent<DOTweenAnimation>().DOPlayById("hide");
            m_countdown.GetComponent<DOTweenAnimation>().DOPlayById("hide");
            m_sound_source.PlayOneShot(m_sounds[1]);
            m_particles.Game_Event("hover stop");
            if (m_timer_event_string != "")
            {

                Game_Events(m_timer_event_string);
            }

        }
        m_countdown.GetComponent<TextMeshProUGUI>().text = m_timer_value.ToString("f2");

    }

    public void Show_Giant_Text(string text, float time, string timer_event_string)
    {

        m_timer_event_string = timer_event_string;
        m_big_title.SetActive(true);
        m_countdown.SetActive(true);
        m_big_title.GetComponent<TextMeshProUGUI>().text = text;
        m_timer_value = time;
        m_timer_active = true;
        m_sound_source.PlayOneShot(m_sounds[0]);
        m_big_title.GetComponent<DOTweenAnimation>().DOPlayById("show");
        m_countdown.GetComponent<DOTweenAnimation>().DOPlayById("show");

        //particles
        m_particles.Game_Event("hover start");

    }

    public void Count_Save(int score)
    {
        m_sound_source.PlayOneShot(m_sounds[6]);
        m_count_positive.GetComponent<TextMeshProUGUI>().DOText(score.ToString(), 0.8f, true, ScrambleMode.All);
    }

    public void Count_Score(int score)
    {
        m_sound_source.PlayOneShot(m_sounds[7]);
        m_count_negative.GetComponent<TextMeshProUGUI>().DOText(score.ToString(),0.8f,true,ScrambleMode.All);
    }

    public void Show_Summary(string type, string[] score_array)
    {
        if(type == "cannon")
        {
            for( int i = 0; i < score_array.Length - 1; i++ )
            {
                m_summary_scores[i].GetComponent<Text>().text = score_array[i];

            }
        }
        Show_Summary();
    }

    public void Show_main_ui()
    {
        c_position = m_camera_rig.transform.position + m_camera_rig_ui_offset;
        m_ui_main.transform.position = c_position;
        c_position.y += 100;
        m_ui_main.SetActive(true);
        m_ui_main.transform.DOMove(c_position,0.6f).From().SetEase(Ease.OutExpo);
        m_sound_source.PlayOneShot(m_sounds[2]);

        //equipment
        Equipment_Enable(false);


    }

    public void Hide_main_ui()
    {
        c_position = m_ui_main.transform.position;
        c_position.y += 100;
        m_ui_main.transform.DOMove(c_position, 0.6f).SetEase(Ease.InExpo).OnComplete(Hide_main_ui_complete);
        m_sound_source.PlayOneShot(m_sounds[3]);

        //equipment
        Equipment_Enable(true);
    }

    private void Hide_main_ui_complete()
    {
        m_ui_main.SetActive(false);
    }

    public void Show_Summary()
    {
        //m_particles.Game_Event("onetimer");
        m_sound_source.PlayOneShot(m_sounds[2]);
        m_summary.SetActive(true);
        m_summary.GetComponent<DOTweenAnimation>().DOPlayById("show");

        //equipment
        Equipment_Enable(false);
    }

    public void Hide_Summary()
    {
        m_summary.GetComponent<DOTweenAnimation>().DOPlayById("hide");
        m_sound_source.PlayOneShot(m_sounds[3]);

        //stop record
        m_summary_record.SetActive(false);
        m_particles.Game_Event("summary record stop");

        //equipment
        Equipment_Enable(true);

    }

    private void Summary_Record()
    {
        m_summary_record.SetActive(true);
        m_summary_record.GetComponent<DOTweenAnimation>().DORewind();
        m_summary_record.GetComponent<DOTweenAnimation>().DOPlayById("record start");
        m_particles.Game_Event("summary record");

    }

    private void Equipment_Enable(bool type)
    {
        if (type)
        {
            c_float = 0f;
        } else
        {
            c_float = 0.5f;
        }
        //m_goalie_equipment[0].GetComponent<Renderer>().material.SetColor("_Diffusecolor",new Color("#4B97FFFF") );
        m_goalie_equipment[0].GetComponent<Renderer>().material.SetFloat("_Transparency", c_float);
        m_goalie_equipment[1].SetActive(type);
        m_goalie_equipment[2].GetComponent<Renderer>().material.SetFloat("_Transparency", c_float);
        m_goalie_equipment[3].SetActive(type);
        m_goalie_equipment[4].GetComponent<Renderer>().material.SetFloat("_Transparency", c_float);
        m_goalie_equipment[5].GetComponent<Renderer>().material.SetFloat("_Transparency", c_float);
        m_goalie_equipment[6].SetActive(type);

        //pointer
        foreach (GameObject go in m_laster_pointer)
        {
            go.SetActive(!type);
        }
    }


   

    


}
