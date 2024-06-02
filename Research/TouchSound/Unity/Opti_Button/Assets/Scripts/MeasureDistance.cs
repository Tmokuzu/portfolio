using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureDistance : MonoBehaviour
{
    private string BUTTON;
    private Vector3 TrackPoint;
    private float TrackPointZ;
    [SerializeField] Collision col;
    public float DISTANCE; //0~19.4

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.Set_Button();
        this.Set_TrackPointZ();
        
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
