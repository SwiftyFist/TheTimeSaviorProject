using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour {

    [SerializeField]
    private Transform shadow;
    [SerializeField]
    private LayerMask Layer_Ground;
    private RaycastHit2D distanceInfo;
    private float distanceFromTerrain;
	
	
	// Update is called once per frame
	void Update ()
    {
        distanceInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1.5f), Vector2.down, Mathf.Infinity, Layer_Ground);
        distanceFromTerrain = Vector2.Distance(transform.position, distanceInfo.point);
        shadow.localScale = new Vector2((distanceFromTerrain + 1f) * 2, distanceFromTerrain * 2);
        shadow.position = distanceInfo.point;
    }
}
