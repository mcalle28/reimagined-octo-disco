using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseManager : MonoBehaviour
{
    private BoxCollider2D chase;
    private Animator animator;

    private GameObject audioComponent;
    private AudioSource audioSource;
    public AudioClip Chase, Hanging;

    void Start(){
        chase = GetComponent<BoxCollider2D>();
        animator = GetComponentInParent<Animator>();

        var aspect = (float)Screen.width / Screen.height;
        var orthoSize = 3f;
        var width = 2.0f * orthoSize * aspect;
        var height = 2.0f * 3f;

        chase.size = new Vector2(width, height);

        audioComponent = GameObject.Find("AudioManager");
        audioSource= audioComponent.GetComponent(typeof(AudioSource)) as AudioSource;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngamePlayerController>();
        if ((player && player.role == Role.Hunter)&& !animator.GetBool("captured"))
        {
            audioSource.clip = Chase;
            audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngamePlayerController>();
        if ((player && player.role == Role.Hunter) && audioSource.clip!=Hanging)
        {
            WaitToPlay();
            audioSource.clip = Hanging;
            audioSource.Play();
        }
    }

    private IEnumerator WaitToPlay()
    {
        yield return new WaitForSeconds(5f);
    }

}
