using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrial : MonoBehaviour
{
    private float timer;
    private bool timerGoing;
    public Text text;
    
    private void Update()
    {
        if (timerGoing)
        {
            timer += Time.deltaTime;
            text.color = new Color(0, 0, 0, 1);
            text.text = timer.ToString("F2");
        }
        else
            timer = 0;
    }

    public void StartTrial()
    {
        if (!timerGoing)
            timerGoing = true;
    }

    public void EndTrial()
    {
        if (timerGoing)
            timerGoing = false;
    }
}
