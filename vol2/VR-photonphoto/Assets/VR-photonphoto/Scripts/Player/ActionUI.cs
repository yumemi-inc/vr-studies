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

namespace VRStudies {

	//------------------------------------------------------------------------------------------------------------------------------//
	public class ActionUI : MonoBehaviour, IPointerClickHandler {

		public void OnPointerClick( PointerEventData eventData ){

			// 該当のアクションを起動する
			var actionId = gameObject.name.Replace("Action ", "");
			PlayerManager.Instance.actionUI.ChangeAction( actionId );
		}
	}
}
