using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public string Col_Button;
    public string Col;
    public Vector3 point;

    public bool isCalledOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform myTransform = this.transform;
        point = myTransform.position;
        /*
        switch (Col){
            case "Space1": 
                Col_Button = 1;
                break;
            case "Space2": 
                Col_Button = 2;
                break;
            case "Space3": 
                Col_Button = 3;
                break;
            case "Space4": 
                Col_Button = 4;
                break;
            default : 
                Col_Button = 0;
                break;
        }
        */
        Col_Button = Col;
        //Debug.Log(Col_Button);
    }

    void OnTriggerExit(Collider collusion)
    {
        Col = string.Empty;
    }

    void OnTriggerEnter(Collider collision)
    {
        Col = collision.name;
        //Debug.Log(Col);
    }
    public string getButton()
    {
        //Debug.Log(Col_Button);
        return Col_Button;
    }
    public Vector3 getTransform()
    {
        return point;
    }
    public float getPositionZ()
    {
        return point.z;
    }
    public bool Called()
    {
        return isCalledOnce;
    }
}
