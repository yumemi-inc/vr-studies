//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class GrabControllerTarget : MonoBehaviour, GrabController.IGrabControllerTarget {

	public Rigidbody Rigidbody = null;
	protected bool isGrabbed = false;

	//------------------------------------------------------------------------------------------------------------------------------//
	void Awake () {
		if ( Rigidbody == null ) {
			Rigidbody =	this.GetComponent<Rigidbody> ();
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	public Rigidbody GetTargetRigidbody() {
		return Rigidbody;
	}	

	public void OnGrabControllerGrab( bool isOn ) {
		isGrabbed = isOn;
	}
}
