using UnityEngine;

public class LocalStore : MonoBehaviour
{
    private string playerName;
    public static LocalStore instance = null;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void setPlayerName(string playerName) {
        this.playerName = playerName;
    }

    public string getPlayerName()
    {
        return playerName;
    }
}
