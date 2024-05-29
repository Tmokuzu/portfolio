using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class fairy_move : MonoBehaviour
{
    private int user_x, user_y, user_z;
    private int tl_x = 0,tl_y = 0,br_y = 0,br_x = 0;
    private bool nothing = false;
    private float fairy_x,fairy_y, fairy_z;
    private const float Fairy_x_max = 3;
    private const float Fairy_x_min = -2.5f;
    private const float Fairy_y_max = 0.75f;
    private const float Fairy_y_min = -2.5f;
    private const float Fairy_width = Fairy_x_max - Fairy_x_min;
    private const float Fairy_height = Fairy_y_max - Fairy_y_min;

    [SerializeField] UDPReceive udp;
    [SerializeField] SceneChanger sc;
    private bool flag = true;

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
            fairy_x = 315;
            fairy_y = 190;
            fairy_z = 440;
        }
    }


    // Update is called once per frame
    void LateUpdate()
    {
        this.Set_UserPos();
        this.Set_RectPos();
        this.Set_isNothing();
        //if (nothing) sc.sceneChange("Start");

        Vector3 currentP = this.transform.position;
        //float x = ((user_x- br_x) / (tl_x - br_x))* Fairy_width - Fairy_x_min;
        //float y = ((user_y- br_y) / (tl_y - br_y))* Fairy_height - Fairy_y_min;
        Vector2 dst = new Vector2(user_x, user_y);
        Vector2 fairy = new Vector2(fairy_x, fairy_y);
        Vector2 move = (dst - fairy);

        //Debug.Log("a");
        //Debug.Log(user_x);
        //Debug.Log(user_y);
        //Debug.Log(user_z);

        if (user_z < 520 && user_z > 340 && !calib)
        {
            if (move.magnitude < 2) flag = false;
            if (flag)
            {
                move.Normalize();
                float speed = 1f;
                fairy_x += move.x * speed;
                fairy_y += move.y * speed;

                this.transform.position = new Vector3(currentP.x - move.x * speed * Fairy_width / (br_x - tl_x), currentP.y - move.y * speed * Fairy_height / (br_y - tl_y), currentP.z);
            }
            else
            {
                if (move.magnitude > 10) flag = true;
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
        else if (Input.GetKeyDown(KeyCode.Return) && calib)
        {
            setPosition();
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
        //sw.WriteLine(position.x.ToString() + "," + position.y.ToString() + "," + position.z.ToString() + "," + length.x.ToString() + "," + length.y.ToString() + "," + length.z.ToString());
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
            fairy_x = float.Parse(csvDatas[1][0]);
            fairy_y = float.Parse(csvDatas[1][1]);
            fairy_z = float.Parse(csvDatas[1][2]);
            sr.Close();
            return true;
        }
        else
        {
            return false;
        }

    }
}
