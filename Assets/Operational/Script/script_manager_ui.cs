using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_manager_ui : MonoBehaviour {

    private script_manager_gameplay m_manager_gameplay; 

    private GameObject m_ui_main;
    private List<GameObject> m_ui_main_children = new List<GameObject>();

    private GameObject m_ui_main_goalie;
    private List<GameObject> m_uit_main_goalie_children = new List<GameObject>();

    void Awake()
    {

        m_ui_main = GameObject.Find("UI_Main").gameObject;
        m_ui_main_goalie = GameObject.Find("UI_Main_Goalie").gameObject;
        
        m_manager_gameplay = GameObject.Find("Manager_Gameplay").GetComponent<script_manager_gameplay>();
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
        gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonPressed(string id)
    {
        switch (id)
        {
            case "goalie":
                foreach( GameObject child in m_ui_main_children)                {                    child.SetActive(false);               }
                foreach (GameObject child in m_uit_main_goalie_children)        {                    child.SetActive(true);                }
                break;
            case "to_main":
                foreach (GameObject child in m_ui_main_children)                {                    child.SetActive(true);                }
                foreach (GameObject child in m_uit_main_goalie_children)        {                    child.SetActive(false);               }
                break;
            case "cannon":
                transform.position = new Vector3(0, -20, 0);
                m_manager_gameplay.Game_Event(id);
                break;
        }
    }
}
