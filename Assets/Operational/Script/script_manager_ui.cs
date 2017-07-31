using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_manager_ui : MonoBehaviour {

    private script_manager_gameplay m_manager_gameplay;
    private script_manager_ui_world m_manager_ui_world;

    private GameObject m_ui_main;
    private List<GameObject> m_ui_main_children = new List<GameObject>();

    private GameObject m_ui_main_goalie;
    private List<GameObject> m_uit_main_goalie_children = new List<GameObject>();

    private GameObject m_ui_main_challenges;
    private List<GameObject> m_uit_main_challenges_children = new List<GameObject>();

    void Awake()
    {

        m_ui_main = GameObject.Find("UI_Main").gameObject;
        m_ui_main_goalie = GameObject.Find("UI_Main_Goalie").gameObject;
        m_ui_main_challenges = GameObject.Find("UI_Main_Challenges").gameObject;

        m_manager_gameplay = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay>();
        m_manager_ui_world = GameObject.Find("World Canvas").GetComponent<script_manager_ui_world>();
    }

	// Use this for initialization
	void Start () {

        //main list
        foreach(Transform child in m_ui_main.transform)
        {
            m_ui_main_children.Add(child.gameObject);
        }

        //goalie list
        foreach (Transform child in m_ui_main_goalie.transform)
        {
            m_uit_main_goalie_children.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        //challenges list
        foreach (Transform child in m_ui_main_challenges.transform)
        {
            m_uit_main_challenges_children.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        //other
        gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonPressed(string id)
    {
        m_manager_ui_world.Game_Events("button press");
        switch (id)
        {
            case "goalie":
                foreach( GameObject child in m_ui_main_children)             {  child.SetActive(false);  }
                foreach (GameObject child in m_uit_main_goalie_children)     {  child.SetActive(true);   }
                foreach (GameObject child in m_uit_main_challenges_children) { child.SetActive(false);   }
                break;
            case "to_main":
                foreach (GameObject child in m_ui_main_children)             {  child.SetActive(true);   }
                foreach (GameObject child in m_uit_main_goalie_children)     {  child.SetActive(false);  }
                foreach (GameObject child in m_uit_main_challenges_children) { child.SetActive(false);   }
                break;
            case "cannon":
                m_manager_ui_world.Hide_main_ui();
                m_manager_gameplay.Game_Event(id);
                break;
            case "challenges":
                foreach (GameObject child in m_ui_main_children) { child.SetActive(false); }
                foreach (GameObject child in m_uit_main_goalie_children) { child.SetActive(false); }
                foreach (GameObject child in m_uit_main_challenges_children) { child.SetActive(true); }
                break;
        }
    }
}
