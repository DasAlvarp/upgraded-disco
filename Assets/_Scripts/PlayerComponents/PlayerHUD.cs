using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUD : HealthControl
{
    public Slider valTrack;
    public Slider healthTrack;
	
	// Update is called once per frame
	void Update ()
    {
        valTrack.value = Mathf.Pow(((float)parent.gameObject.GetComponent<AttackLine>().GetDamage() / 20f), 4);
        healthTrack.value = (float)parent.GetComponent<CharacterCombatController>().health / (float)parent.GetComponent<CharacterCombatController>().maxHealth;
    }
}
