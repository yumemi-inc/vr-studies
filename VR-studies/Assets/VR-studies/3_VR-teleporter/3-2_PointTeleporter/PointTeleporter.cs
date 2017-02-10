//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PointTeleporter : LaserController {

	SteamVR_Controller.Device hand = null;

	RaycastHit hitInfo;
	GameObject hitObject;

	//------------------------------------------------------------------------------------------------------------------------------//
	void Start() {

		SteamVR_TrackedObject handTO = transform.GetComponent<SteamVR_TrackedObject>();
		hand = SteamVR_Controller.Input( (int) handTO.index );

		//レーザーポインタを作成する
		base.CreateLaserPointer();
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void FixedUpdate () {

		// 物理オブジェクトのヒットテスト
		bool hasHit = Physics.Raycast ( transform.position, transform.forward, out hitInfo, 100 );
		if ( hasHit ) {

			//レーザーの長さを調節
			AdjustLaserDistance( hitInfo.distance );

			//ターゲットが変更されたか
			if ( hitObject != hitInfo.collider.gameObject ) {

				// 以前のターゲットを無効に
				if ( hitObject ) {
					DispatchHitEvent(false);
				}

				//ヒットイベント発行
				hitObject = hitInfo.collider.gameObject;
				DispatchHitEvent(true);

			} else {

				//トリガーを引いたらクリックイベントを発行
				if ( hand.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) ) {
					DispatchClickEvent();
				}
			}

		} else {
			
			DispatchHitEvent(false);
			hitObject = null;
		}
	} 

	//------------------------------------------------------------------------------------------------------------------------------//
	public interface IPointTeleporterTarget {
		void OnPointTeleporterHit(bool isOn);
		void OnPointTeleporterClick();
	}

	void DispatchHitEvent (bool isOn) {
		if (hitObject) {
			var target = hitObject.GetComponent<IPointTeleporterTarget> ();
			if (target != null) {
				target.OnPointTeleporterHit( isOn );
			}
		}
	}

	void DispatchClickEvent () {
		if (hitObject) {
			var target = hitObject.GetComponent<IPointTeleporterTarget> ();
			if (target != null) {
				target.OnPointTeleporterClick();
			}
		}
	}
}
