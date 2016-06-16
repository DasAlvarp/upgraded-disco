using UnityEngine;
using System.Collections;

public class BaseMinionComponent : BaseUnitComponent
{
    public Transform player;
    public float attackTime;
    float time;

	// Use this for initialization
	void Start () {
        time = attackTime;
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
        time -= Time.deltaTime;
        if(Mathf.Abs((player.position - transform.position).magnitude) < 2 && time <= 0)
        {
            player.GetComponent<CharacterCombatController>().health -= 4;
            time = attackTime;
            StartCoroutine(Knockback(.5f, 5f));
            StartCoroutine(Push(player, transform.forward, .1f, 1f));
        }
    }

    public void Move()
    {
        transform.LookAt(player);

        if (Mathf.Abs((player.position - transform.position).magnitude) > 2)
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    IEnumerator Knockback(float time, float distance)
    {
        for(float passed = 0; passed < time; passed += Time.deltaTime)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * -distance/time);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Push(Transform player, Vector3 direction, float time, float distance)
    {
        for (float passed = 0; passed < time; passed += Time.deltaTime)
        {
            player.Translate(direction * Time.deltaTime * distance / time);

            yield return new WaitForEndOfFrame();
        }
    }
}
