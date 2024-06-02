using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCSender : MonoBehaviour
{
    private string BUTTON;
    private long lastTimeStamp;
    private float distan;

    [SerializeField] MeasureDistance dis;
    [SerializeField] Collision col;

    // Start is called before the first frame update
    void Start()
    {
        OSCHandler.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        int button_num = 0;
        float[] distans = new float[9];

        this.Set_Distance();
        this.Set_Button();
        
        List<object> values = new List<object>();
        values.AddRange(new object[]{distan});
        OSCHandler.Instance.SendMessageToClient("Puredata", "/Position", values); //OSCでPureDataクライアントにPositionを送信
    }

    private void Set_Distance()
    {
        distan = dis.sendDistance();
    }
    private void Set_Button()
    {
        BUTTON = col.getButton();
    }
}
