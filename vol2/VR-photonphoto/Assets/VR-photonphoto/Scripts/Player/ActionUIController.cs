//
//  VR-Studies
//  Created by miuccie miurror on 6/04/2017.
//  Copyright 2017 Yumemi.Inc / miuccie miurror
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStudies {

	public class ActionUIController : MonoBehaviour {

		//------------------------------------------------------------------------------------------------------------------------------//
		private static ActionUIController instance = null;
		public static ActionUIController Instance {
			get {
				if (instance != null) return instance;
				instance = FindObjectOfType(typeof(ActionUIController)) as ActionUIController;
				return instance;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		Dictionary<string, List<string>> animationMap = new Dictionary<string, List<string>> () {
			{"1", new List<string>{"Wave", "Bow", "Clap"}},
			{"2", new List<string>{"ThumbsUp", "Raise", "Jump"}},
			{"3", new List<string>{"Surprised", "Excite", "Crazy"}},
			{"4", new List<string>{"Angry", "Charge",  "No"}},
			{"5", new List<string>{"Dance", "Robot", "Samba", "Capoeira"}}
		};

		GameObject avatar;
		Animator   animator;

		//------------------------------------------------------------------------------------------------------------------------------//
		public void Setup ( GameObject _avatar ) {

			// 自身をアバターの子にする == 自動で追随
			avatar = _avatar;
			transform.parent = avatar.transform;
			animator = avatar.GetComponent<Animator> ();
		}
		
		//------------------------------------------------------------------------------------------------------------------------------//
		void Update () {

			if( avatar == null ){ return; }

			// キーボード入力の受付 //歩いている時以外
			AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo (0);
			if ( currentState.nameHash != Animator.StringToHash ("Base Layer.Walk") ) {
				foreach (var key in animationMap.Keys) {
					if (Input.GetKeyDown (key)) {

						ChangeAction (key);
					}
				}
			}
		}

		public void ChangeAction( string key, GameObject _avatar = null ){

			if( _avatar == null ){ _avatar = avatar; }

			// キーに合わせてランダムなアニメを再生
			var animeList = animationMap[ key ];
			var animeId = animeList [Random.Range (0, animeList.Count)];
			_avatar.GetComponent<Avatar>().ChangeAction( animeId );
		}

	}
}
