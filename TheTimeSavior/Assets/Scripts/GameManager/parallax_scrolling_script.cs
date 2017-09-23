﻿using UnityEngine;
using System.Collections;

public class parallax_scrolling_script : MonoBehaviour {

	public Transform[] backgrounds;
	private float[] parallaxScale;
	public float smoothing = 1f;

	private Transform cam;
	private Vector3 previousCamPos;

	void Awake()
	{
		cam = Camera.main.transform;
	}

	void Start () 
	{
		previousCamPos = cam.position;

		parallaxScale = new float[backgrounds.Length];
		for (int i = 0; i < backgrounds.Length; i++) 
		{
            if (backgrounds[i] != null)
			    parallaxScale[i] = backgrounds[i].position.z * -1;
		}
	}
	

	void Update () 
	{
		for (int i = 0; i < backgrounds.Length; i++) 
		{
            if (backgrounds[i] == null) return;
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScale [i];
			float backgroundTargetPosX = backgrounds [i].position.x + parallax;
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX,backgrounds[i].position.y,backgrounds[i].position.z);
			backgrounds [i].position = Vector3.Lerp (backgrounds [i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}
		previousCamPos = cam.position;

	}
}
