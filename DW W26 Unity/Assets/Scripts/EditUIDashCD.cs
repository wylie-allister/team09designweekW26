using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EditUIDashCD : MonoBehaviour
{
    public Canvas canvas;
    public Image overlay;
    public Image underlay;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>(); // the canvas this script is attached to
        Image[] images = canvas.GetComponentsInChildren<Image>(); // get the children of this canvas

        // search for and store the overlay and underlay objects.
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].gameObject.name == "Overlay")
            {
                overlay = images[i];
            }
            else if (images[i].gameObject.name == "Underlay")
            {
                underlay = images[i];
            }
        }
    }

    public void setCooldownProgress(float lastDash, float nextDash)
    {
        // update the overlay circle's fill amount to represent 0 fill when the dash was activated and 1 when the next dash is available
        overlay.fillAmount = (Time.time - lastDash) / (nextDash - lastDash);

        // make the cooldown UI more transparent the closer it is to complete
        overlay.color = new Color (1, 1, 1, 1-math.pow(overlay.fillAmount, 10));
        underlay.color = new Color (0, 0, 0, (1 - math.pow(overlay.fillAmount, 10)) *0.25f);
    }
}
