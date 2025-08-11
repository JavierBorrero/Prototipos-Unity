using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForware : MonoBehaviour
{

    public float speed = 40;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
