using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Shell : PooledObject {

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(float angle, float force)
    {
        Vector2 newVector = new Vector2(0, -angle);
        rb.AddForce(newVector * force, ForceMode2D.Impulse);
    }

   
}
