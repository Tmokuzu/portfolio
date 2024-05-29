using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] UDPReceive_for_hand udp_hand;
    [SerializeField] ParticleSystem particle;
    [SerializeField] ParticleSystem spark;
    [SerializeField] ParticleSystem fire;
    private bool is_active = false;
    private float x_scale, y_scale;


    // Start is called before the first frame update
    void Start()
    {
        spark.gameObject.SetActive(false);
        fire.gameObject.SetActive(false);
        x_scale = particle.transform.localScale.x;
        y_scale = particle.transform.localScale.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (udp_hand.is_Palm() && !is_active)
        {
            StartCoroutine("Burning");
        }
    }

    IEnumerator Burning()
    {
        is_active = true;
        spark.gameObject.SetActive(true);
        fire.gameObject.SetActive(true);
        Vector3 s = new Vector3(2.5f, 2.5f, 1);
        particle.transform.localScale = s;

        yield return new WaitForSeconds(4);

        spark.gameObject.SetActive(false);
        fire.gameObject.SetActive(false);
        s = new Vector3(x_scale, y_scale, 1);
        particle.transform.localScale = s;

        yield return new WaitForSeconds(1);

        is_active = false;
    }

}
