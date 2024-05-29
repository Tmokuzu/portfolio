using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class MyFileManager : MonoBehaviour
{
    public GameObject bird;
    public anim2 anim;
    private StreamWriter sw;
    
    void Start()
    {
        // ログファイルの作成
        MakeLog();

        // 初期設定の読み取り
        ReadSettings();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)) OutputLog();
    }    

    void OnDestroy()
    {        
        this.sw.Flush();
        this.sw.Close();
    }

    private void MakeLog()
    {
        if(!Directory.Exists("./Logs")) Directory.CreateDirectory("./Logs");
        this.sw = new StreamWriter(@"./Logs/pos-size.csv", true, Encoding.UTF8);
        string[] s1 = { "年月日", "時", "分", "秒", "posX", "posY", "sizeX", "sizeY" };
        string s2 = string.Join(",", s1);
        this.sw.WriteLine(s2);
        this.sw.Flush();
    }

    private void ReadSettings()
    {
        // 初期設定用のファイルがなければ、初期値を代入したファイルを生成する
        if(!File.Exists("./init_settings.csv"))
        {
            StreamWriter newSw = new StreamWriter(@"./init_settings.csv", true, Encoding.UTF8);
            string[] title = { "posX", "posY", "sizeX", "sizeY", "LeftEnd", "RightEnd", "jumpHeight", "birdSpeed", "operationSpeed"};
            string[] value = { "0", "0", "3", "3", "-1.7", "1.7", "2", "0.01", "0.02" };
            string tiedTitle = string.Join(",", title);
            string tiedValue = string.Join(",", value);
            newSw.WriteLine(tiedTitle);
            newSw.WriteLine(tiedValue);
            newSw.Flush();
            newSw.Close();
        }
        
        StreamReader sr = new StreamReader(@"./init_settings.csv",Encoding.UTF8);
        string line = "";
        for(int i=0; i<2; i++) line = sr.ReadLine(); // 2行目のみを読み取る。
        sr.Close();
        string[] buf = line.Split(',');
        
        bird.transform.position = new Vector3(float.Parse(buf[0]), float.Parse(buf[1]), 0);

        // z方向の大きさが小さすぎると描画がおかしくなるので、便宜上Maxを取る。
        bird.transform.localScale = new Vector3(float.Parse(buf[2]), float.Parse(buf[3]), Math.Max(float.Parse(buf[2]), float.Parse(buf[3])));

        anim.SetInitPosY(float.Parse(buf[1]));
        anim.SetLeftEnd(float.Parse(buf[4]));
        anim.SetRightEnd(float.Parse(buf[5]));
        anim.SetJumpHeight(float.Parse(buf[6]));
        anim.SetBirdSpeed(float.Parse(buf[7]));
        anim.SetOperationSpeed(float.Parse(buf[8]));
    }

    private void OutputLog()
    {
        String date = DateTime.Now.Year.ToString() +
                      DateTime.Now.Month.ToString() + 
                      DateTime.Now.Day.ToString();
        String hour = DateTime.Now.Hour.ToString(); 
        String minute = DateTime.Now.Minute.ToString();
        String second = DateTime.Now.Second + "." + DateTime.Now.Millisecond;
        
        string[] s1 = { date, hour, minute, second,
                        bird.transform.position.x+"",
                        bird.transform.position.y+"",
                        bird.transform.localScale.x+"",
                        bird.transform.localScale.y+"",};
        string s2 = string.Join(",", s1);
        this.sw.WriteLine(s2);
        this.sw.Flush();
    }
}
