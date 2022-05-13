using FishNet;
using FishNet.Object;
using TMPro;

public class Countdown : NetworkBehaviour
{
    public double timeLeft = 180;
    public TMP_Text timerText;
    public bool IsRunning;
    private double sinceStarted, totalTime;

    private void Start()
    {
        IsRunning = true;
        sinceStarted= InstanceFinder.TimeManager.ServerUptime;
        totalTime = timeLeft;
    }

    void Update()
    {
        if (IsRunning)
        {
            if (timeLeft > 0)
            {
               timeLeft = totalTime-(InstanceFinder.TimeManager.ServerUptime-sinceStarted);
               DisplayTime(timeLeft);
            }
            else
            {
                timeLeft = 0;
                IsRunning = false;
                if(base.IsServer) GameSystem.Instance.RpcCheckGhostWinCon();
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

