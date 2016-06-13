using UnityEngine;
using System.Collections;

public class CharacterCombatController : BaseUnitComponent
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();
	}

    void Move()
    {
        transform.GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
    }
}
