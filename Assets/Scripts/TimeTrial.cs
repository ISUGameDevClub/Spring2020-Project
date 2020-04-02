using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrial : MonoBehaviour
{
    private float timer;
    private bool timerGoing;
    public GameObject timerText;
    private Text currentText;

    private void Update()
    {
        if (timerGoing)
        {
            timer += Time.deltaTime;
            currentText.color = new Color(0, 0, 0, 1);
            currentText.text = timer.ToString("F2");
        }
        else
            timer = 0;
    }

    public void StartTrial()
    {
        if (!timerGoing)
        {
            currentText = Instantiate(timerText).gameObject.GetComponent<Text>();
            timerGoing = true;
        }
    }

    public void EndTrial()
    {
        if (timerGoing)
        {
            Destroy(currentText);
            timerGoing = false;
        }
    }
}
