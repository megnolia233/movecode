using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 取消弹性 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        PhysicsMaterial2D material = new PhysicsMaterial2D();
        material.bounciness = 0;
        collider.sharedMaterial = material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
