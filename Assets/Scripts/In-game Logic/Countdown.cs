using Mirror;
using TMPro;

public class Countdown : NetworkBehaviour
{
    public double timeLeft = 120;
    public TMP_Text timerText;
    public bool IsRunning = false;
    private double sinceStarted = 0;

    private void Start()
    {
        IsRunning = true;
        sinceStarted= NetworkTime.time;
    }

    void Update()
    {
        if (IsRunning)
        {
            if (timeLeft > 0)
            {
               timeLeft = 20-(NetworkTime.time-sinceStarted);
               DisplayTime(timeLeft);
            }
            else
            {
                timeLeft = 0;
                IsRunning = false;
                if(isServer) GameSystem.Instance.RpcCheckGhostWinCon();
            }
        }

    }

    private void DisplayTime(double timeToDisplay)
    {
        double minutes = (int)(timeToDisplay / 60);
        double seconds = (int)(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

