using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRStudies { namespace SinglePlayer {

public class Avatar : MonoBehaviour {

	//------------------------------------------------------------------------------------------------------------------------------//
	void Update () {

		// キーボード入力による移動処理
		var v = Input.GetAxis ("Vertical");
		Vector3 velocity = new Vector3 (0, 0, v);
		velocity = transform.TransformDirection (velocity);
		velocity *= 5f;
		transform.localPosition += velocity * Time.fixedDeltaTime;

		// キーボード入力による回転処理
		var h = Input.GetAxis ("Horizontal");
		transform.Rotate (0, h * 3f, 0);
	}
}

}}