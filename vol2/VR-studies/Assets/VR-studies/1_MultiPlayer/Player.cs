using UnityEngine;
using System.Collections;

namespace VRStudies { namespace MultiPlayer {

public class Player : MonoBehaviour {

	// Joinイベントをリッスン
	//------------------------------------------------------------------------------------------------------------------------------//
	void OnJoinedRoom() {

		// ルームログイン後に呼ぶ
		CreateAvatar ();
	}
	
	//------------------------------------------------------------------------------------------------------------------------------//
	void CreateAvatar () {

		Debug.Log("Player: 自分のアバターを生成します.");

		// Photon経由で自分のアバターを動的に生成
		// 自分のアバターが他クライアント上にも自動で生成される = 他クライアントが生成したアバターは自クライアント上に自動で生成される
		GameObject avatar = PhotonNetwork.Instantiate("Avatar", new Vector3(0, 1, 0), Quaternion.identity, 0);
		avatar.transform.parent = transform;	

		// カメラを自身のアバターの子にする
		GameObject camera = transform.FindChild("Camera").gameObject;
		camera.transform.parent = avatar.transform;
	}
}
	
}}
