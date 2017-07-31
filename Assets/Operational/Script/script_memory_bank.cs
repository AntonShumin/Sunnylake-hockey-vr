using UnityEngine;

public class script_memory_bank : MonoBehaviour {

    private string m_version;
    private string m_version_current = "0.1"; //in case of a mismatch, clear everything
    private int m_pucks;
    private string m_username;
    private string m_password;

    //accessors 
    public int Pucks
    {
        get { return m_pucks; }
    }

    void Awake()
    {
        Load_Data();
    }

    private void Load_Data()
    {
        //version check
        if( ES2.Exists("version") )
        {

            m_version = ES2.Load<string>("versions");
            if (m_version_current != m_version) Reset_All();

        } else
        {

            m_version = m_version_current;

        }

        //pucks
        if (ES2.Exists("pucks"))
        {
            m_pucks = ES2.Load<int>("pucks");
        } else
        {
            m_pucks = 0;
        }
    }

    
    public void Add_Pucks(int value)
    {

        m_pucks += value;
        ES2.Save(m_pucks, "pucks");

    }

    private void Reset_All()
    {
        //implementation
    }


}
