using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.IO.Ports;


public class SerialReceive : MonoBehaviour
{
    
    //private float xg = 2050;
    //private float yg = 2050;
    //private float zg = 2050;
    //private int value = 0;
    AudioSource audioSource;
    private SerialPort serialPort;
    public float Distance = 0;
    // Start is called before the first frame update
    void Start()
    {
        serialPort = new SerialPort("COM5", 9600); // ここを自分の設定したシリアルポート名に変えること
        serialPort.DtrEnable = true;
        serialPort.RtsEnable = true;
        serialPort.Open();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (serialPort.IsOpen)
        {
            string data = serialPort.ReadLine();
            Distance = float.Parse(data);
            //if (count < 10){
                //dat = dat + data;
                //ave = dat/count;
            //}
            //string[] datas = data.Split(','); // 追加

            //double xg = (data[0]*3.3/4096-1.65)/0.66;
            //double yg = (data[1]*3.3/4096-1.65)/0.66;
            //double zg = (data[2]*3.3/4096-1.65)/0.66;
            //xg = float.Parse(datas[0]);
            //yg = float.Parse(datas[1]);
            //zg = float.Parse(datas[2]);
            //value = int.Parse(datas[3]);

            //Debug.Log(Distance);
            //Debug.Log("X:" + xg + "Y:" + yg + "Z:" + zg + "VAL:" + value);
            //Debug.Log(datas[1]);
            //Debug.Log(datas[2]);
            //count++;
        }
    }
    public float getPoint()
    {
        return Distance;
    }
}
