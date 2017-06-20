using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Shell : PooledObject {

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(float angle, float force, Quaternion startingRot)
    {
        Vector3 newVector3 = startingRot.eulerAngles;
        transform.rotation = Quaternion.Euler(newVector3.x, newVector3.y, (newVector3.z - 90) + angle);
        rb.AddForce(transform.up * (force / 100), ForceMode2D.Impulse);
    }

   
}
