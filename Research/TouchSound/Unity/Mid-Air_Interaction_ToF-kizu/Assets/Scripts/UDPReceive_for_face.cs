using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity;
using UnityEngine.SceneManagement;

public class UDPReceive_for_face : MonoBehaviour
{
    int LOCAL_PORT = 50009;
    static UdpClient udp;
    Thread thread;
    public string parameter;

    public static Action<string> DataCallBack;

    private bool is_face;
    private int face_x = 0;
    private int face_y = 0;
    private int rect_width = 640;
    private int rect_height = 480;
    const int size = 3;

    bool[] faces = new bool[size];
    int[] face_xs = new int[size];
    int[] face_ys = new int[size];

    void Start()
    {
        UDPReceive.DataCallBack += Method;
        UDPStart();
        is_face = false;
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

    int count = 0;
    private void ThreadMethod()
    {
        while (true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            string s = Encoding.ASCII.GetString(data);
            string[] arr = s.Split(',');
            string yes = "yes";
            if (arr[0] == yes)
            {
                is_face = true;
            }
            else
            {
                is_face = false;
            }
            face_x = Int32.Parse(arr[1]);
            face_y = Int32.Parse(arr[2]);
            rect_width = Int32.Parse(arr[3]);
            rect_height = Int32.Parse(arr[4]);

            if (count >= size) count = 0;
            faces[count] = is_face;
            face_xs[count] = face_x;
            face_ys[count] = face_y;

            count++;
            Debug.Log(is_face);
        }
    }

    public bool get_is_face()
    {
        return median(faces);
    }
    public Vector2Int get_face_pos()
    {
        return new Vector2Int(median(face_xs), median(face_ys));
    }
    public Vector2Int get_rect_size()
    {
        return new Vector2Int(rect_width, rect_height);
    }

    public void SceneChange()
    {
        thread.Abort();
        udp.Close();
    }

    public bool median(bool[] a)
    {
        int count = 0;
        for(int i=0; i< a.Length; i++)
        {
          if(a[i])
            {
               count++;
            }
        }
        if (count >= a.Length / 2) return true;
        else return false;
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
}