using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PseudoController : MonoBehaviour
{

    [SerializeField] UDPReceive udp;
    [SerializeField] GameObject panel;
    private int user_x, user_y, user_z;
    private int tl_x = 0, tl_y = 0, br_y = 0, br_x = 0;
    private int object_x, object_y, object_z;
    private Vector3 current_position;

    private static float tof_x_min = 220;
    private static float tof_x_max = 350;
    private float tof_range = tof_x_max - tof_x_min;

    private float force_rate = 8/8;
    private bool touch = false;

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
        if (!ReadValue())
        {
            object_x = 220;
            object_y = 220;
            object_z = 420;
        }

        current_position = panel.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.Set_UserPos();
        this.Set_RectPos();

        Debug.Log(user_x);
        Debug.Log(user_y);
        //Debug.Log(user_z);

        if (user_x < object_x+20 && user_x > object_x-10 && user_z < object_z + 50 && user_z > object_z - 50 && !touch && !calib) touch = true;

        if (user_x < object_x + 140 && user_x > object_x - 10 && user_z < object_z + 50 && user_z > object_z - 50 && touch && !calib)
        {
            if (current_position.x >= -400 && current_position.x <= 400)
            {
                panel.transform.position = new Vector3((user_x - tof_x_min)/tof_range*force_rate*800f - 400f, 0, 0);
                if(panel.transform.position.x < -400) panel.transform.position = new Vector3(-400f, 0, 0);
                if(panel.transform.position.x > 400) panel.transform.position = new Vector3(400f, 0, 0);
            }
        }
        else
        {
            touch = false;
            if (current_position.x > -400)
            {
                panel.transform.position = new Vector3(current_position.x - 12, 0, 0);
                if (panel.transform.position.x < -400) panel.transform.position = new Vector3(-400f, 0, 0);
            }
        }
        current_position = panel.transform.position;
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
                this.ReadValue();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) && calib)
        {
            setPosition();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            force_rate = 1f / 8f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            force_rate = 2f / 8f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            force_rate = 3f / 8f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            force_rate = 4f / 8f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            force_rate = 5f / 8f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            force_rate = 6f / 8f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            force_rate = 7f / 8f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            force_rate = 8f / 8f;
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
            object_x = int.Parse(csvDatas[1][0]);
            object_y = int.Parse(csvDatas[1][1]);
            object_z = int.Parse(csvDatas[1][2]);
            sr.Close();
            return true;
        }
        else
        {
            return false;
        }

    }
}
