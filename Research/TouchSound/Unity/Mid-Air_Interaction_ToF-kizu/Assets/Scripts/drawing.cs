using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using OpenCVForUnity;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class drawing : MonoBehaviour
{
    [SerializeField] UDPReceive udp;
    [SerializeField] SceneChanger sc;

    private int user_x = 0, user_y = 0, user_z = 0;
    private int tl_x = 0, tl_y = 0, br_y = 0, br_x = 0;
    private int display_tlx, display_tly, display_brx, display_bry;
    private bool nothing = false;
    private Mat mat;

    private Texture2D cam_Texture;
    [SerializeField] RawImage rawImg;
    private int height = 480, width = 640;
    private bool flag = false;
    private List<Scalar> color;
    private int col;
    private float countTime;

    private string _name;
    private string sceneName;
    private StreamWriter sw;
    private StreamReader sr;
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    private bool calib = false;
    [SerializeField] GameObject calibtext;

    void Start()
    {
        if(!ReadValue())
        {
            display_tlx = 205;
            display_tly = 145;
            display_brx = 380;
            display_bry = 280;
        }

        rawImg.texture = cam_Texture;
        mat = Mat.ones(height, width, CvType.CV_8UC3);
        mat.setTo(new Scalar(255, 255, 255));

        Scalar cyan = new Scalar(0, 255, 255);
        Scalar yellow = new Scalar(255, 255, 0);
        Scalar magenta = new Scalar(255, 0, 255);
        color = new List<Scalar>() { cyan, yellow, magenta };
        col = 0;
        countTime = 0;
    }

    private static int size = 20;
    private Vector2[] prev = new Vector2[size];
    private int count = 1;

    // Update is called once per frame
    void Update()
    {
        this.Set_UserPos();
        this.Set_RectPos();
        this.Set_isNothing();
        //if (nothing) sc.sceneChange("Start");
        //Debug.Log(user_x);
        //Debug.Log(user_y);
        //Debug.Log(user_z);
        if (countTime < 1)
        {
            countTime += Time.deltaTime;
            return;
        }
        if (calib) return;

        if (user_x != 0 && !flag) create_canvas(Mathf.Abs(display_bry - display_tly), Mathf.Abs(display_brx - display_tlx));
        if (user_x > display_brx || user_x < display_tlx || user_y > display_bry || user_y < display_tly) return;

        if (user_z > 380 && user_z < 460)
        {
            Imgproc.circle(mat, new Point((display_brx - user_x), (display_bry - user_y)), 3, color[col%3], -1);
            //Imgproc.rectangle(mat, new Point(mat.cols() - (user_x - tl_x), mat.cols() - (user_x - tl_x)), new Point(mat.cols() - (user_x - tl_x)+10, mat.cols() - (user_x - tl_x)+10), new Scalar(255, 0, 0),2);
        }

        MonoBehaviour.Destroy(cam_Texture);
        cam_Texture = new Texture2D(mat.width(), mat.height(), TextureFormat.RGB24,false);
        Utils.matToTexture2D(mat, this.cam_Texture,false);
        rawImg.texture = this.cam_Texture;
    }

    public void LateUpdate()
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
        else if (Input.GetKeyDown(KeyCode.Alpha1) && calib)
        {
            setPosition_topleft();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && calib)
        {
            setPosition_bottomright();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && !calib)
        {
            col++;
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

    private void create_canvas(int height, int width)
    {
        mat = Mat.ones(height, width, CvType.CV_8UC3);
        mat.setTo(new Scalar(255, 255, 255));
        flag = true;
    }


    private void setPosition_topleft()
    {
        Vector3Int user = udp.getNearPoint();

        sw = new StreamWriter(Application.persistentDataPath + "/PositionCSV/" + sceneName + "/" + _name + ".csv", false, Encoding.UTF8);

        sw.WriteLine("TopLeft_x,TopLeft_y,BottomRight_x,BottomRight_y");
        sw.WriteLine(user.x.ToString() + "," + user.y.ToString() + "," + display_brx.ToString() + "," + display_bry.ToString());
        sw.Flush();
        sw.Close();
    }

    private void setPosition_bottomright()
    {
        Vector3Int user = udp.getNearPoint();

        sw = new StreamWriter(Application.persistentDataPath + "/PositionCSV/" + sceneName + "/" + _name + ".csv", false, Encoding.UTF8);

        sw.WriteLine("TopLeft_x,TopLeft_y,BottomRight_x,BottomRight_y");
        sw.WriteLine(display_tlx.ToString() + "," + display_tly.ToString() + "," + user.x.ToString() + "," + user.y.ToString());
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
            display_tlx = int.Parse(csvDatas[1][0]);
            display_tly = int.Parse(csvDatas[1][1]);
            display_brx = int.Parse(csvDatas[1][2]);
            display_bry = int.Parse(csvDatas[1][3]);
            sr.Close();
            return true;
        }
        else
        {
            return false;
        }
    }

}
