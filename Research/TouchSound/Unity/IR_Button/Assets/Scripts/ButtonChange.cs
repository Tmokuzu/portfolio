using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

//public bool ButtonState;

public class ButtonChange : MonoBehaviour
{
    // Start is called before the first frame update
    public Image img;
    public Sprite NONPUSH;
    public Sprite ABOVE;
    public Sprite PUSH;
    public string State;
    [SerializeField] UDPReceive udp;
    [SerializeField] SerialReceive serial;
    private int user_x, user_y, user_z;
    private float POINT;
    private bool nothing = false;

    public AudioClip se1;
    public AudioClip se2;
    public AudioClip se3;

    AudioSource audioSource1;
    AudioSource audioSource2;
    //TouchDisc touch;
    public float volume1;
    public float volume2;

    int cnt=0;

    bool isCalledOnce1 = false;
    bool isCalledOnce2 = false;

    void Start()
    {
        img.sprite = NONPUSH;
        State = "NONPUSH";
        //audioSource1 = GetComponent<AudioSource>();
        audioSource1 = gameObject.AddComponent<AudioSource>();
        //audioSource2 = GetComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource1.volume = volume1;
        audioSource2.volume = volume2;
    }

    // Update is called once per frame
    void Update()
    {
        this.Set_UserPos();
        //this.Set_SerialPos();

        //volume = 100 - user_z/10;
        //volume = (user_z - 450) / 150;
        volume2 = (1 - ((float)user_z - 450) / 150) / 1;

        /*
        if(POINT < 550 && POINT > 300){
            img.sprite = ABOVE;
            State = "ABOVE";
            if (POINT < 400){
                img.sprite = PUSH;
                State = "PUSH";
            }
        }
        else
        {
            img.sprite = NONPUSH;
            State = "NONPUSH";
        }
        */

        if ((user_x > 230 && user_x < 290) && (user_y > 150 && user_y < 200))
        {
            img.sprite = ABOVE;
            State = "ABOVE";
            if (!isCalledOnce2)
                {
                    isCalledOnce2 = true;
                    audioSource1.PlayOneShot(se2);
                }
            

            if(user_z < 450 && user_z > 410)
            {
                img.sprite = PUSH;
                State = "PUSH";
                audioSource2.Stop();
                //touch.OnClickDiscSe1();
                
                if (!isCalledOnce1)
                {
                    isCalledOnce1 = true;
                    audioSource1.PlayOneShot(se1);
                }
            }
        }
        else
        {
            img.sprite = NONPUSH;
            State = "NONPUSH";
            isCalledOnce1 = false;
            isCalledOnce2 = false;
            audioSource2.Stop();
        }
        if (State == "ABOVE")
        {
            aboveSound();
        }

        //State = 
        Debug.Log(user_x + "," + user_y + "," + user_z + "," + volume1 + "," + volume2);
        //Debug.Log(cnt + "," + POINT);
        //Debug.Log(POINT);
        //if(ButtonState == True){
        //    img.sprite = Resources.Load<Sprite>("button2");
        //}
        //else{
        //    img.sprite = Resources.Load<Sprite>("button1");
        // }
        //cnt++;

    }
    private void Set_UserPos()
    {
        Vector3Int user = udp.getNearPoint();
        user_x = user.x;
        user_y = user.y;
        user_z = user.z;
    }
    private void Set_SerialPos()
    {
        POINT = serial.getPoint();
    }
    private void Set_isNothing()
    {
        if (udp.get_is_Nothing() == 0)
        {
            nothing = false;
        }
        else
        {
            nothing = true;
        }
    }
    private void aboveSound()
    {
        audioSource2.volume = volume2;
        audioSource2.loop = !audioSource2.loop;
        audioSource2.PlayOneShot(se3);
    }
}
