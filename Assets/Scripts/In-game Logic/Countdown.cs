using Mirror;
using TMPro;

public class Countdown : NetworkBehaviour
{
    public double timeLeft = 180;
    public TMP_Text timerText;
    public bool IsRunning, BackToRoom;
    private double sinceStarted, totalTime;

    private void Start()
    {
        IsRunning = true;
        BackToRoom = false;
        sinceStarted= NetworkTime.time;
        totalTime = timeLeft;
    }

    void Update()
    {
        if (IsRunning)
        {
            if (timeLeft > 0)
            {
               timeLeft = totalTime-(NetworkTime.time-sinceStarted);
               DisplayTime(timeLeft);
            }
            else
            {
                timeLeft = 0;
                IsRunning = false;
                BackToRoom = true;
                if(isServer) GameSystem.Instance.RpcCheckGhostWinCon();
            }
        }
        else if (BackToRoom)
        {
            if(timeLeft > - 10)
            {
                timeLeft = 0 - (NetworkTime.time - sinceStarted - totalTime);
            }
            else
            {
                ChangeToRoom();
                BackToRoom = false;
            }
        }

    }

    private void DisplayTime(double timeToDisplay)
    {
        double minutes = (int)(timeToDisplay / 60);
        double seconds = (int)(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void ChangeToRoom()
    {
        var manager = NetworkManager.singleton as GHNetworkManager;
        if (isServer) manager.ServerChangeScene(manager.RoomScene);
    }
}

