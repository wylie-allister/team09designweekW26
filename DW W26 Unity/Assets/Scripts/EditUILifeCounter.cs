using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EditUILifeCounter : MonoBehaviour
{
    public Image counter;

    public float animTimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        counter = GetComponent<Image>();

    }

    public void setLifeCounter(int lives)
    {
        if (lives == 2)
        {
            counter.fillAmount = 0.62f;
            counter.color = new Color(1, 1, 0, 1);
        }
        else if (lives == 1)
        {
            counter.fillAmount = 0.38f;
            counter.color = new Color(1, 0, 0, 1);
        }
        else if (lives == 3)
        {
            counter.fillAmount = 1f;
            counter.color = new Color(0, 1, 0, 1);
        }
        animTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        animTimer -= Time.deltaTime;
        if (animTimer < 0) { animTimer = 0; }
        else if (animTimer > 0)
        {
            counter.color = new Color (counter.color.r, counter.color.g, counter.color.b, math.pow(-(1-animTimer), 10)+1);
        }
    }
}
