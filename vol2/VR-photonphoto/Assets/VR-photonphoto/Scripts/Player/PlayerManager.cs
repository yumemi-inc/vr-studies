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

	public class PlayerManager : MonoBehaviour {

		//------------------------------------------------------------------------------------------------------------------------------//
		private static PlayerManager instance = null;
		public static PlayerManager Instance {
			get {
				if (instance != null) return instance;
				instance = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
				return instance;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		List<string> modelList = new List<string> () {
			"unitychan", "audioboy", "bighead", "zombiman", "boxman" 
		};

		//------------------------------------------------------------------------------------------------------------------------------//
		public string 		       playerName = "プレイヤー名";
		public GameObject 		   avatar;
		public CameraUIController  cameraUI;
		public ActionUIController  actionUI;

		//------------------------------------------------------------------------------------------------------------------------------//
		public void Setup () {

			// Photon経由で自分のアバターを生成 //他人のアバターは自動で生成 & 共有される
			int index = Random.Range(0, modelList.Count);
			avatar = PhotonNetwork.Instantiate("Prefabs/Player/Avatar_" + modelList[index], Vector3.zero, Quaternion.identity, 0);
			avatar.transform.parent = transform;
			Debug.Log("PlayerManager: 自分のアバターを生成しました: Name:" + avatar.gameObject.name);

			// カメラUIとアクションUIにアバターをセットする
			cameraUI = transform.Find("Camera UI").gameObject.GetComponent<CameraUIController>();
			cameraUI.Setup( avatar );
			actionUI = transform.Find("Action UI").gameObject.GetComponent<ActionUIController>();
			actionUI.Setup( avatar );

			//アバターの名前にプレイヤー名をセットする
			avatar.gameObject.name = playerName + " - ID:" + PhotonNetwork.player.ID;
			avatar.GetComponent<Avatar>().ChangeName ( avatar.gameObject.name );

			//アバターをランダムな初期位置に移動
			avatar.transform.position = new Vector3 (Random.Range (-6, 6), 0, Random.Range (-6, 6));
			avatar.transform.LookAt (Vector3.zero);
		}
	}
}