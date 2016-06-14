using UnityEngine;
using System.Collections;

public class CharacterCombatController : BaseUnitComponent
{
    public Canvas centerUI;

	// Use this for initialization
	void Start () {
        centerUI = GameObject.FindObjectOfType<Canvas>();
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();
        DrawUI();
	}

    void Move()
    {
        transform.GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
    }

    void DrawUI()
    {
        centerUI.GetComponent<HealthControl>().health = health;
        centerUI.GetComponent<HealthControl>().parent = transform;

    }
}
