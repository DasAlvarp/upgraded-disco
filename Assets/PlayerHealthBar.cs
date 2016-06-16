using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthBar : HealthControl
{
    public Slider valTrack;
    public Slider healthTrack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        valTrack.value = Mathf.Pow(((float)parent.GetComponent<AttackLine>().GetDamage() / 20f), 4);
        healthTrack.value = (float)((float)health / (float)maxHealth);

    }
}
