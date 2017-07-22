using System;
using UnityEngine;

[Serializable]
public class script_cannon_settings {

    public enum hot
    {
        none,
        stick,
        pad,
        glove
    }

    public float m_velocity;
    public float m_height_top;
    public float m_heigh_bottom;
    public float m_width_max;
    [System.NonSerialized] public float m_width_min;
    public  int m_max_shots;
    public  bool m_speedy = false;
    public  hot m_hot;

    /*
     *Basic flow:
     * warm up - 10 shots
     * 33,36,39 - 15 shots, 2 of the three going hot
     * 33 or 36 speedy - 20 shots
     * 43 - 10 shots
     * 43 - 20 shots speedy hot
     */


	
}
