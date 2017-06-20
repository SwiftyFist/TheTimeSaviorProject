using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Shell_Pool : MonoBehaviour {

    [SerializeField]
    private Gun_Shell gunShellPrefab;
    [SerializeField]
    private Transform spawnpoint;
    [SerializeField]
    private float durationInScene;

    [SerializeField]
    private float minForce, maxForce, minAngle, maxAngle;

    private Rigidbody2D rb;

    private void Awake()
    {
        
    }


    public void ShootShell()
    {
        float force = Random.Range(minForce, maxForce);
        float angle = Random.Range(minAngle, maxAngle);
        Gun_Shell shell = gunShellPrefab.GetPooledInstance<Gun_Shell>();
        shell.transform.position = spawnpoint.position;
        shell.Shoot(force, angle);
        StartCoroutine(ReturnShellToPool(shell));
        //shell.transform.position = spawnpoint

    }

    private IEnumerator ReturnShellToPool(Gun_Shell shellToReturn)
    {
        yield return new WaitForSeconds(durationInScene);
        shellToReturn.ReturnToPool();
    }
}
