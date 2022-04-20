using Mirror;
using TMPro;
using UnityEngine;

public class Countdown : NetworkBehaviour
{
    public double timeLeft = 120;
    public TMP_Text timerText;
    public bool IsRunning = false;
    private double sinceStarted = 0;

    private void Start()
    {
        // Auto-Start
        IsRunning = true;
        sinceStarted= NetworkTime.time;
    }

    void Update()
    {
        if (IsRunning)
        {
            if (timeLeft > 0)
            {
               timeLeft = 120-(NetworkTime.time-sinceStarted);
               DisplayTime(timeLeft);
            }
            else
            {
                // The timer ends
                timeLeft = 0;
                IsRunning = false;
            }
        }

    }

    private void DisplayTime(double timeToDisplay)
    {
        //timeToDisplay +=1;
        double minutes = (int)(timeToDisplay / 60);
        double seconds = (int)(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

