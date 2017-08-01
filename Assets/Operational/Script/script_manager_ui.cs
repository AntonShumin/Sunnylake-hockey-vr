using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class script_manager_ui : MonoBehaviour {

    private script_manager_gameplay m_manager_gameplay;
    private script_manager_ui_world m_manager_ui_world;
    private script_memory_bank m_memory_bank;
    private Dictionary<string, GameObject> m_challenges = new Dictionary<string, GameObject>();
    private Dictionary<string, int> m_challenges_values = new Dictionary<string, int>();
    private Dictionary<string, GameObject> m_challenges_elements = new Dictionary<string, GameObject>();

    private GameObject m_ui_main;
    private List<GameObject> m_ui_main_children = new List<GameObject>();

    private GameObject m_ui_main_goalie;
    private List<GameObject> m_uit_main_goalie_children = new List<GameObject>();

    private GameObject m_ui_main_challenges;
    private List<GameObject> m_uit_main_challenges_children = new List<GameObject>();

    //cached
    private string[] c_string_array;
    private int c_cost;
    private Vector3 c_v3;

    void Awake()
    {

        m_ui_main = GameObject.Find("UI_Main").gameObject;
        m_ui_main_goalie = GameObject.Find("UI_Main_Goalie").gameObject;
        m_ui_main_challenges = GameObject.Find("UI_Main_Challenges").gameObject;

        m_manager_gameplay = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay>();
        m_manager_ui_world = GameObject.Find("World Canvas").GetComponent<script_manager_ui_world>();
        m_memory_bank = m_manager_gameplay.GetComponent<script_memory_bank>();

        //challenges
        c_string_array = new string[2];
        c_string_array[0] = "challenge_classic";
        c_string_array[1] = "challenge_sudden";
        foreach (string s in c_string_array)
        {
            m_challenges.Add(s, GameObject.Find(s).gameObject);
            m_challenges_values.Add(s + "_cost", 100);
        }
        m_challenges_elements.Add("not enough", GameObject.Find("not enough").gameObject);
        m_challenges_elements["not enough"].SetActive(false);
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
            case "challenge_sudden":
            case "challenge_classic":
                if ( Challenge_Fee(id) ) {
                    goto case "cannon"; //fall-through
                } else
                {
                    break;
                }
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

    private bool Challenge_Fee(string challenge_name)
    {
        c_cost = m_challenges_values[challenge_name + "_cost"];
        if (c_cost <= m_memory_bank.Pucks)
        {
            m_memory_bank.Add_Pucks(-c_cost);
            return true;
        } else
        {
            m_manager_ui_world.Play_UI_Sound(12);
            return false;
        }
        
    }

    public void Challenge_NotEnough(string s)
    {
        if (s == "hide")
        {
            m_challenges_elements["not enough"].SetActive(false);
        } else
        {
            c_cost = m_challenges_values[s + "_cost"];
            if (c_cost > m_memory_bank.Pucks)
            {
                c_v3 = m_challenges[s].transform.localPosition;
                c_v3.x += 250;
                c_v3.y -= 65;
                m_challenges_elements["not enough"].transform.localPosition = c_v3;
                m_challenges_elements["not enough"].SetActive(true);
            }

            
        }
    }
}
