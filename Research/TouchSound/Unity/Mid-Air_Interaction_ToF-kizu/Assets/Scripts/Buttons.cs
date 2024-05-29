using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    // Start is called before the first frame update
    private Text text;
    private Color col;
    private Image img;
    private bool highlight = false;
    private bool is_touch = false;
    [SerializeField] UDPReceive udp;
    [SerializeField] SceneChanger sc;
    [SerializeField] Image gage;
    private float countTime = 0.5f;

    private Vector3Int position;
    private Vector3Int length;
    private string _name;
    private string sceneName;
    private StreamWriter sw;
    private StreamReader sr;
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    private GameObject StartScene;
    private Startscene Ss;

    public void Start()
    {
        StartScene = GameObject.Find("StartScene");
        Ss = StartScene.GetComponent<Startscene>();
        
        if(!ReadValue())
        {
            position.x = 0;
            position.y = 0;
            position.z = 0;
            length.x = 20;
            length.y = 20;
            length.z = 20;
        }

        text = GetComponentInChildren<Text>();
        img = GetComponent<Image>();
        col = img.color;
        //gage.color = new Color(gage.color.r, gage.color.g, gage.color.b, 0);
    }
    public void Clicked(bool tf = true)
    {
        is_touch = tf;

        StopAllCoroutines();
        StartCoroutine(noAction());  //0.1秒間touchがなければ戻す
    }
    public void Update()
    {
        if (is_touch)
        {
            if (highlight)
            {
                ;
            }
            else if (gage.fillAmount < 1.0f)
            {
                gage.fillAmount += 1.0f / countTime * Time.deltaTime;
            }
            else
            {
                gage.fillAmount = 0;
                img.color = Color.blue;
                sc.sceneChange(text.text);
                highlight = true;
            }
        }
        else
        {
            img.color = col;
            highlight = false;
            if (gage.fillAmount > 0)
            {
                gage.fillAmount -= (1.0f / countTime * Time.deltaTime) / 2;
            }
        }
    }

    IEnumerator noAction()
    {
        yield return new WaitForSeconds(0.1f);
        is_touch = false;
    }

    public Vector3Int getPosition()
    {
        return position;
    }
    public Vector3Int getLength()
    {
        return length;
    }

    public void setPosition()
    {
        if (!Ss.Get_is_Calib()) return;
        Vector3Int user = udp.getNearPoint();

        sw = new StreamWriter(Application.persistentDataPath + "/PositionCSV/" + sceneName + "/" + _name + ".csv", false, Encoding.UTF8);

        sw.WriteLine("x,y,z,width,height,depth");

        sw.WriteLine(user.x.ToString() + "," + user.y.ToString() + "," + user.z.ToString() + "," + length.x.ToString() + "," + length.y.ToString() + "," + length.z.ToString());
        //sw.WriteLine(position.x.ToString() + "," + position.y.ToString() + "," + position.z.ToString() + "," + length.x.ToString() + "," + length.y.ToString() + "," + length.z.ToString());
        sw.Flush();
        sw.Close();
    }

    public bool ReadValue()
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
            position.x = int.Parse(csvDatas[1][0]);
            position.y = int.Parse(csvDatas[1][1]);
            position.z = int.Parse(csvDatas[1][2]);
            length.x = int.Parse(csvDatas[1][3]);
            length.y = int.Parse(csvDatas[1][4]);
            length.z = int.Parse(csvDatas[1][5]);
            sr.Close();
            return true;
        }
        else
        {
            return false;
        }
    }

}
