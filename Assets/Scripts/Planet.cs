using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float shrinkRate = 0.1f;
    public float minSize = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        if (transform.localScale.x > minSize)
            transform.localScale *= 1f - shrinkRate * Time.deltaTime;
    }
}
