using UnityEngine;
using System.Collections;

namespace VRStudies { namespace SinglePlayer {

public class Player : MonoBehaviour {

	void Start () {

		// カメラをアバターの子にして追従させる
		GameObject avatar = transform.Find("Avatar").gameObject;
		GameObject camera = transform.Find("Camera").gameObject;
		camera.transform.parent = avatar.transform;
	}
}

}}
