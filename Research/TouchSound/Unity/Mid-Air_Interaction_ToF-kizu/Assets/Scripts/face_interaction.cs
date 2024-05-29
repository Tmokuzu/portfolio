using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class face_interaction : MonoBehaviour
{
    private bool is_face = false;
    private int face_x = 0;
    private int face_x_prev = 0;
    private int face_y = 0;
    private int rect_width = 640;
    private int rect_height = 480;

    [SerializeField] UDPReceive_for_face udp_face;
    [SerializeField] UDPReceive udp;
    [SerializeField] SceneChanger sc;
    [SerializeField] Camera cam;
    [SerializeField] GameObject facial;
    private Animator animator;
    private float countTime = 0;
    private bool flag = false;
    private bool nothing = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.Set_isface();
        this.Set_face_pos();
        this.Set_rect_size();
        this.Set_isNothing();
        if (nothing)
        {
            //udp_face.SceneChange();
            //sc.sceneChange("Start");
        }

        countTime += Time.deltaTime;
        if (countTime > 1) flag = true;

        //this.move_cam(face_x);

        if (is_face)
        {
            countTime = 0;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Smile"))
        {
            is_face = false;
            return;
        }

        if (flag && is_face)
        {
            animator.SetBool("isface", true);
            flag = false;
        }

        //Debug.Log(is_face);
    }

    public float speed = 0.5f;  //カメラの移動速度
    public float radius = 2f; //円の大きさ

    float _x;
    float _z;
    private void move_cam(int x)
    {
        int x_max = 440;
        int x_min = 225;
        int center = (x_max + x_min) / 2;
        float max = 12.0f;
        float deg = this.transform.rotation.y * 180 / Mathf.PI;

        
        //if (Mathf.Abs(deg - (max * (x - center) / (x_max - center))) < 1f) return;
        if ( deg < max * (x - center)/(x_max - center) && deg < max)
        {
            this.transform.Rotate(0, 1.5f, 0);
        }
        else if( deg > max * (x - center) / (x_max - center) && deg > -max)
        {
            this.transform.Rotate(0, -1.5f, 0);
        }

        /*
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(0, -2f, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(0, 2f, 0);
        }
        */

        //Debug.Log(max * (x - center) / (x_max - center));
        Debug.Log(x);

        face_x_prev = x;
    }

    private void Set_isface()
    {
        is_face = udp_face.get_is_face();
    }
    private void Set_face_pos()
    {
        Vector2Int face_p = udp_face.get_face_pos();
        face_x = face_p.x;
        face_y = face_p.y;
    }
    private void Set_rect_size()
    {
        Vector2Int rect_s = udp_face.get_rect_size();
        rect_width = rect_s.x;
        rect_height = rect_s.y;
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
