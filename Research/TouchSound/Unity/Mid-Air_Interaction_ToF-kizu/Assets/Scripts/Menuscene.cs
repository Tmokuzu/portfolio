using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menuscene : MonoBehaviour
{
    [SerializeField] UDPReceive udp;
    [SerializeField] SceneChanger sc;

    [SerializeField] Buttons_for_menu button1;
    [SerializeField] Buttons_for_menu button2;
    [SerializeField] Buttons_for_menu button3;
    [SerializeField] Buttons_for_menu button4;
    [SerializeField] Buttons_for_menu button5;
    [SerializeField] Buttons_for_menu button6;
    [SerializeField] Buttons_for_menu back;
    [SerializeField] Buttons_for_menu next;
    [SerializeField] Buttons_for_menu menu;
    [SerializeField] Buttons_for_menu c1;
    [SerializeField] Buttons_for_menu c2;
    [SerializeField] Buttons_for_menu c3;
    [SerializeField] Buttons_for_menu c4;
    [SerializeField] Buttons_for_menu ok;
    [SerializeField] GameObject Buttons_group;
    [SerializeField] GameObject Counts_group;
    [SerializeField] GameObject Ordered_group;
    private bool calib = false;
    [SerializeField] GameObject calibtext;

    private Buttons_for_menu[] buttons = new Buttons_for_menu[8];
    private Buttons_for_menu[] counts = new Buttons_for_menu[5];

    private int user_x = 0, user_y = 0, user_z = 0;
    private int tl_x = 0, tl_y = 0, br_y = 0, br_x = 0;
    private bool nothing = false;
    private int state = 0;
    private int scene = 0;
    private float countTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        buttons[0] = button1;
        buttons[1] = button2;
        buttons[2] = button3;
        buttons[3] = button4;
        buttons[4] = button5;
        buttons[5] = button6;
        buttons[6] = back;
        buttons[7] = next;
        counts[0] = c1;
        counts[1] = c2;
        counts[2] = c3;
        counts[3] = c4;
        counts[4] = ok;
    }

    // Update is called once per frame
    void Update()
    {
        this.Set_UserPos();
        this.Set_RectPos();
        this.Set_isNothing();
        countTime += Time.deltaTime;

        //if (nothing)sc.sceneChange("Start");
        if (countTime < 0.7f) return;

        if (scene == 0 && !calib)
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
        else if (scene == 1 && !calib)
        {
            Vector3Int p, l;
            for (int i = 0; i < counts.Length; i++)
            {
                p = counts[i].getPosition();
                l = counts[i].getLength();
                if (Mathf.Abs(user_x - p.x) < l.x / 2 && Mathf.Abs(user_y - p.y) < l.y / 2 && Mathf.Abs(user_z - p.z) < l.z / 2)
                {
                    CountsClick(i);
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
    private void CountsClick(int bn)
    {
        for (int i = 0; i < counts.Length; i++)
        {
            if (i == bn)
            {
                counts[i].Clicked(true);
            }
            else
            {
                counts[i].Clicked(false);
            }
        }
    }

    public void set_State(int s)
    {
        countTime = 0;
        state = s;
        for (int i = 0; i < buttons.Length; i++)
        {
           buttons[i].set_State(s);
        }
    }
    public int get_State()
    {
        return state;
    }
    public void go_count(Sprite sp)
    {
        countTime = 0;
        menu.GetComponent<Image>().sprite = sp;
        Buttons_group.SetActive(false);
        Counts_group.SetActive(true);
        scene = 1;
    }
    public void go_Order()
    {
        countTime = 0;
        Counts_group.SetActive(false);
        Ordered_group.SetActive(true);
        scene = 2;
        StartCoroutine(go_Menu());
    }

    IEnumerator go_Menu()
    {
        countTime = 0;
        yield return new WaitForSeconds(2.0f);
        Ordered_group.SetActive(false);
        Buttons_group.SetActive(true);
        scene = 0;
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
                for (int i = 0; i < counts.Length; i++)
                {
                    counts[i].ReadValue();
                }
            }
        }
    }

    public bool Get_is_Calib()
    {
        return calib;
    }
}
