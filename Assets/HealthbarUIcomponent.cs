using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthbarUIcomponent : HealthControl
{

    public Camera cam;

	// Use this for initialization
	void Start () {
        transform.SetParent(GameObject.Find("UICanvas").transform);
        cam = GameObject.FindObjectOfType<Camera>();

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(health > 0)
        {
            SetPos();
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(health, 1) * ( 100 / (parent.position - cam.transform.position).magnitude);
        }
	}

    void SetPos()
    {
        RectTransform CanvasRect = GameObject.Find("UICanvas").GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = cam.WorldToViewportPoint(parent.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        gameObject.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
    }
}
