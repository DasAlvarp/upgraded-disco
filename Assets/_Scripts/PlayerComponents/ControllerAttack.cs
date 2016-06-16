using UnityEngine;
using System.Collections;

public class ControllerAttack : MonoBehaviour
{
    //point placed, and current front of line.
    public Transform point;
    public Transform front;

    //effective hitbox of one line segment.
    public GameObject hitbox;

    //list of points
    ArrayList placedPoints;

    //list of line segments
    ArrayList connectingLines;

    //position seen
    Vector3 sightPos;

    //color of line
    public Material mat;

    //attack stats
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
        placedPoints = new ArrayList();
        connectingLines = new ArrayList();

        front = Instantiate(front);

        //time = time passed since activation, steps = number of opints placed.
        time = 0;
        steps = 0;

        front.position = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {

        sightPos = front.position ;//position is always set to the head.

        front.gameObject.SetActive(on);//always the same as the active setting.
        
        //attack button.
        if (cooldown < 0 && Mathf.Abs(Input.GetAxisRaw("rightJoyHorizontal")) + Mathf.Abs(Input.GetAxisRaw("rightJoyVertical")) > .5f)
        {
            //first change
            if(on == false)
            {
                front.position = transform.position;
                on = true;
            }

            //increment time
            time += Time.deltaTime;

            //movement is always the same.
            front.Translate(new Vector3(Input.GetAxis("rightJoyHorizontal"), 0,Input.GetAxis("rightJoyVertical")) * attackSpeed *Time.deltaTime);

            //place point at times. Reset time every placement.
            if (time > .02)
            {
                time = 0;
                steps++;
                placedPoints.Add(Instantiate(point, sightPos, Quaternion.identity));
            }
        }
        else if(on == true)//just let go of joystick. Draw stuff.
        {
            on = false;
            //last point shoiuld def be there.
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
        //for fade.
        return (baseCooldown - cooldown) / baseCooldown;
    }


    IEnumerator DrawLine()
    {
        //create line
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.material = mat;
        lineRenderer.SetWidth(.5F, .5F);

        lineRenderer.SetVertexCount(placedPoints.Count);

        //placing verticies
        for (int x = 0; x < placedPoints.Count; x++)
        {
            Vector3 vec = ((Transform)placedPoints[x]).position;
            lineRenderer.SetPosition(x, vec);
        }

        //rectange acts as hitbox. Larger, but that's ok, since aiming a thin line is tough.
        for(int x = 0; x < placedPoints.Count - 1; x++)
        {
            GameObject line = (GameObject)Instantiate(hitbox, (((Transform)placedPoints[x]).transform.position + ((Transform)placedPoints[x + 1]).transform.position) / 2, Quaternion.identity);
            line.transform.LookAt(((Transform)placedPoints[x]).transform.position);
            line.transform.localScale = new Vector3(1, 1, (((Transform)placedPoints[x]).transform.position - ((Transform)placedPoints[x + 1]).transform.position).magnitude);

            connectingLines.Add(line);
        }

        //deals damage
        DoDamage(connectingLines);

        //reset things.
        steps = 0;

        yield return new WaitForSeconds(1f);

        Destroy(lineRenderer);

        //destroy a lot of things.
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

        front.position = transform.position;
        sightPos = front.position;


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
            
            //couldn't think of a better way to do this. It's not pretty. Avert your eyes.
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
        //damage dealing algorithm. Eh. It's a thing.
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
