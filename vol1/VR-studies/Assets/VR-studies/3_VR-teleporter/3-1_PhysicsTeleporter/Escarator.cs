//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class Escarator : MonoBehaviour {

	public Vector3 direction = Vector3.up;
	public float   velocity = 0.002f;
	public float   duration = 2.0f;

	Vector3 startPos = Vector3.zero;

	void Start () {
		startPos = transform.position;
	}

	void Update () {

		// 設定に従って自身を移動する
		transform.position += direction * velocity;

		// 特定距離を移動したら引き返す
		var distance = Vector3.Distance ( startPos, transform.position );
		if( distance >= duration ){
			direction *= -1.0f; 
			startPos = transform.position;
		}
	}
}
