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
    [SerializeField] private Image signuppopup;                         //가입 팝업
    [SerializeField] private Button signuppopupBtn;                     //가입 팝업 버튼
    [SerializeField] private Button signupBtn;                          //가입 버튼
    [SerializeField] private Button signCancelBtn;                      //취소 버튼

    //확인용 변수
    private string signCorrectPW;     //비밀번호 확인 때 사용

    private UserInfo userInfo = new UserInfo();

    private enum SignupError    //가입 시 오류 발생
    {
        None,               //입력창 하나라도 비어있으면
        DuplicateNickname,  //중복 닉네임
        DuplicateID,        //중복 ID
        SignIncorrectPW     //비밀번호가 다를 때 (비밀번호 확인)
    }

    private void SignErrorMessages(SignupError error)
    {
        switch(error)
        {
            case SignupError.DuplicateNickname:
                signErrorMessages[0].text = "이미 사용 중인 이름입니다.";
                break;
            case SignupError.DuplicateID:
                signErrorMessages[1].text = "이미 사용 중인 ID입니다.";
                break;
            case SignupError.SignIncorrectPW:
                signErrorMessages[2].text = "비밀번호가 옳지 않습니다.";
                break;
            case SignupError.None:
                signErrorMessages[3].text = "모두 채워넣어주세요.";
                break;
        }
    }

    public void OnClickSignupCanvasButton()
    {
        signuppopup.gameObject.SetActive(true);
    }

    public void OnClickSignupButton()
    {
        StartCoroutine(SignUpCoroutine(userInfo));
    }

    public void OnClickSignCancelButton()
    {
        signuppopup.gameObject.SetActive(false);
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
        form.AddField("Nick", _userInfo.signNickname);
        form.AddField("id", _userInfo.signID);
        form.AddField("password", _userInfo.signPW);
        form.AddField("phonenum", _userInfo.signPhoneNum);
        form.AddField("Email", _userInfo.signEmail);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.ProtocolError)  //Protocol 에러
            {
                signErrorMessages[3].text = www.error;
            }   
            else //로그인 성공
            {
                signErrorMessages[3].text = "가입되셨습니다.";
                signuppopup.gameObject.SetActive(false);
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