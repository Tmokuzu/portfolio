using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity;
using UnityEngine.SceneManagement;

public class UDPReceive_for_hand : MonoBehaviour
{
    int LOCAL_PORT = 12347;
    static UdpClient udp;
    Thread thread;
    public string parameter;

    public static Action<string> DataCallBack;

    private string left_hand;
    private string right_hand;

    void Start()
    {
        UDPReceive.DataCallBack += Method;
        UDPStart();

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

    private void ThreadMethod()
    {
        while (true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            string s = Encoding.ASCII.GetString(data);
            string[] arr = s.Split(',');
            left_hand = arr[0];
            right_hand = arr[1];
            Debug.Log("right: " + right_hand);
        }
        
    }

    public bool is_Palm()
    {
        return left_hand == "OpenPalm" || right_hand == "OpenPalm";
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("left: " + left_hand);
        }
    }

}