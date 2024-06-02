using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

//public bool ButtonState;

public class ButtonChangeSerial : MonoBehaviour
{
    // Start is called before the first frame update
    public Image img;
    public Sprite NONPUSH;
    public Sprite ABOVE;
    public Sprite PUSH;
    public string State;
    [SerializeField] SerialReceive serial;
    private float POINT;

    void Start()
    {
        img.sprite = NONPUSH;
        State = "NONPUSH";
    }

    // Update is called once per frame
    void Update()
    {
        //this.Set_UserPos();
        this.Set_SerialPos();

        if(POINT < 550 && POINT > 300){
            img.sprite = ABOVE;
            State = "ABOVE";
            if (POINT < 400){
                img.sprite = PUSH;
                State = "PUSH";
            }
        }
        else {
            img.sprite = NONPUSH;
            State = "NONPUSH";
        }
        
        Debug.Log(POINT);

    }

    private void Set_SerialPos()
    {
        POINT = serial.getPoint();
    }
}
