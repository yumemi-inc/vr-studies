//
//  VR-Sphere
//  Created by miuccie miurror on 12/04/2016.
//  Copyright 2016 miuccie miurror
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PointController : LaserController {

	SteamVR_Controller.Device hand = null;

	GameObject jointObj = null;
	FixedJoint joint = null;

	RaycastHit hitInfo;
	GameObject hitObject;
	bool isClicking = false;

	//------------------------------------------------------------------------------------------------------------------------------//
	void Start() {

		SteamVR_TrackedObject handTO = transform.GetComponent<SteamVR_TrackedObject>();
		hand = SteamVR_Controller.Input( (int) handTO.index );

		//レーザーポインタを作成する
		base.CreateLaserPointer();

		// ジョイントをダミーカーソルにアタッチ
		jointObj = Instantiate( cursor ) as GameObject;
		jointObj.name = "DummyCursor";
		jointObj.GetComponent<MeshRenderer> ().enabled = false;
		joint = jointObj.AddComponent<FixedJoint> ();
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void FixedUpdate () {

		// 物理オブジェクトのヒットテスト
		bool hasHit = Physics.Raycast ( transform.position, transform.forward, out hitInfo, 100, 1 << LayerMask.GetMask("Ignore raycast") );
		if ( hasHit ) {

			//レーザーの長さを調節
			AdjustLaserDistance( hitInfo.distance );
			jointObj.transform.position = cursor.transform.position;
				
			//ターゲットが変更されたか
			if ( hitObject != hitInfo.collider.gameObject ) {

				// 以前のターゲットを無効に
				if ( hitObject ) {
					DispatchHitEvent( false );
					if( isClicking ){
						isClicking = false;
						DispatchClickEvent ( false );
					}
				}

				//ヒットイベント発行
				hitObject = hitInfo.collider.gameObject;
				DispatchHitEvent(true);

			} else {

				//トリガーを引いたらクリックイベントを1度だけ発行
				if ( isClicking == false && hand.GetPressDown (SteamVR_Controller.ButtonMask.Trigger) ) {
					isClicking = true;
					DispatchClickEvent ( true );
				}

				if ( isClicking == true && hand.GetPressUp (SteamVR_Controller.ButtonMask.Trigger) ) {
					isClicking = false;
					DispatchClickEvent ( false );
				}
			}

		} else {

			if( hitObject ){

				if( isClicking ){
					isClicking = false;
					DispatchClickEvent ( false );
				}

				DispatchHitEvent( false );
				hitObject = null;
			}
		}
	} 

	//------------------------------------------------------------------------------------------------------------------------------//
	public interface IPointControllerTarget {
		void OnPointControllerHit( bool isOn );
		void OnPointControllerClick( bool isOn );
	}

	void DispatchHitEvent ( bool isOn ) {
		if ( hitObject ) {
			var target = hitObject.GetComponent<IPointControllerTarget> ();
			if (target != null) {
				target.OnPointControllerHit ( isOn );
			}
		}
	}

	void DispatchClickEvent ( bool isOn ) {
		if (hitObject) {

			if (isOn) {
				
				// 剛体をジョイント
				var rigid = hitObject.GetComponent<Rigidbody> ();
				if( rigid ){
					joint.connectedBody = rigid;
					rigid.angularVelocity = Vector3.zero;
				}

			} else {
				
				//掴んでいるオブジェクトにフォースを加えて投げる
				Rigidbody rigid = joint.connectedBody;
				if (rigid) {
					rigid.velocity = hand.velocity;
					rigid.angularVelocity = -1.0f * hand.angularVelocity;
					rigid.maxAngularVelocity = rigid.angularVelocity.magnitude;
				}

				// ジョイントを解放する
				joint.connectedBody = null;
			}

			var target = hitObject.GetComponent<IPointControllerTarget> ();
			if (target != null) {
				target.OnPointControllerClick( isOn );
			}
		}
	}
}
