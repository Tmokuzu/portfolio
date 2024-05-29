using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using UnityEngine.SceneManagement;

public class Buttons_for_menu : MonoBehaviour
{
    // Start is called before the first frame update
    private Text text;
    private Color col;
    private bool highlight = false;
    private bool is_touch = false;
    [SerializeField] UDPReceive udp;
    [SerializeField] Menuscene menu;
    [SerializeField] Image img;
    [SerializeField] Sprite img1;
    [SerializeField] Sprite img2;
    [SerializeField] Sprite img3;
    private Sprite[] sprites = new Sprite[3];
    [SerializeField] Image gage;
    private float countTime = 0.4f;
    private int state = 0;

    private Vector3Int position;
    private Vector3Int length;
    private string _name;
    private string sceneName;
    private StreamWriter sw;
    private StreamReader sr;
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    private GameObject MenuScene;
    private Menuscene Ms;

    public void Start()
    {
        MenuScene = GameObject.Find("MenuScene");
        Ms = MenuScene.GetComponent<Menuscene>();
        
        if(!ReadValue())
        {
            position.x = 0;
            position.y = 0;
            position.z = 0;
            length.x = 20;
            length.y = 20;
            length.z = 20;
        }

        col = img.color;
        text = GetComponentInChildren<Text>();
        sprites[0] = img1;
        sprites[1] = img2;
        sprites[2] = img3;
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
                highlight = true;
                if (this.tag == "menu")
                {
                    img.color = col;
                    menu.go_count(sprites[state]);
                }
                else if(this.tag == "back")
                {
                    change_state(false);
                }
                else if (this.tag == "next")
                {
                    change_state(true);
                }
                else if(this.tag == "count")
                {
                    GameObject.Find("x1").GetComponent<Text>().text = "x " + text.text;
                }
                else if(this.tag == "ok")
                {
                    img.color = col;
                    GameObject.Find("x1").GetComponent<Text>().text = "x 1";
                    menu.go_Order();
                }

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

    private void change_state(bool is_next)
    {
        if(is_next)
        {
            state++;
            if (state == sprites.Length) state = 0;
        }
        else
        {
            state--;
            if (state == -1) state = sprites.Length - 1;
        }
        menu.set_State(state);
    }
    private int get_State()
    {
        return state;
    }
    public void set_State(int s)
    {
        state = s;
        img.sprite = sprites[state];
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
        if (!Ms.Get_is_Calib()) return;
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
