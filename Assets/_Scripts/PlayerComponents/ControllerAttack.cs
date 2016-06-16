using UnityEngine;
using System.Collections;

public class ControllerAttack : MonoBehaviour {
    public Transform point;
    public Transform front;

    public GameObject hitbox;

    ArrayList connectPoints;
    ArrayList placedPoints;

    ArrayList connectingLines;

    Vector3 sightPos;
    RaycastHit hit;

    public Material mat;

    public int intMax;
    public int attackSpeed;

    LineRenderer lineRenderer;

    float cooldown;
    float baseCooldown = 5f;

    float time;
    int steps;

    bool on = false;

	// Use this for initialization
	void Start ()
    {
        connectPoints = new ArrayList();
        placedPoints = new ArrayList();
        connectingLines = new ArrayList();

        front = Instantiate(front);

        time = 0;
        steps = 0;

    }

    // Update is called once per frame
    void Update ()
    {
        ///get raycast a bit above player so it looks better.
        Vector3 pos = Input.mousePosition;

        pos = Camera.main.ScreenToWorldPoint(pos);


        sightPos = front.position ;
        front.gameObject.SetActive(on);
        
        //attack button.
        if (cooldown < 0 && Mathf.Abs(Input.GetAxisRaw("rightJoyHorizontal")) + Mathf.Abs(Input.GetAxisRaw("rightJoyVertical")) > .5f)
        {
            //first change
            if(on == false)
            {
                front.position = transform.position;
                on = true;
            }


            time += Time.deltaTime;

            front.Translate(new Vector3(Input.GetAxis("rightJoyHorizontal"), 0,Input.GetAxis("rightJoyVertical")) * attackSpeed *Time.deltaTime);

            if (time > .02)
            {
                time = 0;
                steps++;
                connectPoints.Add(sightPos);
                placedPoints.Add(Instantiate(point, sightPos, Quaternion.identity));
            }

            //on release, draw stuff;

        }
        else if(on == true)//just let go of joystick.
        {
            on = false;
            connectPoints.Add(sightPos);
            placedPoints.Add(Instantiate(point, sightPos, Quaternion.identity));
            transform.GetComponent<CharacterCombatController>().health -= GetDamage();
            StartCoroutine(DrawLine());
            ResetCooldown();//reset cooldown on attack.
        }
        //update cooldown
        cooldown -= Time.deltaTime;
    }

    public float GetCooldownRatio()
    {
        return (baseCooldown - cooldown) / baseCooldown;
    }


    IEnumerator DrawLine()
    {
        //create line
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.material = mat;
        lineRenderer.SetWidth(.5F, .5F);


        lineRenderer.SetVertexCount(connectPoints.Count);

        //placing verticies
        for (int x = 0; x < connectPoints.Count; x++)
        {
            lineRenderer.SetPosition(x, (Vector3)connectPoints[x]);
        }

        //rectange acts as hitbox. Larger, but that's ok, since aiming a thin line is tough.
        for(int x = 0; x < connectPoints.Count - 1; x++)
        {
            GameObject line = (GameObject)Instantiate(hitbox, ((Vector3)connectPoints[x] + (Vector3)connectPoints[x + 1]) / 2, Quaternion.identity);
            line.transform.LookAt((Vector3)connectPoints[x]);
            line.transform.localScale = new Vector3(1, 1, ((Vector3)connectPoints[x] - (Vector3)connectPoints[x + 1]).magnitude);

            connectingLines.Add(line);
        }

        //deals damage
        DoDamage(connectingLines);

        //reset things.
        steps = 0;
        connectPoints.Clear();

        yield return new WaitForSeconds(1f);

        Destroy(lineRenderer);

        foreach(Transform block in placedPoints)
        {
            Destroy(block.gameObject);
        }
        foreach(GameObject hitb in connectingLines)
        {
            Destroy(hitb);
        }
        connectingLines.Clear();

        placedPoints.Clear();

        yield return null;
    }

    void ResetCooldown()
    {
        cooldown = baseCooldown;
    }

    public void DoDamage(ArrayList lr)
    {
        //get all enemies.
        GameObject[] withTags = GameObject.FindGameObjectsWithTag("Enemy");

        Bounds boundThing;

        //iterate through all line/enemy permutations. Might fix for efficency.
        foreach(GameObject line in lr)
        {
            boundThing = line.GetComponent<Collider>().bounds;
            
            foreach(GameObject tagged in withTags)
            {
                if (boundThing.Intersects(tagged.GetComponent<Collider>().bounds))
                {
                    tagged.GetComponent<BaseUnitComponent>().health -= GetDamage();
                }
            }
        }

    }

    public int GetDamage()
    {
        if (steps == 0)
            return 0;

        double retval = steps;
        retval -= 4;
        retval /= 5;
        retval *= -retval;
        retval += 20;

        if (retval <= 0)
            return 0;

        return (int)retval;
    }
}
