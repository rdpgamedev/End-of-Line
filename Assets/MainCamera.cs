using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour 
{
    /*** Public Fields ***/
    public GameObject cameraTarget;

    private Vector3 offset;
    private float elapsedShake = 0.0f;
    private float durationShake = 0.0f;
    private float seed;
    private float intensity;
    private float targetHeight;

    public void Shake(float duration, float intensity)
    {
        durationShake = duration;
        this.intensity = intensity;
        elapsedShake = 0.0f;
        seed = Random.Range(-1.0f, 1.0f);
    }

    void Start () 
    {
        offset = Vector3.zero;
        targetHeight = cameraTarget.transform.position.y;
    }
	
    void Update () 
    {
        if (elapsedShake < durationShake)
        {
            elapsedShake += Time.deltaTime;
            UpdateShake();
        }
    }

    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        transform.position = new Vector3(cameraTarget.transform.position.x, 
                                         targetHeight, 
                                         transform.position.z)
                             + offset;
    }

    void UpdateShake()
    {
        float t = elapsedShake / durationShake;
        float damp = 1.0f - t * t;
        if (t > 1.0f) t = 1.0f;
        float x = Mathf.PerlinNoise(t, 0);
        x = 2.0f * x - 1.0f;
        float y = Mathf.PerlinNoise(0, t);
        y = 2.0f * y - 1.0f;
        offset = new Vector3(x, y, 0.0f) * intensity * damp;
    }
}
