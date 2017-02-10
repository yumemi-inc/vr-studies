//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenSample : GrabControllerTarget {

	public Color inkColor;

	LineRenderer lineRenderer;
	List<Vector3> points = new List<Vector3>();

	void Start () {

		lineRenderer = gameObject.AddComponent<LineRenderer> ();
		lineRenderer.SetWidth( 0.02f, 0.02f );
		//lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended"));
		lineRenderer.material = new Material(Shader.Find("Particles/Additive (Soft)"));
		lineRenderer.SetColors ( inkColor, inkColor );
	}
	
	void Update () {

		// ラインレンダラーで線を描く
		if( base.isGrabbed ){
			var pt = transform.position + new Vector3( 0.03f, 0, -0.01f );
			points.Add ( pt );
			lineRenderer.SetVertexCount( points.Count );
			lineRenderer.SetPositions( points.ToArray() );
		}
	}
}
