using UnityEngine;
using UnityEngine.EventSystems;

public class TouchDisc : MonoBehaviour
{
    public AudioClip se1;

    public AudioClip se2;

    public AudioClip se3;

    public AudioClip se4;

    AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnClickDiscSe1()
    {
        audioSource.PlayOneShot(se1);
        Debug.Log("click se1");
    }

    public void OnClickDiscSe2()
    {
        audioSource.PlayOneShot(se2);
        Debug.Log("click se2");
    }

    public void OnClickDiscSe3()
    {
        audioSource.PlayOneShot(se3);
        Debug.Log("click se3");
    }

    public void OnClickDiscSe4()
    {
        audioSource.PlayOneShot(se4);
        Debug.Log("click se4");
    }
}