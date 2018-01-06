//
//  VR-Studies
//  Created by miuccie miurror on 6/04/2017.
//  Copyright 2017 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace VRAcademy {

	public class PlayerManagerTest : MonoBehaviour {

		//------------------------------------------------------------------------------------------------------------------------------//
		private static PlayerManagerTest instance = null;
		public static PlayerManagerTest Instance {
			get {
				if (instance != null) return instance;
				instance = FindObjectOfType(typeof(PlayerManagerTest)) as PlayerManagerTest;
				return instance;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		List<string> modelList = new List<string> () {
			"unitychan", "audioboy", "bighead", "zombiman", "boxman" 
		};


		//------------------------------------------------------------------------------------------------------------------------------//
		public GameObject 		   avatar;
		public List<GameObject>    avatars = new List<GameObject>();
		public CameraUIController  cameraUI;
		public ActionUIController  actionUI;

		// Joinイベントをリッスン
		//------------------------------------------------------------------------------------------------------------------------------//
		void OnJoinedRoom() {

			// ルームログイン後に呼ぶ
			Setup ();
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		public void Setup () {

			int index = Random.Range(0, modelList.Count);
			avatar = PhotonNetwork.Instantiate("Prefabs/Player/Avatar_" + modelList[index], Vector3.zero, Quaternion.identity, 0);
			avatar.transform.parent = transform;
			Debug.Log("PlayerManager: アバターを生成しました: Name:" + avatar.gameObject.name);

			// カメラUIとアクションUIにアバターをセットする
			cameraUI = transform.Find("Camera UI").gameObject.GetComponent<CameraUIController>();
			cameraUI.Setup( avatar );
			actionUI = transform.Find("Action UI").gameObject.GetComponent<ActionUIController>();
			actionUI.Setup( avatar );

			//アバターの名前にプレイヤー名をセットする
			avatar.gameObject.name = "ME" + " - Id:" + PhotonNetwork.player.ID;
			avatar.GetComponent<Avatar>().ChangeName ( avatar.gameObject.name );
			avatar.transform.Find ("Name").gameObject.GetComponent<TextMesh>().color = Color.yellow;

			// テスト用に複数プレイヤーを生成
			for( int i = 0; i < 10; i++ ){
				CreatePlayer( i );
			}
		}

		void CreatePlayer ( int playerIndex ) {

			int index = playerIndex % modelList.Count;
			//int index = Random.Range(0, modelList.Count);
			avatar = PhotonNetwork.Instantiate("Prefabs/Player/Avatar_" + modelList[index], Vector3.zero, Quaternion.identity, 0);
			avatar.transform.parent = transform;
			Debug.Log("PlayerManager: アバターを生成しました: Name:" + avatar.gameObject.name);

			//アバターの名前にプレイヤー名をセットする
			avatar.gameObject.name = "Other" + " - Id:" + playerIndex;
			avatar.GetComponent<Avatar>().ChangeName ( avatar.gameObject.name );

			//アバターをランダムなスポットに移動
			GameObject spot = SpotUIController.Instance.GetAnchor( playerIndex );
			avatar.transform.position = spot.transform.position;

			// カメラの方を向かせる
			GameObject video = GameObject.Find ("Videocam Model");
			avatar.transform.LookAt (video.transform.position);

			avatars.Add ( avatar );
		}
	}
}