//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class PhysicsTeleporter : MonoBehaviour {

	SteamVR_Controller.Device head = null;

	public float climbableHeight = 0.5f;

	bool  isFalling = false;
	float fallEndY = 0;
	float fallVeloY = 0;

	GameObject shadow;

	//------------------------------------------------------------------------------------------------------------------------------//
	void Start () {

		head = SteamVR_Controller.Input( (int) SteamVR_TrackedObject.EIndex.Hmd );

		//プレイヤーの足元に影を作成
		shadow = GameObject.CreatePrimitive( PrimitiveType.Cylinder );
		shadow.transform.SetParent( transform, false );
		shadow.transform.localScale = new Vector3( 0.3f, 0.03f, 0.3f );
		shadow.transform.localPosition = new Vector3( head.transform.pos.x, 0, head.transform.pos.z );
		shadow.GetComponent<MeshRenderer>().material.color = Color.gray;
		Object.DestroyImmediate( shadow.GetComponent<CapsuleCollider>() );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void Update () {

		// 落下中の場合は重力アニメを付加
		if( isFalling ){
			fallVeloY += (Physics.gravity.y * Time.deltaTime);
			var newY = this.transform.position.y + fallVeloY * Time.deltaTime;
			if( newY <= fallEndY ){
				newY = fallEndY;
				isFalling = false;
			}
			this.transform.position = new Vector3( transform.position.x, newY, transform.position.z );
		}

		//影を足元に追随させる
		shadow.transform.localPosition = new Vector3( head.transform.pos.x, 0, head.transform.pos.z );
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void FixedUpdate () {

		// 現在の足元+頭のY位置を算出
		Vector3 eyePos = transform.position + head.transform.pos;

		// 真下へレイキャストして障害物の高さを調べる
		RaycastHit hitInfo;
		bool hasHit = Physics.Raycast ( eyePos, Vector3.down, out hitInfo, 100 );
		if ( hasHit ) {

			var offsetH = hitInfo.point.y - transform.position.y;
			if( isFalling == false ){

				// 相対的に登れる高さの場合は、足元のY位置を移動
				if ( offsetH > 0 && offsetH <= climbableHeight ) {

					transform.position = new Vector3 ( transform.position.x, hitInfo.point.y, transform.position.z );

				// 落下時は重力アニメを開始
				} else if( offsetH < 0 ) {

					fallEndY = hitInfo.point.y;
					fallVeloY = 0;
					isFalling = true;
				}
			}
		}
	} 
}
