using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushJudge : MonoBehaviour
{
    private string BUTTON;
    public Material PUSH;
    public Material ABOVE;
    public Material NONPUSH;

    public GameObject BUTTON1;
    public GameObject BUTTON2;
    public GameObject BUTTON3;
    public GameObject BUTTON4;
    public GameObject TOUCH2;
    public GameObject TOUCH4;

    public Renderer Button1Renderer;
    public Renderer Button2Renderer;
    public Renderer Button3Renderer;
    public Renderer Button4Renderer;
    public Renderer Touch2Renderer;

    //public Material BUTTON1;

    [SerializeField] Collision col;

    // Start is called before the first frame update
    void Start()
    {
        BUTTON1 = GameObject.Find("/3D Space/Canvas/Button1");
        BUTTON2 = GameObject.Find("/3D Space/Canvas/Button2");
        BUTTON3 = GameObject.Find("/3D Space/Canvas/Button3");
        BUTTON4 = GameObject.Find("/3D Space/Canvas/Button4");
        TOUCH2 = GameObject.Find("/3D Space/Canvas/Button2/Touch2");
        TOUCH4 = GameObject.Find("/3D Space/Canvas/Button4/Touch4");

        Button1Renderer = BUTTON1.GetComponent<Renderer>();
        Button2Renderer = BUTTON2.GetComponent<Renderer>();
        Button3Renderer = BUTTON3.GetComponent<Renderer>();
        Button4Renderer = BUTTON4.GetComponent<Renderer>();
        //Touch2Renderer = TOUCH2.GetComponent<Renderer>();

        //PUSH = Resources.Load<Material>("/Material/PushButton");

        //BUTTON1 = material.Find("/3D Space/Canvas/Button1");
    }

    // Update is called once per frame
    void Update()
    {
        this.Set_Button();

        switch (BUTTON){
            case "Space1": 
                Button1Renderer.material = ABOVE;
                break;
            case "Space2": 
                TOUCH2.SetActive(true);
                Button2Renderer.material = ABOVE;
                break;
            case "Space3": 
                Button3Renderer.material = ABOVE;
                break;
            case "Space4": 
                TOUCH4.SetActive(true);
                Button4Renderer.material = ABOVE;
                break;
            case "Button1":
                Button1Renderer.material = PUSH;
                break;
            case "Button2":
                Button2Renderer.material = PUSH;
                break;
            case "Button3":
                Button3Renderer.material = PUSH;
                break;
            case "Button4":
                Button4Renderer.material = PUSH; 
                break;
            default :
                TOUCH2.SetActive(false);
                TOUCH4.SetActive(false);
                Button1Renderer.material = NONPUSH;
                Button2Renderer.material = NONPUSH;
                Button3Renderer.material = NONPUSH;
                Button4Renderer.material = NONPUSH; 
                break;
        }
        //Debug.Log(BUTTON);
        
    }
    
    private void Set_Button()
    {
        BUTTON = col.getButton();
    }
    
}
