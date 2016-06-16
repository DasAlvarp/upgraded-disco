using UnityEngine;
using System.Collections;

public class BaseUnitComponent : MonoBehaviour
{
    public int health;
    public int speed;

    public Transform headPoint;

    public GameObject healthBar;
    
	// Use this for initialization
	void Start ()
    {
        healthBar = Instantiate(healthBar);
    }

    // Update is called once per frame
    void Update ()
    {
        SetHealtBar();
    }

    //seting components
    public void SetHealtBar()
    {
        healthBar.GetComponent<HealthControl>().health = health;
        healthBar.GetComponent<HealthControl>().parent = headPoint;
    }
}
