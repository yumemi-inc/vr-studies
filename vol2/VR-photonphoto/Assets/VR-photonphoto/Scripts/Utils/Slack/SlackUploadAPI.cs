using System;
using System.Collections;
using UnityEngine;

// Slack-API-Tokenが必要です
// https://api.slack.com/custom-integrations/legacy-tokens

public static class SlackAPI {

	public class UploadData {
		public string token             = "";
		public string filename          = "";
		public byte[] filedata          = null;
		public string title             = "";
		public string initial_comment   = "";
		public string channels          = "";
	}

	public static IEnumerator Upload ( UploadData data, Action onSuccess = null, Action<string> onError = null ){

		yield return new WaitForEndOfFrame();

		var form        = new WWWForm();
		form.AddField("token"              , data.token);
		form.AddField("title"              , data.title);
		form.AddField("initial_comment"    , data.initial_comment);
		form.AddField("channels"           , data.channels );
		form.AddBinaryData("file", data.filedata, data.filename, "image/png");

		var url = "https://slack.com/api/files.upload";
		var www = new WWW( url, form );
		yield return www;
		var error = www.error;
		if ( !string.IsNullOrEmpty( error ) ){
			if ( onError != null ){
				onError( error );
			}
			yield break;
		}
		if ( onSuccess != null ){
			onSuccess();
		}
	}
}
