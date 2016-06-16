using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthBar : HealthControl
{
    public Slider valTrack;
    public Slider healthTrack;
    public Image fillBar;
	
	// Update is called once per frame
	void Update ()
    {
        //adjust value bars.
        valTrack.value = Mathf.Pow(((float)parent.GetComponent<ControllerAttack>().GetDamage() / 20f), 4);
        healthTrack.value = (float)((float)health / (float)maxHealth);

        //fade to show ability cooldown.
        if (parent.GetComponent<ControllerAttack>().GetCooldownRatio() >= 1)
        {
            fillBar.color = Color.cyan;
        }
        else
        {
            fillBar.color = Color.green;
        }
        fillBar.color =  new Color(fillBar.color.r, fillBar.color.g, fillBar.color.b, parent.GetComponent<ControllerAttack>().GetCooldownRatio());
    }
}
