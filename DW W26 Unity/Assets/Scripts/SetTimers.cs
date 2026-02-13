using UnityEngine;
using UnityEngine.UI;

public class SetTimers : MonoBehaviour
{
    public Canvas canvas;
    public Text textBurrow;
    public Text textField;
    
    void Start()
    {
        // hi i'm maddy and you're watching my 3am descent into madness. i'm not commenting this
        canvas = GetComponent<Canvas>();
        Text[] text = canvas.GetComponentsInChildren<Text>();
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].gameObject.name == "BurrowTimer")
            {
                textBurrow = text[i];
            }
            else if (text[i].gameObject.name == "FieldTimer")
            {
                textField = text[i];
            }
        }
    }

    public void SetTimersText(float remaining)
    {
        remaining = Mathf.Floor(remaining);
        string text;

        if (remaining >= 60)
        {
            text = $"{Mathf.Floor(remaining / 60)}:";
        }
        else
        {
            text = "0:";
        }

        if (remaining % 60 < 10)
        {
            text += $"0{remaining % 60}";
        }
        else
        {
            text += remaining % 60;
        }
    }
}
