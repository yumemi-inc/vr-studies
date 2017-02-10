//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour {

	//------------------------------------------------------------------------------------------------------------------------------//
	void OnCollisionEnter( Collision collision ) {

		var other = collision.gameObject;
		var rigid = other.GetComponent<Rigidbody> ();
		if( rigid ){

			// 床に落ちたボールは削除する
			if( other.name.StartsWith( "Ball" ) || other.name.StartsWith( "Water" ) ){
				Object.Destroy ( other );
			}
		}
	}

	void OnTriggerExit(Collider other) {
	}

}
