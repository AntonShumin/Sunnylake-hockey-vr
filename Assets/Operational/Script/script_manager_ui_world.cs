using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class script_manager_ui_world : MonoBehaviour {

    private script_manager_gameplay_cannon m_manager_gameplay_cannon;
    private GameObject m_count_positive;
    private GameObject m_count_negative;
    private GameObject m_big_title;
    private GameObject m_countdown;

    private string m_last_event = "";
    private string m_timer_event_string = "";
    private bool m_timer_active = false;
    private float m_timer_value = 0f;

    private GameObject m_ui_main;
    private GameObject m_summary;
    private GameObject[] m_summary_scores = new GameObject[9];


    void Awake()
    {
        m_count_positive = transform.Find("Count_positive").gameObject;
        m_count_negative = transform.Find("Count_negative").gameObject;
        m_big_title = transform.Find("Big_title").gameObject;
        m_countdown = transform.Find("Countdown").gameObject;
        m_manager_gameplay_cannon = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay_cannon>();

        m_summary = GameObject.Find("Game Summary");
        m_summary_scores[0] = GameObject.Find("sum_score");
        m_summary_scores[1] = GameObject.Find("sum_saves");
        m_summary_scores[2] = GameObject.Find("sum_allowed");
        m_summary_scores[3] = GameObject.Find("sum_time");
        m_summary_scores[4] = GameObject.Find("sum_games");
        m_summary_scores[5] = GameObject.Find("sum_total_saves");
        m_summary_scores[6] = GameObject.Find("sum_best");
        m_summary_scores[7] = GameObject.Find("sum_record");
        m_summary_scores[8] = GameObject.Find("sum_position");

        //UI Main
        m_ui_main = GameObject.Find("User Interface");
    }

	// Use this for initialization
	void Start () {
        m_count_positive.SetActive(false);
        m_count_negative.SetActive(false);
        m_big_title.SetActive(false);
        m_countdown.SetActive(false);
        m_summary.SetActive(false);
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
            case ("cannon"):

                Show_Giant_Text("warm up", 1, "cannon fire");
                break;
            case ("cannon fire"):
                Reset_scores();
                m_count_positive.SetActive(true);
                m_count_negative.SetActive(true);
                m_manager_gameplay_cannon.Game_Event(event_name);
                break;
            case ("replay cannon"):
                m_summary.SetActive(false);
                m_manager_gameplay_cannon.Game_Event("cannon");
                break;
            case ("return from summary"):
                break;
            case ("giant text finish - cannon"):
                m_manager_gameplay_cannon.Game_Event(event_name);
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
            m_big_title.SetActive(false);
            m_countdown.SetActive(false);
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

    }

    public void Count_Save(int score)
    {
        m_count_positive.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    public void Count_Score(int score)
    {
        m_count_negative.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    public void Show_Summary(string type, string[] score_array)
    {
        if(type == "cannon")
        {
            for( int i = 0; i < score_array.Length; i++ )
            {
                m_summary_scores[i].GetComponent<Text>().text = score_array[i];

            }
        }
        m_summary.SetActive(true);
    }

    public void Show_main_ui()
    {
        m_ui_main.SetActive(true);
    }

   

    


}
