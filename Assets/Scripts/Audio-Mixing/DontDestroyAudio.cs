using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyAudio : MonoBehaviour
{

    private AudioSource audioSource;
    private static GameObject instance;
    private bool isMainPlaying;
    public AudioClip MainRoom, Corridor;


    private void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        isMainPlaying = true;
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)instance = this.gameObject;
        else Destroy(this.gameObject);
    }
    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (next.name == "Hanging Corridor"){ PlayCorridorClip(); isMainPlaying = false; }
        else if((next.name== "Main Screen" || next.name == "Room") && !isMainPlaying) PlayMainRoomClip();
    }

    public void PlayCorridorClip()
    {
        audioSource.clip = Corridor;
        audioSource.Play();
    }

    public void PlayMainRoomClip() {
        audioSource.clip = MainRoom;
        audioSource.Play();
    }
}
