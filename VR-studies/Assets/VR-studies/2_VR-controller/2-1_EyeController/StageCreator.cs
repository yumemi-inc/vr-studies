//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class StageCreator : MonoBehaviour {

	void Start () {
	
		// ステージ上のオブジェクトを大量複製する
		var sphere = transform.FindChild("Sphere").gameObject;
		var cube = transform.FindChild("Cube").gameObject;
		var capsule = transform.FindChild("Capsule").gameObject;

		for( int i = 0; i < 20; i++ ){
			var copied = GameObject.Instantiate (sphere);
			copied.transform.SetParent ( transform );
		}

		for( int i = 0; i < 20; i++ ){
			var copied = GameObject.Instantiate (cube);
			copied.transform.SetParent ( transform );
		}

		for( int i = 0; i < 20; i++ ){
			var copied = GameObject.Instantiate (capsule);
			copied.transform.SetParent ( transform );
		}
	}
}
