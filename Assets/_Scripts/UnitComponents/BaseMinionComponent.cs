using UnityEngine;
using System.Collections;

public class BaseMinionComponent : BaseUnitComponent
{
    public Transform player;

	// Use this for initialization
	void Start () {
        healthBar = Instantiate(healthBar);
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update ()
    {
        Attack();
        Move();

        SetHealtBar();
        if (health <= 0)
        {
            Destroy(healthBar);
            Destroy(gameObject);
        }
    }

    public void Attack()
    {
        if(Mathf.Abs((player.position - transform.position).magnitude) < 2)
        {
            player.GetComponent<CharacterCombatController>().health -= 4;
        }
    }

    public void Move()
    {
        transform.LookAt(player);

        if (Mathf.Abs((player.position - transform.position).magnitude) > 2)
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
