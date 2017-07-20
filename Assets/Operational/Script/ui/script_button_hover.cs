using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class script_button_hover : MonoBehaviour {

    //vars
    private script_manager_ui_world m_manager_world;

    void Start()
    {
        m_manager_world = GameObject.Find("World Canvas").GetComponent<script_manager_ui_world>();
    }

    //cached
    private Vector2 c_scale_hover = new Vector2(1.01f,1.01f);
    private Vector2 c_scale_original = new Vector2(1f, 1f);
    private Tweener c_tween;

    public void PointerEnter()
    {
        //Debug.Log("enter");
        //transform.localScale = c_scale_hover;
        //c_tween = transform.DOScale(c_scale_hover, 0.1f);
        m_manager_world.Game_Events("ui hover sound");
        
    }

    public void PointerExit()
    {
        Debug.Log("exit");
        //c_tween.Kill();
        //transform.localScale = c_scale_original;
        //m_manager_world.Game_Events("ui unhover sound");
    }
}
