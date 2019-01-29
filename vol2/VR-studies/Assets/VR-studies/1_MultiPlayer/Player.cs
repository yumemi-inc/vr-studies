using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace VRStudies { namespace MultiPlayer {

public class Player : MonoBehaviourPunCallbacks {

	// Joinイベントをリッスン
	//------------------------------------------------------------------------------------------------------------------------------//
	public override void OnJoinedRoom() {

		// ルームログイン後に呼ぶ
		CreateAvatar ();
	}
	
	//------------------------------------------------------------------------------------------------------------------------------//
	void CreateAvatar () {

		Debug.Log("Player: 自分のアバターを生成します.");

		// Photon経由で自分のアバターを動的に生成
		// 自分のアバターが他クライアント上にも自動で生成される = 他クライアントが生成したアバターは自クライアント上に自動で生成される
		GameObject avatar = PhotonNetwork.Instantiate("Avatar_Multi", new Vector3(0, 1, 0), Quaternion.identity, 0);
		avatar.transform.parent = transform;	

		// カメラを自身のアバターの子にする
		GameObject camera = transform.Find("Camera").gameObject;
		camera.transform.parent = avatar.transform;
	}
}
	
}}
