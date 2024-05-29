using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] UDPReceive udp;
    public void sceneChange(string scene)
    {
        switch(scene)
        {
            case "Cat":
                SceneManager.LoadScene("Cat");
                break;
            case "Drawing":
                SceneManager.LoadScene("Drawing");
                break;
            case "Peekaboo":
                SceneManager.LoadScene("PeekABoo");
                break;
            case "KeyPad":
                SceneManager.LoadScene("KeyPad");
                break;
            case "Fairy":
                SceneManager.LoadScene("Fairy");
                break;
            case "Menu":
                SceneManager.LoadScene("Menu");
                break;
            case "Burning":
                SceneManager.LoadScene("Burning");
                break;
            case "PseudoHaptics":
                SceneManager.LoadScene("PseudoHaptics");
                break;
            case "Start":
                SceneManager.LoadScene("Start");
                break;
            default:
                break;
        }

    }

    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("Start");
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Cat");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SceneManager.LoadScene("Drawing");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("PeekABoo");
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene("KeyPad");
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("Fairy");
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Menu");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene("Burning");
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene("PseudoHaptics");
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
}
