using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadscene : MonoBehaviour
{
    [SerializeField] UDPReceive udp;
    [SerializeField] SceneChanger sc;

    [SerializeField] Buttons_for_keypad c;
    [SerializeField] Buttons_for_keypad equal;
    [SerializeField] Buttons_for_keypad button1;
    [SerializeField] Buttons_for_keypad button2;
    [SerializeField] Buttons_for_keypad button3;
    [SerializeField] Buttons_for_keypad minus;
    [SerializeField] Buttons_for_keypad div;
    [SerializeField] Buttons_for_keypad button4;
    [SerializeField] Buttons_for_keypad button5;
    [SerializeField] Buttons_for_keypad button6;
    [SerializeField] Buttons_for_keypad plus;
    [SerializeField] Buttons_for_keypad mult;
    [SerializeField] Buttons_for_keypad button7;
    [SerializeField] Buttons_for_keypad button8;
    [SerializeField] Buttons_for_keypad button9;
    [SerializeField] Buttons_for_keypad button0;
    [SerializeField] Buttons_for_keypad buttonp;
    private bool calib = false;
    [SerializeField] GameObject calibtext;

    private Buttons_for_keypad[] buttons = new Buttons_for_keypad[17];

    private int user_x = 0, user_y = 0, user_z = 0;
    private int tl_x = 0, tl_y = 0, br_y = 0, br_x = 0;
    private bool nothing = false;

    private float countTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        buttons[0] = c;
        buttons[1] = equal;
        buttons[2] = button1;
        buttons[3] = button2;
        buttons[4] = button3;
        buttons[5] = minus;
        buttons[6] = div;
        buttons[7] = button4;
        buttons[8] = button5;
        buttons[9] = button6;
        buttons[10] = plus;
        buttons[11] = mult;
        buttons[12] = button7;
        buttons[13] = button8;
        buttons[14] = button9;
        buttons[15] = button0;
        buttons[16] = buttonp;
    }

    // Update is called once per frame
    void Update()
    {
        this.Set_UserPos();
        this.Set_RectPos();
        this.Set_isNothing();
        //if(nothing)sc.sceneChange("Start");
        countTime += Time.deltaTime;

        if (!calib && countTime > 0.7)
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
