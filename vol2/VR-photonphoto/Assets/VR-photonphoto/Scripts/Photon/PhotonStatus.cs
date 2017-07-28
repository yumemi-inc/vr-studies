//
//  VR-Studies
//  Created by miuccie miurror on 6/04/2017.
//  Copyright 2017 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace VRAcademy {

	public class PhotonStatus : Photon.PunBehaviour {

		TextMesh monitor = null;

		//------------------------------------------------------------------------------------------------------------------------------//
		void Start(){
			monitor = this.GetComponent<TextMesh>();
			Application.logMessageReceived  += OnLogMessage;
		}

		void Update () {

			// Photon接続状況を画面に出力
			monitor.text = "* Photon *\n" + PhotonNetwork.connectionStateDetailed.ToString();
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		void OnJoinedRoom() {

			//ログイン後数秒後にロガーを消す
			DOVirtual.DelayedCall ( 3f, ()=> gameObject.SetActive(false) );
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		void OnLogMessage( string logText, string stackTrace, LogType type ){
			
			// ログを画面に出力
			//monitor.text = "\n" + logText;
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		public override void OnPhotonPlayerConnected( PhotonPlayer other  ) {

			monitor.text = "他のプレイヤーが接続 ID: " + other.ID;
			monitor.text += "\n現在のプレイヤー数 : " + PhotonNetwork.room.PlayerCount;
			Debug.Log ( monitor.text );
		}

		public override void OnPhotonPlayerDisconnected( PhotonPlayer other  ) {

			// 他プレイヤの切断時
			monitor.text = "他のプレイヤーが切断 ID: " + other.ID;
			monitor.text += "\n現在のプレイヤー数 : " + PhotonNetwork.room.PlayerCount;

			// 自分がマスターになったかどうか
			if ( PhotonNetwork.isMasterClient ) {
				monitor.text += "\n現在あなたがルームマスターです.";
			}

			Debug.Log ( monitor.text );
		}
	}
}
