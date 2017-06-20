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

    private Transform rotation;

    private void Awake()
    {
        rotation = FindObjectOfType<arm_rotation_script>().transform;
    }


    public void ShootShell()
    {
        float force = Random.Range(minForce, maxForce);
        float angle = Random.Range(minAngle, maxAngle);
        Gun_Shell shell = gunShellPrefab.GetPooledInstance<Gun_Shell>();
        shell.transform.position = spawnpoint.position;
        shell.Shoot(angle, force, rotation.rotation);
        StartCoroutine(ReturnShellToPool(shell));
        //shell.transform.position = spawnpoint

    }

    private IEnumerator ReturnShellToPool(Gun_Shell shellToReturn)
    {
        yield return new WaitForSeconds(durationInScene);
        shellToReturn.ReturnToPool();
    }
}
