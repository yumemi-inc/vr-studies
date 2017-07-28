//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class GrabController : MonoBehaviour {

	SteamVR_Controller.Device hand;

	FixedJoint joint;
	GameObject hitObject;

	//------------------------------------------------------------------------------------------------------------------------------//
	public interface IGrabControllerTarget {
		Rigidbody GetTargetRigidbody();
		void OnGrabControllerGrab( bool isOn );
	}
	void DispatchGrabEvent (bool isOn) {
		if ( hitObject ) {
			var grabTarget = hitObject.GetComponent<IGrabControllerTarget>();
			grabTarget.OnGrabControllerGrab (isOn);
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void Start () {

		// コントローラーデバイスを保持
		hand = SteamVR_Controller.Input( (int)this.GetComponent<SteamVR_TrackedObject>().index );

		// SteamVR_TrackedControllerをアタッチして、トリガーイベントハンドラを登録
		SteamVR_TrackedController handTO = gameObject.AddComponent<SteamVR_TrackedController> ();
		handTO.TriggerClicked += new ClickedEventHandler (OnTriggerClicked);
		handTO.TriggerUnclicked += new ClickedEventHandler (OnTriggerUnclicked);

		// コリジョン判定用のColliderとRigidBodyをアタッチ
		var collider = gameObject.AddComponent<SphereCollider>();
		collider.isTrigger = true; //衝突イベントを受け取るために有効化
		collider.radius = 0.06f;

		var rigid = gameObject.AddComponent<Rigidbody>();
		rigid.useGravity = false;
		rigid.isKinematic = true; //スクリプトから操作するために有効化
		rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

		// ジョイントをアタッチ
		joint = gameObject.AddComponent<FixedJoint> ();
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	// コリジョンイベントの検知
	void OnTriggerEnter( Collider other ) {

		// 掴めるオブジェクトが衝突した場合のみ検知する
		if ( other.gameObject.GetComponent<IGrabControllerTarget>() != null ) {
			hitObject = other.gameObject;
			hand.TriggerHapticPulse ( 3000 );
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	// トリガーボタンイベントの検知
	public void OnTriggerClicked( object sender, ClickedEventArgs e ) {

		if ( hitObject ) { 

			// 剛体をジョイントにアサイン
			var grabTarget = hitObject.GetComponent<IGrabControllerTarget>();
			joint.connectedBody = grabTarget.GetTargetRigidbody();
	
			// イベント発行
			DispatchGrabEvent ( true );
		}
	}

	public void OnTriggerUnclicked( object sender, ClickedEventArgs e ) {
		
		if ( hitObject ) { 

			// イベント発行
			DispatchGrabEvent ( false );

			//掴んでいるオブジェクトにフォースを加えて投げる
			Rigidbody rigid = joint.connectedBody;
			rigid.velocity = hand.velocity;
			rigid.angularVelocity = hand.angularVelocity;
			rigid.maxAngularVelocity = rigid.angularVelocity.magnitude;

			// ジョイントを解放する
			joint.connectedBody = null;
			hitObject = null;
		}
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void FixedUpdate() {

		// コントローラーとの距離が一定以上になったらJointを解放する
		if ( hitObject != null ) {
			float distance = Vector3.Distance ( this.transform.position, hitObject.transform.position );
			if ( distance > 0.25f ) {
				joint.connectedBody = null; 
				hitObject = null;
				hand.TriggerHapticPulse ( 3000 );
			}
		}
	}
}
