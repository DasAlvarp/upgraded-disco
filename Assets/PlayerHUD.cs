using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUD : HealthControl {
    public Text hp;
    public Text power;
    public Slider valTrack;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        hp.text = health + "";
        power.text = parent.gameObject.GetComponent<AttackLine>().GetDamage() + "";
        valTrack.value = Mathf.Pow(((float)parent.gameObject.GetComponent<AttackLine>().GetDamage() / 20f), 4);


    }
}
