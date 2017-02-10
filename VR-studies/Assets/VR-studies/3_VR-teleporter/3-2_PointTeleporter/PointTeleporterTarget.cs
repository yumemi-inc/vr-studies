//
//  VR-Studies
//  Created by miuccie miurror on 11/04/2016.
//  Copyright 2016 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;

public class PointTeleporterTarget : MonoBehaviour, PointTeleporter.IPointTeleporterTarget {

	GameObject CameraRig = null;
	GameObject CameraEye = null;
	SteamVR_Controller.Device head = null;
	Color color;

	void Start() {

		color = gameObject.GetComponent<Renderer> ().material.color;

		// カメラにSteamVR_Fadeスクリプトをアタッチ
		CameraRig = GameObject.Find ("[CameraRig]");
		CameraEye = CameraRig.transform.FindChild( "Camera (eye)" ).gameObject;
		CameraEye.AddComponent<SteamVR_Fade> ();

		// HMDデバイスの参照を取得
		head = SteamVR_Controller.Input( (int) SteamVR_TrackedObject.EIndex.Hmd );
	}

	public void OnPointTeleporterHit( bool isOn ) {
		gameObject.GetComponent<Renderer> ().material.color = isOn ? new Color ( 1, 1, 0 ) : color;
	}

	public void OnPointTeleporterClick() {	
		StartTeleport ();
	}

	void StartTeleport() {
		
		// 画面をフェードアウトしてカメラの位置を変更した後にフェードイン
		SteamVR_Fade.Start( new Color( 0, 0, 0 ), 0.3f, false );
		Invoke ( "EndTeleport", 0.3f );
	}

	void EndTeleport() {

		// 自身の真下にレイを飛ばして足元を探る
		RaycastHit hitInfo;
		if ( Physics.Raycast ( transform.position, Vector3.down, out hitInfo, 100 ) ) {

			// ルームごと移動させるために、ターゲット座標とHMD座標との差分をCameraRigのポジションに適用
			CameraRig.transform.position = new Vector3( hitInfo.point.x - head.transform.pos.x,  hitInfo.point.y, hitInfo.point.z - head.transform.pos.z );
			SteamVR_Fade.Start( Color.clear, 1.0f, false );
		}
	}
}
