using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTouchSound : MonoBehaviour
{
    public AudioClip clic;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        audioSource.PlayOneShot(clic);
    }

}
