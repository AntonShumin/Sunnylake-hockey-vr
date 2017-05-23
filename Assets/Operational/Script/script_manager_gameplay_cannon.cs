using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_manager_gameplay_cannon : MonoBehaviour {

    public enum GameState { None, Active, Shooting };
    private GameState m_game_state = GameState.None;

    private script_manager_ui_world m_ui_world;
    private script_cannon m_cannon_easy;

    //puck management
    public float m_shoot_frequency = 1.4f;
    public GameObject m_puck_prefab;
    private Rigidbody[] m_pucks = new Rigidbody[5];
    private int m_current_puck = 0;
    
    private IEnumerator shoot_coroutine;


    //other
    private script_cannon m_selected_cannon;
    private Collider m_goal_collider_exit;

    //Scores
    private int m_score_positive;
    private int m_score_negative;
    public int m_wave;
    public int m_wave_shots_left;

    private int m_stats_saves;
    private int m_stats_allowed;
    private float m_stats_time;
    private int m_stats_games_played;
    private int m_stats_saves_total;
    private int m_stats_best;
    private int m_stats_record;
    private int m_stats_world;

    //cached
    private string[] score_array = new string[9];
    



    void Awake()
    {
        m_ui_world = GameObject.Find("World Canvas").GetComponent<script_manager_ui_world>();
        m_cannon_easy = GameObject.Find("Cannon C").GetComponent<script_cannon>();
        
        Setup_Summary_Vars();
    }

    private void Setup_Summary_Vars()
    {
        //visible vars
        m_score_positive = 0;
        m_score_negative = 0;
        m_stats_saves = 0;
        m_stats_allowed = 0;
        m_stats_time = 0;
        m_stats_games_played = 100;
        m_stats_saves_total = 200;
        m_stats_best = 250;
        m_stats_record = 300;
        m_stats_world = 8;

        //silent vars
        m_wave = 1;
        m_wave_shots_left = 100;
    }

    void Start()
    {
        Game_Event("setup summary");
        for (int i = 0; i < 5; i++)
        {
            m_pucks[i] = GameObject.Instantiate(m_puck_prefab, new Vector3(132,10,127), Quaternion.identity).GetComponent<Rigidbody>();
            //m_pucks[i].gameObject.SetActive(false);
        }
        shoot_coroutine = Shoot_Puck();
        m_goal_collider_exit = GameObject.Find("Collider Exit").GetComponent<Collider>();
    }

    void Update()
    {
        if (m_game_state != GameState.None)
        {
            m_stats_time += Time.deltaTime;
        }
    }

    public void Game_Event(string event_name)
    {
        switch(event_name)
        {
            case ("cannon"):
                m_ui_world.Game_Events(event_name);
                Game_Event("setup summary");
                m_game_state = GameState.Active;
                break;
            case ("cannon fire"):
                m_cannon_easy.Game_Event(event_name);
                m_game_state = GameState.Shooting;
                StartCoroutine(shoot_coroutine);
                break;
            case ("count shots"):
                Count_Shots();
                break;
            case ("setup summary"):
                Setup_Summary_Vars();
                break;
            case ("build summary"):
                Build_Summary_Scores();
                break;
            case ("stop cannon"):
                m_game_state = GameState.Active;
                m_cannon_easy.Game_Event("stop");
                StopAllCoroutines();
                StopCoroutine(shoot_coroutine);
                break;
            case ("end wave"):
                StartCoroutine(Sequence_End_Round());
                break;
            case ("next wave"):
                m_wave++;
                if (m_wave == 2)
                {
                    m_ui_world.Show_Giant_Text("Well done", 5,"giant text finish - cannon");
                }
                break;
            case ("End Round"):
                m_game_state = GameState.None;
                Build_Summary_Scores();
                break;
            case ("giant text finish - cannon"):
                Game_Event("End Round");
                break;
        }
    }

    IEnumerator Sequence_End_Round()
    {
        Game_Event("stop cannon");
        yield return new WaitForSeconds(2f);
        Game_Event("next wave");
    }

    private void Count_Shots()
    {
        m_wave_shots_left--;
        if (m_wave_shots_left <= 0)
        {
            Game_Event("end wave");

        }
    }

    public void Collider_Event(script_puck script, string type)
    {
        
        //puck exits the goal area
        if (m_game_state == GameState.Shooting)
        {

            if (type == "exit zone")
            {
                Score_Positive(script);
            }
            else if (type == "score")
            {
                Score_Negative(script);
            }
        }
    }

    private void Score_Positive (script_puck script)
    {
        script.m_cannon_fired = false;
        m_score_positive++;
        m_stats_saves++;
        m_ui_world.Count_Save(m_score_positive);

    }

    private void Score_Negative(script_puck script)
    {
        script.m_cannon_fired = false;
        m_score_negative++;
        m_stats_allowed++;
        m_ui_world.Count_Score(m_score_negative);
    }

    

    

    private void Build_Summary_Scores()
    {

        score_array[0] = "Round score: " + m_score_positive; //round score
        score_array[1] = "Saves made: " + m_stats_saves; //saves made
        score_array[2] = "Goals allowed: " + m_stats_allowed; //goals allowed
        score_array[3] = "Round time: " + Seconds_to_time(m_stats_time) ; //round time
        score_array[4] = "Games played: " + m_stats_games_played; // games played
        score_array[5] = "Tital saves: " + m_stats_saves_total; // total saves
        score_array[6] = "BEST ROUND: <color=#5ACFFFFF>: " + m_stats_best + "</color> points"; // best round points
        score_array[7] = "7 Days record: " + m_stats_record; // 7 days record
        score_array[8] = "World position: " + m_stats_world; // word position

        m_ui_world.Show_Summary("cannon", score_array);
    }

    private string Seconds_to_time(float sec)
    {
        string minutes = Mathf.Floor(sec / 60).ToString("00");
        string seconds = Mathf.RoundToInt(sec % 60).ToString("00");

        return minutes + ":" + seconds;
    }

    IEnumerator Shoot_Puck()
    {
        while (true)
        {
            //select cannon
            m_selected_cannon = m_cannon_easy;
            m_selected_cannon.Shoot_prepare();
            yield return new WaitForSeconds(0.6f);


            //puck fire
            m_goal_collider_exit.enabled = false;
            m_selected_cannon.Shoot(m_pucks[m_current_puck]);
            m_goal_collider_exit.enabled = true;
            Game_Event("count shots");

            //handle puck array index
            m_current_puck++;
            if (m_current_puck > m_pucks.Length - 1)
            {
                m_current_puck = 0;
            }

            //delay
            yield return new WaitForSeconds(m_shoot_frequency);


        }
    }





}
