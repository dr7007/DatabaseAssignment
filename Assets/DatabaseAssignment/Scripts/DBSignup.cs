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
    [SerializeField] private Button signupIDBtn;                        //가입 ID 확인 버튼
    [SerializeField] private Button signupBtn;                          //가입 팝업 버튼
    [SerializeField] private Button signupErrorBtn;                     //가입 버튼

    //확인용 변수
    private string signCorrectPW;     //비밀번호 확인 때 사용

    private UserInfo userInfo = new UserInfo();
    public void OnValueChangedSignID(string signIDtmp)
    {
        userInfo.signID = signIDtmp;
        UpdateSignupIDButton();  // 값이 바뀔 때마다 버튼 상태 업데이트
    }
    private void UpdateSignupIDButton()
    {
        // signIDtmp 값이 비어 있으면 버튼을 비활성화하고, 값이 있으면 활성화
        if (string.IsNullOrEmpty(userInfo.signID))
        {
            signupIDBtn.interactable = false;
        }
        else
        {
            signupIDBtn.interactable = true;
        }
    }

    public void OnValueChangedPW(string _signpw)
    {
        userInfo.signPW = _signpw;
    }

    public void OnValueChangedSignCollectPW(string _signcollectpw)
    {
        signCorrectPW = _signcollectpw;
    }

    private void Update()
    {
        if (userInfo.signPW != signCorrectPW)
        {
            signErrorMessages[1].color = Color.red;
            SignErrorMessages(SignupError.SignIncorrectPW);
            signErrorMessages[1].gameObject.SetActive(true);
        }
        else
        {
            signErrorMessages[1].color = Color.blue;
            signErrorMessages[1].text = "비밀번호가 일치합니다.";
            signErrorMessages[1].gameObject.SetActive(true);
        }

        if (!string.IsNullOrEmpty(userInfo.signID))
        {
            signupIDBtn.interactable = true;
        }
        else
        {
            signupIDBtn.interactable = false;
        }
    }

    private enum SignupError    //가입 시 오류 발생
    {
        DuplicateID,        //중복 ID
        SignIncorrectPW,    //비밀번호가 다를 때 (비밀번호 확인)
        None                //입력창에 빈 곳이 있을 경우
    }

    private void SignErrorMessages(SignupError error)   //가입 중 문제 발생 시
    {
        switch(error)
        {
            case SignupError.DuplicateID:
                signErrorMessages[0].text = "사용 중인 ID입니다.";
                signErrorMessages[2].text = "사용 중인 ID입니다.";
                signErrorMessages[0].color = Color.red;
                signErrorMessages[0].gameObject.SetActive(true);
                signupErrorBtn.gameObject.SetActive(false);
                break;
            case SignupError.SignIncorrectPW:
                signErrorMessages[1].text = "비밀번호가 옳지 않습니다.";
                signErrorMessages[2].text = "비밀번호가 옳지 않습니다.";
                signErrorMessages[1].gameObject.SetActive(true);
                signupErrorBtn.gameObject.SetActive(false);
                break;
            case SignupError.None:
                signErrorMessages[2].text = "모두 채워넣어주세요.";
                signupErrorBtn.gameObject.SetActive(false);
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
    public void OnClickSignupIDButton()
    {
        StartCoroutine(DuplicateIDCoroutine(userInfo));
    }

    private IEnumerator DuplicateIDCoroutine(UserInfo _userInfo)
    {
        _userInfo.signID = signIDtmp.text;

        string uri = "http://127.0.0.1/DASignIDchk.php";

        WWWForm form = new WWWForm();
        form.AddField("id", _userInfo.signID);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError)  //Protocol 에러
            {
                signErrorMessages[2].text = www.error;
                yield break;
            }

            string signResponse = www.downloadHandler.text; //서버 응답 받기

            if (signResponse == "DuplicateID")   //중복 ID 일 시
            {
                SignErrorMessages(SignupError.DuplicateID);
            }

            else
            {
                signErrorMessages[0].color = Color.blue;
                signErrorMessages[0].text = "사용 가능한 ID입니다.";
                signErrorMessages[0].gameObject.SetActive(true);
            }
        }
    }

    public void OnClickSignupButton()
    {
        StartCoroutine(SignUpCoroutine(userInfo));
    }

    private IEnumerator SignUpCoroutine(UserInfo _userInfo)
    {
        _userInfo.signNickname = signNicknametmp.text;
        _userInfo.signID = signIDtmp.text;
        _userInfo.signPW = signPWtmp.text;
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

        if (!string.IsNullOrEmpty(_userInfo.signID) && !string.IsNullOrEmpty(_userInfo.signPW) && !string.IsNullOrEmpty(signCorrectPW) &&
            !string.IsNullOrEmpty(_userInfo.signNickname) && !string.IsNullOrEmpty(_userInfo.signPhoneNum) && !string.IsNullOrEmpty(_userInfo.signEmail))  //값이 비었을 때
        {
            using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ProtocolError)  //Protocol 에러
                {
                    signErrorMessages[2].text = www.error;
                    signupErrorBtn.gameObject.SetActive(false);
                    yield break;
                }

                string signResponse = www.downloadHandler.text; //서버 응답 받기

                if (signResponse == "DuplicateID")   //중복 ID일 시
                {
                    SignErrorMessages(SignupError.DuplicateID);
                }

                else
                {
                    signErrorMessages[2].text = "가입 완료";
                }
            }
        }
        else
        {
            SignErrorMessages(SignupError.None);
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