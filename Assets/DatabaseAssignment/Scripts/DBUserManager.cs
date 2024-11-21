using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using TMPro;

using UnityEngine.Networking;
using UnityEngine.UI;

public class DBUserManager : MonoBehaviour
{
    public class UserInfo
    {
        public string signNickname { get; set; }
        public string signID { get; set; }
        public string signPW { get; set; }
        public string signPhoneNum { get; set; }
        public string signEmail { get; set; }
    }

    [SerializeField]
    private DBPlayerInfo playerinfo;

    private UserInfo userinfo = new UserInfo();


    private IEnumerator TakeUserInfoCoroutine(UserInfo _userInfo)
    {
        string uri = "http://127.0.0.1/DAUserState.php";

        WWWForm form = new WWWForm();
        form.AddField("id", _userInfo.signID);
        form.AddField("pw", _userInfo.signPW);
        form.AddField("nick", _userInfo.signNickname);
        form.AddField("pNum", _userInfo.signPhoneNum);
        form.AddField("eM", _userInfo.signEmail);


        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError)  //Protocol 에러
            {
                
                yield break;
            }

            string signResponse = www.downloadHandler.text; //서버 응답 받기

            if (signResponse == "")   //중복 ID일 시
            {
                
            }
            else
            {

            }
        }
    }

    private void Start()
    {
        userinfo.signID = playerinfo.uid;
    }

}
