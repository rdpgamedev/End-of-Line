using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour 
{
    /*** Public Fields ***/
    public LineRenderer lineRenderer;
    public CapsuleCollider2D lineCollider;
    
    public Vector2 BeginPoint
    {
        get{ return transform.position; }
    }
    
    public Vector2 EndPoint
    {
        get{ return lineRenderer.GetPosition(1); }
    }

    private float length;

    public void Init(float length)
    {
        this.length = length;
        Vector3 endPoint = new Vector3(length, 0);
        lineRenderer.SetPosition(1, endPoint);

        UpdateCollider();
    }

    void Start () 
    {

    }

    void Update () 
    {
        // Check for deletion
        Camera cam = GameManager.instance.mainCamera.GetComponent<Camera>();
        Vector3 screenLeft = new Vector3(0,
                                         cam.pixelHeight/2,
                                         cam.nearClipPlane);
        Vector3 worldLeft = cam.ScreenToWorldPoint(screenLeft);
        if ((EndPoint.x + transform.position.x) < worldLeft.x)
        {
            GameManager.instance.AddPlatform();
            Destroy(this.gameObject);
        }
    }

    void UpdateCollider()
    {
        lineCollider.size.Set(length + 0.25f, lineCollider.size.y);
        lineCollider.offset.Set(length/2, 0);
    }
}
