//
//  VR-Studies
//  Created by miuccie miurror on 6/04/2017.
//  Copyright 2017 Yumemi.Inc / miuccie miurror
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace VRAcademy {

	//------------------------------------------------------------------------------------------------------------------------------//
	public class SpotUI : MonoBehaviour, IPointerClickHandler {

		public void OnPointerClick( PointerEventData eventData ){

			// プレイヤーを該当箇所まで移動する
			Vector3 pos = transform.position; pos.y = 0;
			PlayerManager.Instance.avatar.GetComponent<Avatar>().MoveToPosition( pos );
		}
	}
}
