using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceVolume : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float minDistance = 1, maxDistance = 15; 

    [SerializeField]
    [Range(0,1)]
    private float audioVolume;

    void Update()
    {

        float dist = Vector2.Distance(transform.position, playerTransform.position);

        if(dist < minDistance)
        {
            audioSource.volume = audioVolume;
        }
        else if(dist > maxDistance)
        {
            audioSource.volume = 0;
            audioSource.enabled = false;
        }
        else
        {
            audioSource.enabled = true;
            audioSource.volume = audioVolume - ((dist - minDistance) / (maxDistance - minDistance));
        }
    
    }
}
