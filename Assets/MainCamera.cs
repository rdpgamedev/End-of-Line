using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour 
{
    /*** Public Fields ***/
    public GameObject cameraTarget;

    void Start () 
    {

    }
	
    void Update () 
    {
        
    }

    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        transform.position = new Vector3(cameraTarget.transform.position.x, 
                                         transform.position.y, 
                                         transform.position.z);
    }
}
