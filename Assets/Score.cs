using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour 
{
    /*** Public Static Fields ***/
    public static Score instance;

    public int Value
    {
        get{ return value; }
    }

    private Text text;
    private int value = 0;

    public void IncScore()
    {
        text.text = "" + ++value;
    }

    void Start () 
    {
        instance = this;
        text = GetComponent<Text>();
    }
}
