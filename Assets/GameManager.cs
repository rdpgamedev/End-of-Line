using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

/*** Static Public Fields ***/
    static public GameManager instance;

/*** Public Fields ***/
    public GameObject platformPreFab;
    public GameObject mainCamera;
    public GameObject startingPlatform;

/*** Public Tweakables ***/
    [Range(0.0f, 20.0f)]
    public float minimumPlatformLength = 5.0f;
    [Range(20.0f, 40.0f)]
    public float maximumPlatformLength = 30.0f;

/*** Properties ***/
    public Platform ActivePlatform
    {
        get{ return activePlatform; }
    }
    public Platform NextPlatform
    {
        get{ return nextPlatform; }
    }

/*** Private Fields ***/
    private LinkedList<Platform> platforms;

    private Platform activePlatform;
    private Platform nextPlatform;

    public void GrabPlatform()
    {
        platforms.RemoveFirst();
        activePlatform = platforms.First.Value;
        nextPlatform = platforms.First.Next.Value;
    }

    public void AddPlatform()
    {
        Platform lastPlatform = platforms.Last.Value;
        float jumpSpeed = PlayerController.instance.jumpingSpeed;
        Vector3 startPos = lastPlatform.EndPoint + jumpSpeed * Vector2.right;
        startPos += lastPlatform.transform.position;

        GameObject newPlatform = Instantiate(platformPreFab, startPos, transform.rotation);

        float length = Random.Range(minimumPlatformLength, maximumPlatformLength);
        newPlatform.GetComponent<Platform>().Init(length);

        platforms.AddLast(newPlatform.GetComponent<Platform>());
    }

    void Start () 
    {
        instance = this;
        platforms = new LinkedList<Platform>();

        // Add dummy platform
        platforms.AddFirst(Instantiate(platformPreFab, transform).GetComponent<Platform>());
        platforms.First.Value.gameObject.SetActive(false);
        activePlatform = platforms.First.Value;

        // Add real platforms
        platforms.AddLast(startingPlatform.GetComponent<Platform>());
        nextPlatform = platforms.First.Next.Value;
        AddPlatform();
        AddPlatform();
    }

    void Update () 
    {

    }

// Currently not used
    /*
    void CheckLastPlatform()
    {
        // Get right side of screen in world coords
        Camera cam = mainCamera.GetComponent<Camera>();
        Vector3 screenRight = new Vector3(cam.pixelWidth, 
                                          cam.pixelHeight/2, 
                                          cam.nearClipPlane);
        Vector3 worldRight = cam.ScreenToWorldPoint(screenRight);
        
        // Check if last platform's end is half a screen's width away from appearing
        if ((platforms.Last.Value.EndPoint.x - worldRight.x) < worldRight.x/2)
        {
            AddPlatform();
        }
    }
    */
}
