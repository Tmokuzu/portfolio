using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCReceive : MonoBehaviour
{
    private long lastTimeStamp;
    public int Distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        OSCHandler.Instance.Init();
        lastTimeStamp = -1;
    }

    // Update is called once per frame
    void Update()
    {
        OSCHandler.Instance.UpdateLogs();

        foreach (KeyValuePair<string, ServerLog> item in OSCHandler.Instance.Servers){
            for (int i=0; i < item.Value.packets.Count; i++){
                if (lastTimeStamp < item.Value.packets[i].TimeStamp){
                    lastTimeStamp = item.Value.packets[i].TimeStamp;

                    //string address = item.Value.packets[i].Address;

                    var arg0 = item.Value.packets[i].Data[0];

                    Distance = (int)arg0;

                    //Debug.Log(address + ":" + arg0);
                    //Debug.Log(int.Parse(arg0));
                    //Debug.Log(Distance);
                }
            }
        }
    }
    public int getPoint()
    {
        return Distance;
    }
}
