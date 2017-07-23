using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_manager_gameplay_cannon : MonoBehaviour {

    public enum GameState { None, Active, Shooting };
    private GameState m_game_state = GameState.None;
    

    private script_manager_ui_world m_ui_world;
    private script_cannon m_cannon_center;
    private script_cannon m_cannon_left;
    private script_cannon m_cannon_right;
    private script_cannon[] m_cannons = new script_cannon[3];

    //puck management
    public float m_shoot_frequency; //1.4
    public GameObject m_puck_prefab;
    private Rigidbody[] m_pucks = new Rigidbody[5];
    private int m_current_puck = 0;
    private IEnumerator shoot_coroutine;

    //Scores
    private int m_score_positive;
    private int m_score_negative;
    public int m_wave;
    public int m_wave_shots_left;
    private int m_wave_max = 5; //current max 4

    private int m_stats_saves;
    private int m_stats_allowed;
    private float m_stats_time;
    private int m_stats_games_played;
    private int m_stats_saves_total;
    private int m_stats_best;
    private int m_stats_record;
    private int m_stats_world;

    //other
    private script_cannon m_selected_cannon;
    private Collider m_goal_collider_exit;

    //cached
    private string[] score_array = new string[9];
    



    void Awake()
    {
        m_ui_world = GameObject.Find("World Canvas").GetComponent<script_manager_ui_world>();
        m_cannon_center = GameObject.Find("Cannon C").GetComponent<script_cannon>();
        m_cannon_left = GameObject.Find("Cannon L").GetComponent<script_cannon>();
        m_cannon_right = GameObject.Find("Cannon R").GetComponent<script_cannon>();
        m_cannons[0] = m_cannon_center;
        m_cannons[1] = m_cannon_left;
        m_cannons[2] = m_cannon_right;
        shoot_coroutine = Shoot_Puck();
        Setup_Cannon_Vars();
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
        m_stats_best = 10;
        m_stats_record = 300;
        m_stats_world = 8;

        //silent vars
        m_wave = 0;
        m_wave_shots_left = 0;
        
    }

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            m_pucks[i] = GameObject.Instantiate(m_puck_prefab, new Vector3(132,10,127), Quaternion.identity).GetComponent<Rigidbody>();
            //m_pucks[i].gameObject.SetActive(false);
        }
        
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
                Game_Event("setup summary");
                Game_Event("next wave");
                //Game_Event("next wave start");
                m_game_state = GameState.Active;
                break;
            case ("cannon fire"):
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
                StopCoroutine(shoot_coroutine);
                break;
            case ("end wave"):
                StartCoroutine(Sequence_End_Round());
                break;
            case ("next wave"):
                SetupNextWave();
                break;
            case ("next wave start"):
                Game_Event("cannon fire");
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


    private void SetupNextWave()
    {
        //preset vars
        string giant_text_message = "";
        bool speedy_round = false;

        //check for speed round
        if(m_wave == 0)
        {

            m_wave++;
            giant_text_message = "Warm Up";

                
        }
        //not the very first wave
        else
        {
            //speedy round
            if (m_cannons[0].m_cannon_settings[m_wave - 1].m_speedy)
            {
                //set vars
                speedy_round = true;
                m_cannons[0].m_cannon_settings[m_wave - 1].m_speedy = false;

                //the very last wave
                if (m_wave == m_cannons[0].m_cannon_settings.Length) 
                {
                    giant_text_message = "Final stand!";
                    giant_text_message += " Hot " + m_cannons[0].m_cannon_settings[m_wave - 1].m_hot.ToString();
                    script_puck.set_hot(m_cannons[0].m_cannon_settings[m_wave - 1].m_hot);
                }
                //not last wave
                else
                {
                    giant_text_message = "Speed round";

                }

            }
            //non speedy round
            else
            {

                //set vars
                m_wave++;
                giant_text_message = "Speed up" + (m_wave - 1).ToString();

                //hot is set, not last wave
                if (m_cannons[0].m_cannon_settings[m_wave - 1].m_hot != script_cannon_settings.hot.none && m_wave != m_cannons[0].m_cannon_settings.Length - 1)
                {
                    giant_text_message += " Hot " + m_cannons[0].m_cannon_settings[m_wave - 1].m_hot.ToString();
                    script_puck.set_hot(m_cannons[0].m_cannon_settings[m_wave - 1].m_hot);
                }


            }
        }
        
        
        

        //if last wave and no more speedy rounds
        if (m_wave <= m_wave_max || speedy_round == true )
        {

            //prepare vars
            m_wave_shots_left = m_cannons[0].m_cannon_settings[m_wave - 1].m_max_shots;
            m_cannon_center.Set_Next_Wave_Stats(m_wave - 1);
            m_cannon_left.Set_Next_Wave_Stats(m_wave - 1);
            m_cannon_right.Set_Next_Wave_Stats(m_wave - 1);
            m_ui_world.Show_Giant_Text(giant_text_message, 5, "next wave start");

        } else
        {
            m_ui_world.Show_Giant_Text("Game Over", 5, "giant text finish - cannon");
        }
    }

    public void Collider_Event(script_puck script, string type)
    {
        
        //puck exits the goal area
        if (m_game_state == GameState.Shooting || m_game_state == GameState.Active)
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
        //load vars

        //saves total
        m_stats_saves_total = m_score_positive;
        if ( ES2.Exists("cannon_total_saves") )
        {
            m_stats_saves_total += ES2.Load<int>("cannon_total_saves");
            Debug.Log("total is " + m_stats_saves_total);
        }
        //record
        if (ES2.Exists("cannon_record"))
        {
            m_stats_record = Mathf.Max(m_score_positive, ES2.Load<int>("cannon_record"));
        } else
        {
            m_stats_record = m_score_positive;
        }
        //games played
        m_stats_games_played = 1;
        if (ES2.Exists("cannon_games_played"))
        {
            m_stats_games_played += ES2.Load<int>("cannon_games_played");
        }

        //saving
        ES2.Save(m_stats_saves_total, "cannon_total_saves");
        ES2.Save(m_stats_record, "cannon_record");
        ES2.Save(m_stats_games_played, "cannon_games_played");


        //buid ui array 
        score_array[0] = "Score: " + m_score_positive; //round score
        score_array[1] = "Saves made: " + m_stats_saves; //saves made
        score_array[2] = "Goals allowed: " + m_stats_allowed; //goals allowed
        score_array[3] = "Round time: " + Seconds_to_time(m_stats_time) ; //round time
        score_array[4] = "Games played: " + m_stats_games_played; // games played
        score_array[5] = "Total saves: " + m_stats_saves_total; // total saves
        score_array[6] = "World: TOP <color=#5ACFFFFF> " + m_stats_best + "</color>"; // best round points
        score_array[7] = "Record: " + m_stats_record; // 7 days record
        //score_array[8] = "World position: " + m_stats_world; // word position

        m_ui_world.Show_Summary("cannon", score_array);
        /*
        foreach( string s in score_array)
        {
            Debug.Log(s);
        }
        */
        

        
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
            //m_selected_cannon = m_cannons[0];
            m_selected_cannon = m_cannons[Random.Range(0, 3)];
            
            m_selected_cannon.Shoot_prepare();
            yield return new WaitForSeconds(0.4f);

            m_selected_cannon.Shoot_play_sound();
            yield return new WaitForSeconds(0.2f);


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

    private void Setup_Cannon_Vars()
    {
        //setup special wave levels
        //hot and speedy
        int[] hot_list = new int[3] { 1,2,3 };
        hot_list = Shuffle(hot_list);
        int not_hot = Random.Range(1, 4); // wave 2,3,4
        int is_speedy = Random.Range(1, 3); //pick speedy wave 2 or 3
        for (int i = 1; i<=3; i++)
        {
            if(i != not_hot)
            {

                m_cannons[0].m_cannon_settings[i].m_hot = (script_cannon_settings.hot)hot_list[i-1];
            }
            if(i == is_speedy)
            {
                m_cannons[0].m_cannon_settings[i].m_speedy = true;
            }
        }
        m_cannons[0].m_cannon_settings[4].m_hot = (script_cannon_settings.hot)Random.Range(1, 4);
        m_cannons[0].m_cannon_settings[4].m_speedy = true;


        //synchronize cannon settings to the center cannon preset
        for (int y = 0; y < m_cannons[0].m_cannon_settings.Length; y++)
        {
            for (int i = 1; i <= 2; i++)
            {
                m_cannons[i].m_cannon_settings[y].m_max_shots = m_cannons[0].m_cannon_settings[y].m_max_shots;
                m_cannons[i].m_cannon_settings[y].m_speedy = m_cannons[0].m_cannon_settings[y].m_speedy;
                m_cannons[i].m_cannon_settings[y].m_hot = m_cannons[0].m_cannon_settings[y].m_hot;
            }
        }
        
    }

    public int[] Shuffle(int[] decklist)
    {
        for (int t = 0; t < decklist.Length; t++)
        {
            int tmp = decklist[t];
            int r = Random.Range(t, decklist.Length);
            decklist[t] = decklist[r];
            decklist[r] = tmp;
        }

        return decklist;
    }





}
