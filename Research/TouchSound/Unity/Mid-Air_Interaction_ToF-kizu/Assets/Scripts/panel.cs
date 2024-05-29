using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panel : MonoBehaviour
{
    [SerializeField] UDPReceive udp;
    private Image img;
    private int user_x = 0, user_y = 0, user_z = 0;
    private Color col;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        col = img.color;
    }

    // Update is called once per frame
    void Update()
    {

        this.Set_UserPos();

        if(user_z < 480)
        {
            img.color = new Color(col.r, col.g, col.b, col.a);
        }
        else
        {
            img.color = new Color(col.r, col.g, col.b, 0);
        }


    }

    private void Set_UserPos()
    {
        Vector3Int user = udp.getNearPoint();
        user_x = user.x;
        user_y = user.y;
        user_z = user.z;
    }
}
