using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using OpenCVForUnity;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class objects : MonoBehaviour
{
    [SerializeField] UDPReceive udp;
    [SerializeField] SceneChanger sc;

    private int user_x = 0, user_y = 0, user_z = 0;
    private int tl_x = 0, tl_y = 0, br_y = 0, br_x = 0;
    private int display_tlx, display_tly, display_brx, display_bry;
    private bool nothing = false;

    private static float tof_x_min = 230, tof_x_max = 390, tof_y_min = 130, tof_y_max = 270;
    private static float unity_x_min = -6.0f, unity_x_max = 6.0f, unity_y_min = -4.0f, unity_y_max = 4.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.Set_UserPos();
        this.Set_RectPos();
        this.Set_isNothing();

        Debug.Log(user_z);

        if (user_z > 360 && user_z < 480)
        {
            float x = (user_x - tof_x_min) / (tof_x_max - tof_x_min) * (unity_x_max - unity_x_min) + unity_x_min;
            float y = (user_y - tof_y_min) / (tof_y_max - tof_y_min) * (unity_y_max - unity_y_min) + unity_y_min;
            Vector3 pos = new Vector3(-x, -y, 0.0f);
            this.transform.position = pos;
        }

    }

    private void Set_UserPos()
    {
        Vector3Int user = udp.getNearPoint();
        user_x = user.x;
        user_y = user.y;
        user_z = user.z;
    }
    private void Set_RectPos()
    {
        Vector4 rect = udp.getRect();
        tl_x = (int)rect.x;
        tl_y = (int)rect.y;
        br_x = (int)rect.z;
        br_y = (int)rect.w;
    }
    private void Set_isNothing()
    {
        if (udp.get_is_Nothing() == 0)
        {
            nothing = false;
        }
        else
        {
            nothing = true;
        }
    }

}
