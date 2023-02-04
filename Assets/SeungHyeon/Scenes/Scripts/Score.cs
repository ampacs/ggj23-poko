using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Text text;
    public static float score;
    Island_manager manager;
   
   

   
    void Start()
    {
        text = GetComponent<Text>();
       
    }

    // Update is called once per frame
    void Update()
    {
        score = manager.Points;
        text.text = score.ToString();
    }
}
