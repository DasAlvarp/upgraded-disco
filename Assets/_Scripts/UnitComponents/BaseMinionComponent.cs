using UnityEngine;
using System.Collections;

public class BaseMinionComponent : BaseUnitComponent
{

	// Use this for initialization
	void Start () {
        healthBar = Instantiate(healthBar);

    }

    // Update is called once per frame
    void Update ()
    {
        SetHealtBar();
        if (health <= 0)
        {
            Destroy(healthBar);
            Destroy(gameObject);
        }
    }
}
