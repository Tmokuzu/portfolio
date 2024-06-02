using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UDPReceive : MonoBehaviour
{
    int LOCAL_PORT = 12345;
    static UdpClient udp;
    Thread thread;
    public string parameter;

    public static Action<string> DataCallBack;

    private int width, height, near_x, near_y, val, rect_tl_x, rect_tl_y, rect_br_x, rect_br_y;
    private int nothing;

    void Start()
    {
        UDPReceive.DataCallBack += Method;
        UDPStart();
        width = 640;
        height = 480;
        near_x = 0;
        near_y = 0;
        val = 100;
        rect_tl_x = 0;
        rect_tl_y = 0;
        rect_br_x = 0;
        rect_br_y = 0;
        nothing = 0;
    }

    public void UDPStart()
    {
        udp = new UdpClient(LOCAL_PORT);
        thread = new Thread(new ThreadStart(ThreadMethod));

        thread.Start();
    }

    public void Method(string data)
    {
        parameter = data;
    }

    void OnApplicationQuit()
    {
        thread.Abort();
        udp.Close();
    }

    private void ThreadMethod()
    {
        while (true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            string s = Encoding.ASCII.GetString(data);
            string[] arr = s.Split(',');

            width = Int32.Parse(arr[1]);
            height = Int32.Parse(arr[0]);
            near_x = Int32.Parse(arr[2]);
            near_y = Int32.Parse(arr[3]);
            val = Int32.Parse(arr[4]);
            rect_tl_x = Int32.Parse(arr[5]);
            rect_tl_y = Int32.Parse(arr[6]);
            rect_br_x = Int32.Parse(arr[7]);
            rect_br_y = Int32.Parse(arr[8]);
            nothing = Int32.Parse(arr[9]);
        }
    }

    public Vector2Int getFrameSize()
    {
        return new Vector2Int(width, height);
    }

    public Vector3Int getNearPoint()
    {
        return new Vector3Int(near_x, near_y, val);
    }

    public Vector4 getRect()
    {
        return new Vector4(rect_tl_x, rect_tl_y, rect_br_x, rect_br_y);
    }
    public int get_is_Nothing()
    {
        return nothing;
    }

    public void SceneChange()
    {
        thread.Abort();
        udp.Close();
    }
}