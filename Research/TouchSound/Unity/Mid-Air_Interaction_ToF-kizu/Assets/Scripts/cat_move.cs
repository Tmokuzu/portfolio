using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class cat_move : MonoBehaviour
{
    private GameObject neck;
    private int user_x, user_y, user_z;
    private int tl_x = 0,tl_y = 0,br_y = 0,br_x = 0;
    private bool nothing = false;
    private int cat_x,cat_y, cat_z;

    [SerializeField] UDPReceive udp;
    [SerializeField] SceneChanger sc;
    private Animator animator;
    private float countTime = 0,countTime2 = 0;

    private string _name;
    private string sceneName;
    private StreamWriter sw;
    private StreamReader sr;
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    private bool calib = false;
    [SerializeField] GameObject calibtext;

    // Start is called before the first frame update
    void Start()
    {
        if(!ReadValue())
        {
            cat_x = 290;
            cat_y = 205;
            cat_z = 510;
        }

        neck = GameObject.Find("cu_cat_neck_joint");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        countTime += Time.deltaTime;
        countTime2 += Time.deltaTime;
        this.Set_UserPos();
        this.Set_RectPos();
        this.Set_isNothing();
        //if (nothing) sc.sceneChange("Start");

        Vector3 currentP = this.transform.position;

        //Debug.Log("a");
        //Debug.Log(user_x);
        //Debug.Log(user_y);
        //Debug.Log(user_z);
        /*
        if (user_z > 550 && user_z < 800 && animator.GetCurrentAnimatorStateInfo(0).IsName("C_sleep"))
        {
            Debug.Log(user_z);
            animator.SetBool("awake", true);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("A_walk") && currentP.z < 0.89)
        {
            this.transform.position = new Vector3(currentP.x,currentP.y,currentP.z + 0.0015f);
        }
        else 
        */
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("default") && !calib)
        {
            if (user_z < 520 && user_z > 340)
            {
                countTime2 = 0;
                tracking(false);
                if (user_y < cat_y - 20 && user_x > cat_x-25 && user_x < cat_x + 25 && countTime > 7)
                {
                    if (Random.value < 0.6)
                    {
                        animator.SetBool("jump", true);
                        countTime = 0;
                    }
                    else
                    {
                        countTime = 5;
                    }
                }
                else if (user_y > cat_y + 20 && user_x > cat_x - 25 && user_x < cat_x + 25 && countTime > 4)
                {
                    if (Random.value < 0.7)
                    {
                        animator.SetBool("play", true);
                        countTime = 0;
                    }
                    else
                    {
                        countTime = 4;
                    }
                }
            }
            else
            {
                tracking(true);
                if(countTime2 > 12)
                {
                    countTime = 0;
                    countTime2 = 0;
                    float r = Random.value;
                    if (r < 0.3f)
                    {
                        //animator.SetBool("idle1", true);
                    }
                    else if(r < 0.6)
                    {
                        //animator.SetBool("idle2", true);
                    }
                    else
                    {
                        //animator.SetBool("idle3", true);
                    }
                }
            }
        }
        else if((animator.GetCurrentAnimatorStateInfo(0).IsTag("move")) && !calib)
        {
            if (user_z < 520 && user_z > 340)
            {
                tracking(false);
            }
            else
            {
                tracking(true);
            }
        }

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
                this.ReadValue();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Return) && calib)
        {
            setPosition();
        }
    }

    private Vector2 direc_prev;
    private int threshold = 3;
    private void tracking(bool keep)
    {
        if (keep)
        {
            neck.transform.rotation = Quaternion.identity;
            neck.transform.Rotate(direc_prev.x, direc_prev.y, 0f);
            direc_prev = new Vector2(direc_prev.x * 98f / 100.0f, direc_prev.y * 98f / 100);
        }
        else
        {
            Vector2 target_pos = new Vector3(user_x, user_y);
            Vector2 char_pos = new Vector3(cat_x, cat_y);
            Vector2 direction = target_pos - char_pos;

            Vector2 goal_angle = new Vector2((50f * (float)direction.y / 100f), -(50f * (float)direction.x / 100f));

            //Debug.Log((goal_angle - direc_prev).magnitude);
            Vector2 goal_delta = direc_prev;
            if ((goal_angle - direc_prev).magnitude > threshold)  //大きい移動のとき移動量分割
            {
                goal_delta += (goal_angle - direc_prev).normalized / 0.6f;  //移動量分割オン
            }
            else
            {
                goal_delta += (goal_angle - direc_prev);  //移動量分割オフ
            }
            neck.transform.rotation = Quaternion.identity;
            neck.transform.Rotate(goal_delta.x, goal_delta.y, 0f);

            direc_prev = new Vector2(goal_delta.x, goal_delta.y);
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

    private void setPosition()
    {
        Vector3Int user = udp.getNearPoint();

        sw = new StreamWriter(Application.persistentDataPath + "/PositionCSV/" + sceneName + "/" + _name + ".csv", false, Encoding.UTF8);

        sw.WriteLine("x,y,z");
        sw.WriteLine(user.x.ToString() + "," + user.y.ToString() + "," + user.z.ToString());
        sw.Flush();
        sw.Close();
    }

    private bool ReadValue()
    {
        sceneName = SceneManager.GetActiveScene().name;
        _name = this.name;
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/PositionCSV"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/PositionCSV");
        }
        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/PositionCSV/" + sceneName))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/PositionCSV/" + sceneName);
        }

        if (System.IO.File.Exists(Application.persistentDataPath + "/PositionCSV/" + sceneName + "/" + _name + ".csv"))
        {
            sr = new StreamReader(Application.persistentDataPath + "/PositionCSV/" + sceneName + "/" + _name + ".csv", Encoding.UTF8);
            while (sr.Peek() != -1) // reader.Peaekが-1になるまで
            {
                string line = sr.ReadLine();
                csvDatas.Add(line.Split(','));
            }
            cat_x = int.Parse(csvDatas[1][0]);
            cat_y = int.Parse(csvDatas[1][1]);
            cat_z = int.Parse(csvDatas[1][2]);
            sr.Close();
            return true;
        }
        else
        {
            return false;
        }

    }

}
