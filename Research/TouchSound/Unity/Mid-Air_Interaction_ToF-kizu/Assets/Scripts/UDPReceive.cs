using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UDPReceive : MonoBehaviour
{
    int LOCAL_PORT = 12345;
    static UdpClient udp;
    Thread thread;
    public string parameter;

    public static Action<string> DataCallBack;

    private int width, height, near_x, near_y, near_z, rect_tl_x, rect_tl_y, rect_br_x, rect_br_y;
    private int nothing;

    private const int filtersize = 7;
    int[] near_x_data = new int[filtersize];
    int[] near_y_data = new int[filtersize];
    int[] near_z_data = new int[filtersize];
    int fixed_x = 0, fixed_y = 0, fixed_z = 0;

    void Start()
    {
        UDPReceive.DataCallBack += Method;
        UDPStart();
        width = 640;
        height = 480;
        near_x = 0;
        near_y = 0;
        rect_tl_x = 0;
        rect_tl_y = 0;
        rect_br_x = 0;
        rect_br_y = 0;
        nothing = 0;

        SceneManager.sceneUnloaded += OnSceneUnloaded;
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

    void OnSceneUnloaded(Scene scene)
    {
        thread.Abort();
        udp.Close();
    }

    private int count = 0;
    private int delay = 1;
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
            if (count >= filtersize) count = 0;
            near_x = Int32.Parse(arr[2]);
            near_y = Int32.Parse(arr[3]);
            near_z = Int32.Parse(arr[4]);
            near_x_data[count] = Int32.Parse(arr[2]);
            near_y_data[count] = Int32.Parse(arr[3]);
            near_z_data[count] = Int32.Parse(arr[4]);
            fixed_x = filter(fixed_x, near_x_data[count]);
            fixed_y = filter(fixed_y, near_y_data[count]);
            fixed_z = filter(fixed_z, near_z_data[count]);
            count++;

            rect_tl_x = Int32.Parse(arr[5]);
            rect_tl_y = Int32.Parse(arr[6]);
            rect_br_x = Int32.Parse(arr[7]);
            rect_br_y = Int32.Parse(arr[8]);
            nothing = Int32.Parse(arr[9]);
        }
    }
    
    private void LateUpdate()
    {

    }

    public Vector2Int getFrameSize()
    {
        return new Vector2Int(width, height);
    }

    public Vector3Int getNearPoint()
    {
        //return new Vector3Int(near_x, near_y, near_z);
        //return new Vector3Int(ave(near_x_data), ave(near_y_data), median(near_z_data));
        return new Vector3Int(fixed_x, fixed_y, median(near_z_data));
        //return new Vector3Int(median(near_x_data), median(near_y_data), median(near_z_data));
    }

    public Vector4 getRect()
    {
        return new Vector4(rect_tl_x, rect_tl_y, rect_br_x, rect_br_y);
    }
    public int get_is_Nothing()
    {
        return nothing;
    }

    public int median(int[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = i + 1; j < a.Length; j++)
            {
                if (a[i] > a[j])
                {
                    int x = a[i];
                    a[i] = a[j];
                    a[j] = x;
                }
            }
        }
        return a[a.Length / 2];
    }

    public int ave(int[] a)
    {
        int sum = 0;
        for (int i = 0; i < a.Length; i++)
        {
            sum += a[i];
        }
        return sum / a.Length;
    }

    //指数移動平均
    public int filter(int p, int c)
    {
        return (int)Mathf.Round((float)p * 0.9f + (float)c * 0.1f);
    }
}