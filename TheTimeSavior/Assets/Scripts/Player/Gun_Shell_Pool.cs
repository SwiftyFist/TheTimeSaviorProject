using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Shell_Pool : PooledObject {

    [SerializeField]
    private Transform spawnpoint;

    [SerializeField]
    private float minForce, maxForce, minAngle, maxAngle;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void ShootShell()
    {
        float force = Random.Range(minForce, maxForce);
        float angle = Random.Range(minAngle, maxAngle);
        Gun_Shell_Pool shell = GetPooledInstance<Gun_Shell_Pool>();
        //shell.transform.position = spawnpoint

    }
}
