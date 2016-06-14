using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUD : HealthControl {
    public Text hp;
    public Text power;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        hp.text = health + "";
        power.text = parent.gameObject.GetComponent<AttackLine>().GetDamage() + "";
	}
}
