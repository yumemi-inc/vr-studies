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

public class ParaboraTeleporter : MonoBehaviour {

	SteamVR_Controller.Device head = null;
	SteamVR_Controller.Device hand = null;

	GameObject CameraRig = null;
	GameObject CameraEye = null;

	GameObject cursor;

	List<GameObject> paraboras = new List<GameObject>();
	public float paraboraMaxZ = 10.0f;
	public float paraboraMaxY = 10.0f;

	RaycastHit hitInfo;
	bool isTeleporting = false;

	//------------------------------------------------------------------------------------------------------------------------------//
	void Start() {

		// カメラにSteamVR_Fadeスクリプトをアタッチ
		CameraRig = GameObject.Find ("[CameraRig]");
		CameraEye = CameraRig.transform.FindChild( "Camera (eye)" ).gameObject;
		CameraEye.AddComponent<SteamVR_Fade> ();

		SteamVR_TrackedObject trackedObject = transform.GetComponent<SteamVR_TrackedObject>();
		hand = SteamVR_Controller.Input( (int) trackedObject.index );
		head = SteamVR_Controller.Input( (int) SteamVR_TrackedObject.EIndex.Hmd );

		// パラボラの各点を作成する
		CreateParaboraPoints();
	}

	void CreateParaboraPoints() {

		//ポイントを作成する
		for( int i = 0; i < 10; i++ ){
			var point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			point.transform.SetParent( transform, false );
			point.transform.localScale = new Vector3( 0.04f, 0.04f, 0.04f );
			point.GetComponent<MeshRenderer>().material.color = Color.yellow;
			Object.DestroyImmediate(point.GetComponent<SphereCollider>());
			paraboras.Add ( point );
		}

		//カーソルを作成する
		cursor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		cursor.transform.SetParent( transform, false );
		cursor.transform.localScale = new Vector3( 0.15f, 0.15f, 0.15f );
		cursor.GetComponent<MeshRenderer>().material.color = Color.red;
		Object.DestroyImmediate(cursor.GetComponent<SphereCollider>());
	}


	Vector3 GetParabolaPoint( Vector3 start, Vector3 end, float maxHeight, float t ) {
		
		// 開始と終点の3次元位置から中間の放物線の各点を計算する
		float tic = t * 2 - 1;
		Vector3 toDir = end - start;
		Vector3 ticDir = end - new Vector3( start.x, end.y, start.z );
		Vector3 right = Vector3.Cross( toDir, ticDir );
		Vector3 up = Vector3.Cross( right, ticDir );
		if ( end.y > start.y ) up = -up;
		Vector3 result = start + t * toDir;
		result += ( ( -tic * tic + 1 ) * maxHeight ) * up.normalized;
		return result;
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	bool CastParaboraPointer() {

		if( isTeleporting ){ return false; }
		bool hasHit = false;

		// コントローラーのX軸のアングルを -90 ～ 90度に変換
		var angleX = hand.transform.rot.eulerAngles.x;
		if( angleX <= 90 ){ angleX *= -1; }
		if( angleX >= 270 ){ angleX = 360 - angleX; }

		// 0度より上向きのとき
		if ( angleX > 0 ) {

			// 0度の時に最長になるようにカーソルの奥行を決定
			var endZ = ( 1.0f - angleX / 90.0f ) * paraboraMaxZ;
			cursor.transform.localPosition = new Vector3( 0.0f, 0.0f, endZ );
		}

		// コントローラー前方にレイキャストして衝突位置にカーソルを移動
		if ( Physics.Raycast ( transform.position, transform.forward, out hitInfo, paraboraMaxZ ) ) {
			cursor.transform.position = new Vector3( hitInfo.point.x, hitInfo.point.y + 0.1f, hitInfo.point.z );
		}else{
			cursor.transform.localPosition = new Vector3( 0.0f, 0.0f, paraboraMaxZ );
		}

		// カーソル位置上空から真下にレイキャストして衝突位置までカーソルのY位置を移動
		if ( Physics.Raycast ( new Vector3( cursor.transform.position.x, paraboraMaxY, cursor.transform.position.z ), Vector3.down, out hitInfo, paraboraMaxY ) ) {
			cursor.transform.position = new Vector3( hitInfo.point.x, hitInfo.point.y + 0.1f, hitInfo.point.z );
			hasHit = true;
		}

		// カーソル位置まで各点の放物線軌跡を描く
		for( int i = 0; i < paraboras.Count; i++ ){
			var point = paraboras [i];
			point.transform.position = GetParabolaPoint( transform.position, cursor.transform.position, 2.0f, i * 0.1f );
			point.GetComponent<MeshRenderer> ().material.color = hasHit ? Color.yellow : Color.red;
		}

		return hasHit;
	}

	//------------------------------------------------------------------------------------------------------------------------------//
	void FixedUpdate () {

		// パラボラポインタでヒットを検知する
		if ( CastParaboraPointer() ) {

			//トリガーを引いたらワープする
			if ( hand.GetPressDown( SteamVR_Controller.ButtonMask.Trigger ) ) {
				StartTeleport();
			}
		}
	} 

	void StartTeleport() {

		// 画面をフェードアウトしてカメラの位置を変更した後にフェードイン
		isTeleporting = true;
		SteamVR_Fade.Start( new Color( 0, 0, 0 ), 0.3f, false );
		Invoke ( "EndTeleport", 0.3f );
	}

	void EndTeleport() {

		// ルームごと移動させるために、ターゲット座標とHMD座標との差分をCameraRigのポジションに適用
		CameraRig.transform.position = new Vector3 ( cursor.transform.position.x - head.transform.pos.x, hitInfo.point.y, cursor.transform.position.z - head.transform.pos.z );
		SteamVR_Fade.Start( Color.clear, 1.0f, false );
		isTeleporting = false;
	}
}
