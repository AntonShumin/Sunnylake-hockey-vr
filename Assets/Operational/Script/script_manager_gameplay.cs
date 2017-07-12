using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_manager_gameplay : MonoBehaviour {

    public bool m_testing;

    public enum GameTypes { None, Cannon };
    public GameTypes m_game_type;
    

    private script_manager_ui_world m_ui_world;
    private script_manager_gameplay_cannon m_manager_gameplay_cannon;

    void Awake()
    {
        
        m_ui_world = GameObject.Find("World Canvas").GetComponent<script_manager_ui_world>();
        m_manager_gameplay_cannon = GetComponent<script_manager_gameplay_cannon>();

    }

	// Use this for initialization
	void Start () {
        if(m_testing)
        {
            //m_manager_gameplay_cannon.Game_Event("cannon");
            //m_ui_world.Show_main_ui();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Game_Event(string event_name)
    {
        switch (event_name)
        {
            case "cannon":
                m_game_type = GameTypes.Cannon;
                m_manager_gameplay_cannon.Game_Event(event_name);
                
                break;

        }
    }

    


}
