//
//  VR-Studies
//  Created by miuccie miurror on 6/04/2017.
//  Copyright 2017 Yumemi.Inc / miuccie miurror
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRStudies {

	public class SpotUIController : MonoBehaviour {

		List<GameObject> spots = new List<GameObject>();

		//------------------------------------------------------------------------------------------------------------------------------//
		private static SpotUIController instance = null;
		public static SpotUIController Instance {
			get {
				if (instance != null) return instance;
				instance = FindObjectOfType(typeof(SpotUIController)) as SpotUIController;
				return instance;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		void Start () {

			// アンカーを生成して並べる
			SetUpAnchors ( 2.2f, 4, 1, true );
			SetUpAnchors ( 2.2f, 6 );
			SetUpAnchors ( 3.0f, 8, 2f );
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		void SetUpAnchors ( float radius, int num, float offset = 0f, bool isFlip = false ) {

			//スポットアンカーを円形に並べる
			GameObject spot = transform.Find ("Spot").gameObject;
			for( int i = 0; i < num; i++  ){

				GameObject copy = Object.Instantiate (spot);
				copy.transform.parent = spot.transform.parent;

				float w = ( (i + 1) * (180f / (num+1)) + (isFlip ? 0f : 180f) ) * Mathf.Deg2Rad;
				float x = radius * Mathf.Cos(w);
				float z = radius * Mathf.Sin(w);
				copy.transform.position = new Vector3( x, 0, z - offset );
				spots.Add ( copy );
			}
		}

		//------------------------------------------------------------------------------------------------------------------------------//
		public GameObject GetAnchor ( int index ) {
			return spots [index];
		}
	}

}
