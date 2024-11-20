using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using TMPro;

using UnityEngine.Networking;
using UnityEngine.UI;

public class DBSignup : MonoBehaviour
{
    public class UserInfo
    {
        public string signNickname { get; set; }
        public string signID { get; set; }
        public string signPW { get; set; }
        public string signPhoneNum { get; set; }
        public string signEmail { get; set; }
    }

    [SerializeField] private TMP_InputField signNicknametmp;            //가입 닉네임
    [SerializeField] private TMP_InputField signIDtmp;                  //가입 ID
    [SerializeField] private TMP_InputField signPWtmp;                  //가입 비밀번호
    [SerializeField] private TMP_InputField signCorrectPWtmp;           //가입 비밀번호 확인
    [SerializeField] private TMP_InputField signPhoneNumtmp;            //가입 전화번호
    [SerializeField] private TMP_InputField signEmailtmp;               //가입 이메일
    [SerializeField] private List<TextMeshProUGUI> signErrorMessages = null;   //가입 실패 텍스트
    [SerializeField] private Image signErrorpopup;                      //가입 실패 팝업
    [SerializeField] private Image signuppopup;                         //가입 팝업
    [SerializeField] private Button signuppopupBtn;                     //가입 팝업 버튼
    [SerializeField] private Button signupBtn;                          //가입 버튼

    //확인용 변수
    private string signCorrectPW;     //비밀번호 확인 때 사용

    private UserInfo userInfo = new UserInfo();

    private enum SignupError    //가입 시 오류 발생
    {
        DuplicateID,        //중복 ID
        SignIncorrectPW,    //비밀번호가 다를 때 (비밀번호 확인)
        None                //입력창에 빈 곳이 있을 경우
    }

    private void SignErrorMessages(SignupError error)
    {
        switch(error)
        {
            case SignupError.DuplicateID:
                signErrorMessages[0].text = "사용 중인 ID입니다.";
                signErrorMessages[2].text = "사용 중인 ID입니다.";
                signErrorMessages[0].gameObject.SetActive(true);
                break;
            case SignupError.SignIncorrectPW:
                signErrorMessages[1].text = "비밀번호가 옳지 않습니다.";
                signErrorMessages[2].text = "비밀번호가 옳지 않습니다.";
                signErrorMessages[1].gameObject.SetActive(true);
                break;
            case SignupError.None:
                signErrorMessages[2].text = "모두 채워넣어주세요.";
                break;
        }

        //가입 실패 시 2초 후 팝업 비활성화
        StartCoroutine(FailSignupPopupDelay());
    }

    private IEnumerator FailSignupPopupDelay()
    {
        yield return new WaitForSeconds(2f);

        signErrorpopup.gameObject.SetActive(false);
    }

    public void OnClickSignupButton()
    {
        StartCoroutine(SignUpCoroutine(userInfo));
    }

    private IEnumerator SignUpCoroutine(UserInfo _userInfo)
    {
        _userInfo.signNickname = signNicknametmp.text;
        _userInfo.signID = signIDtmp.text;
        _userInfo.signPW= signPWtmp.text;
        _userInfo.signPhoneNum = signPhoneNumtmp.text; 
        _userInfo.signEmail = signEmailtmp.text;

        signCorrectPW = signCorrectPWtmp.text;

        string uri = "http://127.0.0.1/DASignup.php";

        WWWForm form = new WWWForm();
        form.AddField("Ni ck", _userInfo.signNickname);
        form.AddField("id", _userInfo.signID);
        form.AddField("password", _userInfo.signPW);
        form.AddField("phonenum", _userInfo.signPhoneNum);
        form.AddField("Email", _userInfo.signEmail);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.ProtocolError)  //Protocol 에러
            {
                signErrorMessages[2].text = www.error;
                yield break;
            }

            string signResponse = www.downloadHandler.text; //서버 응답 받기

            if(signResponse == "DuplicateID")   //중복 ID일 시
            {
                SignErrorMessages(SignupError.DuplicateID);
            }
            else if(signResponse == "SignSuccess")
            {
                signErrorMessages[2].text = "가입 완료";
            }
            else
            {
                SignErrorMessages(SignupError.None);
            }
        }
    }

    //회원 정보 로그
    //private IEnumerator GetInfoCoroutine()
    //{
    //    string uri = "http://127.0.0.1/DAGetInfo.php";

    //    using (UnityWebRequest www = UnityWebRequest.PostWwwForm(uri, string.Empty))
    //    {
    //        yield return www.SendWebRequest();

    //        if(www.result == UnityWebRequest.Result.ProtocolError)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            Debug.Log(www.downloadHandler.text);
    //            string data = www.downloadHandler.text;

    //            List<UserInfo> userinfos = JsonUtility.FromJson<List<UserInfo>>(data);

    //            foreach(UserInfo userInfo in userinfos)
    //            {
    //                Debug.Log("ID: " + userInfo.signID + ", PW: " + userInfo.signPW + 
    //                    ", PhoneNumber: " + userInfo.signPhoneNum + ", E-Mail: " + userInfo.signEmail);
    //            }
    //        }
    //    }
    //}
}