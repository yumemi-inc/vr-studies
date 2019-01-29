//
//  VR-Studies
//  Created by miuccie miurror on 6/04/2017.
//  Copyright 2017 Yumemi.Inc / miuccie miurror
//

using UnityEngine;
using System.Collections;
using DG.Tweening;

using Photon.Pun;
using Photon.Realtime;

namespace VRStudies {

	public class PhotonStatus : MonoBehaviourPunCallbacks {

		TextMesh monitor = null;

		//------------------------------------------------------------------------------------------------------------------------------//
		void Start(){
			monitor = this.GetComponent<TextMesh>();
		}

		void Update () {

			// Photon接続状況を画面に出力
			if ( PhotonNetwork.InRoom == false ) {
				monitor.text = "* Photon *\n" + PhotonNetwork.NetworkClientState.ToString();
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		public override void OnJoinedRoom() {

			//ログイン後数秒後にロガーを消す
			DOVirtual.DelayedCall ( 3f, ()=> monitor.text = null );
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		public override void OnPlayerEnteredRoom( Photon.Realtime.Player other  ) {

			monitor.text = "他のプレイヤーが接続 No: " + other.ActorNumber;
			monitor.text += "\n現在のプレイヤー数 : " + PhotonNetwork.CurrentRoom.PlayerCount;

			// 数秒後にロガーを消す
			Debug.Log ( monitor.text );
			DOVirtual.DelayedCall ( 3f, ()=> monitor.text = null );
		}

		public override void OnPlayerLeftRoom( Photon.Realtime.Player other  ) {

			// 他プレイヤの切断時
			monitor.text = "他のプレイヤーが切断 No: " + other.ActorNumber;
			monitor.text += "\n現在のプレイヤー数 : " + PhotonNetwork.CurrentRoom.PlayerCount;

			// 自分がマスターになったかどうか
			if ( PhotonNetwork.IsMasterClient ) {
				monitor.text += "\n現在あなたがルームマスターです.";
			}

			// 数秒後にロガーを消す
			Debug.Log ( monitor.text );
			DOVirtual.DelayedCall ( 3f, ()=> monitor.text = null );
		}
	}
}
