//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class LaserController : MonoBehaviour {

	protected GameObject laser;
	protected GameObject cursor;

	public float thickness = 0.002f;
	public float cursorSize = 0.04f;
	public Color laserColor = new Color( 1, 1, 0 );

	void Start() {

		//レーザーポインタを作成する
		CreateLaserPointer ();
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	protected void CreateLaserPointer () {
		
		laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
		laser.transform.SetParent(transform, false);
		laser.transform.localScale = new Vector3( thickness, thickness, 2.0f );
		laser.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
		laser.GetComponent<MeshRenderer>().material.color = laserColor;
		Object.DestroyImmediate(laser.GetComponent<BoxCollider>());

		cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		cursor.transform.SetParent(transform, false);
		cursor.transform.localScale = new Vector3( cursorSize, cursorSize, cursorSize );
		cursor.transform.localPosition = new Vector3(0.0f, 0.0f, 2.0f);
		cursor.GetComponent<MeshRenderer>().material.color = laserColor;
		Object.DestroyImmediate(cursor.GetComponent<SphereCollider>());
	}

	public void AdjustLaserDistance ( float distance ) {

		if( laser == null ){ return; }
		distance += 0.01f;

		//レーザーの長さを調整
		laser.transform.localScale = new Vector3( thickness, thickness, distance );
		laser.transform.localPosition = new Vector3( 0.0f, 0.0f, distance * 0.5f );
		cursor.transform.localPosition = new Vector3( 0.0f, 0.0f, distance );
	}
}
