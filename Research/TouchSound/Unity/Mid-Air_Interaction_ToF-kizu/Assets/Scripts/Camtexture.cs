using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using OpenCVForUnity;
using System.IO;

public class Camtexture : MonoBehaviour
{
    private Texture2D cam_Texture;
    public RawImage rawImg;
    [SerializeField] UDPReceive udp;
    private int width, height, near_x, near_y, val, rect_tl_x, rect_tl_y, rect_br_x, rect_br_y;

    void Start()
    {
        //cam_Texture = new Texture2D();
        rawImg.texture = cam_Texture;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int size = udp.getFrameSize();
        width = size.x;
        height = size.y;
        Vector3Int near_point = udp.getNearPoint();
        near_x = near_point.x;
        near_y = near_point.y;
        val = near_point.z;
        Vector4 r = udp.getRect();
        OpenCVForUnity.Rect rect = new OpenCVForUnity.Rect(new Point((int)r.x,(int)r.y),new Point((int)r.z,(int)r.w));


        Mat m = Mat.zeros( height, width, CvType.CV_8U);
        Imgproc.circle(m, new Point(near_x, near_y), 20, new Scalar(255, 0, 0));
        Imgproc.rectangle(m, new Point(rect.tl().x, rect.tl().y), new Point(rect.br().x, rect.br().y), new Scalar(255, 0, 0));

        MonoBehaviour.Destroy(cam_Texture);
        cam_Texture = new Texture2D(m.width(), m.height(), TextureFormat.RGB24, false);
        Utils.matToTexture2D(m, this.cam_Texture);
        rawImg.texture = this.cam_Texture;

        //Write to a file in the project folder
        //byte[] bytes = cam_Texture.EncodeToPNG();
        //File.WriteAllBytes(Application.dataPath + "/../chess"+leftright+".png", bytes);
    }

}