using UnityEngine;
using System.Collections;

public class AttackLine : MonoBehaviour {
    public Transform point;
    ArrayList connectPoints;
    ArrayList placedPoints;
    Vector3 sightPos;
    RaycastHit hit;

    public Material mat;
    public Material mat2;

    LineRenderer lineRenderer;


    float time;
    int steps;

	// Use this for initialization
	void Start ()
    {
        connectPoints = new ArrayList();
        placedPoints = new ArrayList();

        time = 0;
        steps = 0;


    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 pos = Input.mousePosition;

        pos = Camera.main.ScreenToWorldPoint(pos);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(pos);

        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.GetComponent<Renderer>().material.color = Color.red;
            sightPos = hit.point + Vector3.up;
        }

        if (Input.GetButton("Fire1"))
        {
            if(Input.GetButtonDown("Fire1"))
            {
                connectPoints.Add(sightPos);
                placedPoints.Add(Instantiate(point, sightPos, Quaternion.identity));
            }
            time += Time.deltaTime;

            if(time > .05)
            {
                time = 0;
                steps++;
                connectPoints.Add(sightPos);
                placedPoints.Add(Instantiate(point, sightPos, Quaternion.identity));
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            connectPoints.Add(sightPos);
            placedPoints.Add(Instantiate(point, sightPos, Quaternion.identity));

            StartCoroutine(DrawLine());
        }


    }


    IEnumerator DrawLine()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.material = mat;
        lineRenderer.SetWidth(1F, 1F);


        lineRenderer.SetVertexCount(connectPoints.Count);

        for (int x = 0; x < connectPoints.Count; x++)
        {
            lineRenderer.SetPosition(x, (Vector3)connectPoints[x]);
        }



        
        steps = 0;
        connectPoints.Clear();

        yield return new WaitForSeconds(1f);

        Destroy(lineRenderer);

        foreach(Transform block in placedPoints)
        {
            Destroy(block.gameObject);
        }
        placedPoints.Clear();

        yield return null;
    }
}
