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
        
        //button_num = Int32.Parse(BUTTON.Replace("Space", ""));

        //Debug.Log(BUTTON.Replace("Space", ""));
        //if (BUTTON != null) button_num = int.Parse(BUTTON[BUTTON.Length - 1]);
        //else button_num = 0;
        //Debug.Log(BUTTON[BUTTON.Length - 1]);
        //distans[Int32.Parse(BUTTON.Replace("Space", ""))-1] = distan;

        //if(BUTTON == "Space1" | BUTTON == "Space2") distan = 0;
        List<object> values = new List<object>();
        values.AddRange(new object[]{distan});
        OSCHandler.Instance.SendMessageToClient("Puredata", "/Position", values);
        //Debug.Log(distan);
        //Debug.Log(BUTTON);
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
