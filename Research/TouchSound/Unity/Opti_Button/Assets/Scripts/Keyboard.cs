using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    private bool isCalledOnce;
    private int key;

    [SerializeField] Collision col;
    [SerializeField] PushJudge_Tenkey tkey;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCalledOnce)
            {
                isCalledOnce = true;
            }
    }
    private void Set_Called()
    {
        isCalledOnce = col.Called();
    }
    private void Set_Key()
    {
        key = tkey.Get_PushedButton();
    }
}
