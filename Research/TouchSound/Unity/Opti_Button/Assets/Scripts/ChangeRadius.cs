using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRadius : MonoBehaviour
{
    private string BUTTON;
    private float distan;

    private float MAX_DIS = 8;

    private Vector3 newRad;

    
    [SerializeField] MeasureDistance dis;
    [SerializeField] Collision col;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rad;
        this.Set_Distance();

        rad = 1 - distan/MAX_DIS;

        //newRad = (rad, 0f, rad);

        this.transform.localScale = new Vector3 (rad, 0.01f, rad);

        //Debug.Log(rad);

        
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
