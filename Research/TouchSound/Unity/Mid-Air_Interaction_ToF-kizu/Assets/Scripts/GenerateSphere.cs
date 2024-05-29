using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSphere: MonoBehaviour
{
    GameObject obj;

    // Use this for initialization
    void Start()
    {
        // CubeプレハブをGameObject型で取得
        obj = (GameObject)Resources.Load("prefab/Sphere");
    }

    private void Update()
    {
        if (this.transform.childCount < 150)
        {
            // Cubeプレハブを元に、インスタンスを生成、
            Instantiate(obj, new Vector3(Random.Range(-9f,9f), 6.5f, 0.0f), Quaternion.identity).transform.parent = this.transform;
        }
    }

}