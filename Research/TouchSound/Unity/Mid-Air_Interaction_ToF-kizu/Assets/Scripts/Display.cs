using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    private Text text;
    private int digit = 0;
    private string symbol = null;
    private float result;
    private bool first = true;
    private bool is_num = false;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = "0";
    }

    public void Update_display(string str)
    {
        if (str == ".") return;

        if (str == "C")  //reset
        {
            text.text = "0";
            Reset();
            return;
        }
        else if (str == "+" || str == "-" || str == "×" || str == "÷"|| str == "=")
        {

            if (first)
            {
                result = float.Parse(text.text);
                first = false;
            }
            else if(is_num)
            {
                if (symbol == "+")
                {
                    result += float.Parse(text.text);
                }
                else if (symbol == "-")
                {
                    result -= float.Parse(text.text);
                }
                else if (symbol == "×")
                {
                    result *= float.Parse(text.text);
                }
                else if (symbol == "÷")
                {
                    result /= float.Parse(text.text);
                }
            }

            if (str == "=")
            {
                text.text = result.ToString();
                Reset();
                return;
            }

            digit = 0;
            is_num = false;
            symbol = str;
            text.text = str;
            return;
        }

        is_num = true;

        if (digit >= 8) { return; }
        float current_num;
        if (digit == 0)
        {
            current_num = float.Parse(str);
            digit = 1;
        }
        else
        {
            current_num = float.Parse(text.text) * 10 + float.Parse(str);
            digit++;
        }

        text.text = current_num.ToString();
    }

    private void Reset()
    {
        symbol = null;
        is_num = false;
        first = true;
    }
}
