using System.Collections;
using System.Collections.Generic;
using Features.Player.Logic.States;
using UnityEngine;

public class ObjectLayering : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        var objectRenderers = FindObjectsOfType<SpriteRenderer>();
        foreach (var objectRenderer in objectRenderers)
        {
            if (objectRenderer.sortingLayerName.Equals("Beach"))
            {
                objectRenderer.sortingOrder = (int)(objectRenderer.transform.position.y * -100);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerSpriteRenderer.sortingOrder = (int)(playerSpriteRenderer.transform.position.y * -100);
    }
}
