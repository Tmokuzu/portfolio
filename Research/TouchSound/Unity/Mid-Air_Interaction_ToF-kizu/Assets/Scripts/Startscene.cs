using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startscene : MonoBehaviour
{
    [SerializeField] UDPReceive udp;
    [SerializeField] SceneChanger sc;

    [SerializeField] Buttons cat;
    [SerializeField] Buttons drawing;
    [SerializeField] Buttons peekaboo;
    [SerializeField] Buttons keypad;
    [SerializeField] Buttons fairy;
    [SerializeField] Buttons menu;
    private bool calib = false;
    [SerializeField] GameObject calibtext;
    private Buttons[] buttons = new Buttons[6];

    private int user_x = 0, user_y = 0, user_z = 0;
    private int tl_x = 0, tl_y = 0, br_y = 0, br_x = 0;


    // Start is called before the first frame update
    void Start()
    {
        buttons[0] = cat;
        buttons[1] = drawing;
        buttons[2] = peekaboo;
        buttons[3] = keypad;
        buttons[4] = fairy;
        buttons[5] = menu;
    }

    // Update is called once per frame
    void Update()
    {
        this.Set_UserPos();
        this.Set_RectPos();

        //Debug.Log("a");
        //Debug.Log(user_x);
        //Debug.Log(user_y);
        //Debug.Log(user_z);

        if (!calib)
        {
            Vector3Int p, l;
            for (int i = 0; i < buttons.Length; i++)
            {
                p = buttons[i].getPosition();
                l = buttons[i].getLength();
                if (Mathf.Abs(user_x - p.x) < l.x / 2 && Mathf.Abs(user_y - p.y) < l.y / 2 && Mathf.Abs(user_z - p.z) < l.z / 2)
                {
                    ButtonClick(i);
                    break;
                }
            }
        }

    }
    private void Set_UserPos()
    {
        Vector3Int user = udp.getNearPoint();
        user_x = user.x;
        user_y = user.y;
        user_z = user.z;
    }
    private void Set_RectPos()
    {
        Vector4 rect = udp.getRect();
        tl_x = (int)rect.x;
        tl_y = (int)rect.y;
        br_x = (int)rect.z;
        br_y = (int)rect.w;
    }

    private void ButtonClick(int bn)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == bn)
            {
                buttons[i].Clicked(true);
            }
            else
            {
                buttons[i].Clicked(false);
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            calib = !calib;
            if (calib)
            {
                calibtext.SetActive(true);
            }
            else
            {
                calibtext.SetActive(false);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].ReadValue();
                }
            }
        }
    }

    public bool Get_is_Calib()
    {
        return calib;
    }
}
