using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushJudge_Tenkey : MonoBehaviour
{
    private string BUTTON;
    public Material PUSH;
    public Material ABOVE;
    public Material NONPUSH;

    public GameObject BUTTON3_1;
    public GameObject BUTTON3_2;
    public GameObject BUTTON3_3;
    public GameObject BUTTON3_4;
    public GameObject BUTTON3_5;
    public GameObject BUTTON3_6;
    public GameObject BUTTON3_7;
    public GameObject BUTTON3_8;
    public GameObject BUTTON3_9;
    //public GameObject TOUCH2;
    //public GameObject TOUCH4;

    public Renderer Button31Renderer;
    public Renderer Button32Renderer;
    public Renderer Button33Renderer;
    public Renderer Button34Renderer;
    public Renderer Button35Renderer;
    public Renderer Button36Renderer;
    public Renderer Button37Renderer;
    public Renderer Button38Renderer;
    public Renderer Button39Renderer;
    //public Renderer Touch2Renderer;

    public int pushedButton = -1;

    //public Material BUTTON1;

    [SerializeField] Collision col;

    // Start is called before the first frame update
    void Start()
    {
        BUTTON3_1 = GameObject.Find("/3D Space/Canvas/Button3-1");
        BUTTON3_2 = GameObject.Find("/3D Space/Canvas/Button3-2");
        BUTTON3_3 = GameObject.Find("/3D Space/Canvas/Button3-3");
        BUTTON3_4 = GameObject.Find("/3D Space/Canvas/Button3-4");
        BUTTON3_5 = GameObject.Find("/3D Space/Canvas/Button3-5");
        BUTTON3_6 = GameObject.Find("/3D Space/Canvas/Button3-6");
        BUTTON3_7 = GameObject.Find("/3D Space/Canvas/Button3-7");
        BUTTON3_8 = GameObject.Find("/3D Space/Canvas/Button3-8");
        BUTTON3_9 = GameObject.Find("/3D Space/Canvas/Button3-9");

        Button31Renderer = BUTTON3_1.GetComponent<Renderer>();
        Button32Renderer = BUTTON3_2.GetComponent<Renderer>();
        Button33Renderer = BUTTON3_3.GetComponent<Renderer>();
        Button34Renderer = BUTTON3_4.GetComponent<Renderer>();
        Button35Renderer = BUTTON3_5.GetComponent<Renderer>();
        Button36Renderer = BUTTON3_6.GetComponent<Renderer>();
        Button37Renderer = BUTTON3_7.GetComponent<Renderer>();
        Button38Renderer = BUTTON3_8.GetComponent<Renderer>();
        Button39Renderer = BUTTON3_9.GetComponent<Renderer>();
        
        //Touch2Renderer = TOUCH2.GetComponent<Renderer>();

        //PUSH = Resources.Load<Material>("/Material/PushButton");

        //BUTTON1 = material.Find("/3D Space/Canvas/Button1");
    }

    // Update is called once per frame
    void Update()
    {
        this.Set_TenkeyButton();

        switch (BUTTON){
            case "Space3-1":
                Button31Renderer.material = ABOVE;
                break;
            case "Space3-2": 
                Button32Renderer.material = ABOVE;
                break;
            case "Space3-3": 
                Button33Renderer.material = ABOVE;
                break;
            case "Space3-4": 
                Button34Renderer.material = ABOVE;
                break;
            case "Space3-5": 
                Button35Renderer.material = ABOVE;
                break;
            case "Space3-6": 
                Button36Renderer.material = ABOVE;
                break;
            case "Space3-7": 
                Button37Renderer.material = ABOVE;
                break;
            case "Space3-8": 
                Button38Renderer.material = ABOVE;
                break;
            case "Space3-9": 
                Button39Renderer.material = ABOVE;
                break;
            
            case "Button3-1":
                pushedButton=1;
                Button31Renderer.material = PUSH;
                break;
            case "Button3-2":
                pushedButton=2;
                Button32Renderer.material = PUSH;
                break;
            case "Button3-3":
                pushedButton=3;
                Button33Renderer.material = PUSH;
                break;
            case "Button3-4":
                pushedButton=4;
                Button34Renderer.material = PUSH; 
                break;
            case "Button3-5":
                pushedButton=5;
                Button35Renderer.material = PUSH; 
                break;
            case "Button3-6":
                pushedButton=6;
                Button36Renderer.material = PUSH; 
                break;
            case "Button3-7":
                pushedButton=7;
                Button37Renderer.material = PUSH; 
                break;
            case "Button3-8":
                pushedButton=8;Button38Renderer.material = PUSH; 
                break;
            case "Button3-9":
                pushedButton=9;
                Button39Renderer.material = PUSH; 
                break;
            
            default :
                //TOUCH2.SetActive(false);
                //TOUCH4.SetActive(false);
                pushedButton = -1;
                Button31Renderer.material = NONPUSH;
                Button32Renderer.material = NONPUSH;
                Button33Renderer.material = NONPUSH;
                Button34Renderer.material = NONPUSH; 
                Button35Renderer.material = NONPUSH;
                Button36Renderer.material = NONPUSH;
                Button37Renderer.material = NONPUSH;
                Button38Renderer.material = NONPUSH; 
                Button39Renderer.material = NONPUSH;
                break;
        
        }
        //Debug.Log(BUTTON);
        
    }
    
    private void Set_TenkeyButton()
    {
        BUTTON = col.getButton();
    }
    public int Get_PushedButton()
    {
        return pushedButton;

    }
    
}
