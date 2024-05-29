using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureDistance : MonoBehaviour
{
    private string BUTTON;
    private Vector3 TrackPoint;
    private float TrackPointZ;
    [SerializeField] Collision col;
    //[SerializeField] Material above = default;
    public float DISTANCE; //0~19.4
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 ButtonTransform1 = GameObject.Find("/3D Space/Canvas/Button1").transform.position;
        //GameObject Button1 = GameObject.Find("/3D Space/Canvas/Button1");
        
    }

    // Update is called once per frame
    void Update()
    {
        this.Set_Button();
        //this.Set_TrackPoint();
        this.Set_TrackPointZ();
        //Debug.Log(BUTTON);
        //Debug.Log(ButtonTransform1.GetType());
        //Debug.Log(TrackPointZ);
        /*
        if(BUTTON == "Space1"){
            //this.GetComponent<MeshRenderer>().material = above;
            //DISTANCE = Vector3.Distance(TrackPoint, ButtonTransform1);
        }
        else if(BUTTON == "Space2"){
            
        }
        else if(BUTTON == "Space3"){
            
        }
        else if(BUTTON == "Space4"){
            
        }
        else {
            DISTANCE = 0;
        }
        */
        /*
        switch (BUTTON){
            case "Space1": 
                DISTANCE = 10 - TrackPointZ - 1.1f;
                break;
            case "Space2": 
                DISTANCE = 10 - TrackPointZ - 1.1f;
                break;
            case "Space3": 
                DISTANCE = 10 - TrackPointZ - 1.1f;
                break;
            case "Space4": 
                DISTANCE = 10 - TrackPointZ - 1.1f;
                break;
            default : 
                DISTANCE = 0;
                break;
        }
        */
        //Debug.Log(BUTTON);
        if(BUTTON.Contains("Space")) DISTANCE = 10 - TrackPointZ - 1.1f;
        else DISTANCE = 0;
        //Debug.Log(DISTANCE);
    }
    

    private void Set_Button()
    {
        BUTTON = col.getButton();
    }
    private void Set_TrackPoint()
    {
        TrackPoint = col.getTransform();
    }
    private void Set_TrackPointZ()
    {
        TrackPointZ = col.getPositionZ();
    }
    
    public float sendDistance()
    {
        return DISTANCE;
    }
}
