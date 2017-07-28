//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class EyeControllerTarget : MonoBehaviour, EyeController.IEyeControllerTarget {

	Color _color;

	void Awake() {

		// 最初はrigid同士が干渉しないようにする
		var rigid = gameObject.GetComponent<Rigidbody>();
		rigid.isKinematic = true;

		//自身の属性をランダムに変更
		var radius = Random.Range ( 0.25f, 1.5f );
		var scale = new Vector3 ( radius, radius, radius );
		var pos = new Vector3 ( Random.Range( -5.0f, 5.0f ), Random.Range( scale.y * 0.5f, 10.0f ), Random.Range( -5.0f, 5.0f ) );
		var rotation = new Vector3( Random.Range( 0.0f, 90.0f ), Random.Range( 0.0f, 90.0f ), Random.Range( 0.0f, 90.0f ) );
		_color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));

		gameObject.transform.localPosition = pos;
		gameObject.transform.localScale = scale;
		gameObject.transform.Rotate( rotation );
		gameObject.GetComponent<MeshRenderer>().material.color = _color;
	}

	public void OnEyeContollerHit( bool isOn ) {

		// 視線マーカーがヒットしたら色を変える
		gameObject.GetComponent<Renderer> ().material.color = isOn ? new Color ( 1, 1, 0 ) : _color;
	}

	public void OnEyeContollerClick() {

		// 視線マーカーでクリックしたら重力を付加する
		var rigid = gameObject.GetComponent<Rigidbody>();
		rigid.isKinematic = false;
		rigid.useGravity = true;
		rigid.mass = 5.0f;

		var colider = gameObject.GetComponent<Collider> ();
		colider.material.dynamicFriction = 0.95f;
		colider.material.bounciness = 0.99f;
	}
}
