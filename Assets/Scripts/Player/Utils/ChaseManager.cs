using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseManager : MonoBehaviour
{
    private CircleCollider2D chase;
    private GameObject audioComponent;
    private AudioSource audioSource;
    public AudioClip Chase, Hanging;
    public List<IngamePlayerController> targets = new List<IngamePlayerController>();
    void Start()
    {
        chase = GetComponent<CircleCollider2D>();
        audioComponent = GameObject.Find("AudioManager");
        audioSource= audioComponent.GetComponent(typeof(AudioSource)) as AudioSource;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngamePlayerController>();
        if (player && player.role == Role.Hunter)
        {
            audioSource.clip = Chase;
            audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngamePlayerController>();
        if (player && player.role == Role.Hunter)
        {
            audioSource.clip = Hanging;
            audioSource.Play();
        }
    }

}
