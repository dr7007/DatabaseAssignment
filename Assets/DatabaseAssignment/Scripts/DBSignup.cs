using System.Collections;
using UnityEngine;

using TMPro;

using UnityEngine.Networking;
using UnityEngine.UI;

public class DBSignup : MonoBehaviour
{
    [SerializeField] private TMP_InputField signNicknametmp;        //가입 닉네임
    [SerializeField] private TMP_InputField signIDtmp;              //가입 ID
    [SerializeField] private TMP_InputField signPasswordtmp;        //가입 비밀번호
    [SerializeField] private TMP_InputField signCorrectpasswordtmp; //가입 비밀번호 확인
    [SerializeField] private TMP_InputField signPhoneNumtmp;        //가입 전화번호
    [SerializeField] private TMP_InputField signEmailtmp;           //가입 이메일
    [SerializeField] private TextMeshProUGUI signerrorMessage;      //가입 실패 텍스트
    [SerializeField] private Button signupBtn;                      //가입 버튼

    //확인용 변수
    private string signNickname;            //중복 닉네임
    private string signID;                  //중복 아이디
    private string signPassword;            //비밀번호 확인 때 사용
    private string signCorrectPassword;     //비밀번호 확인 때 사용
    private string signPhoneNum;            //중복 전화번호
    private string signEmail;               //중복 이메일
    
    private void OnClickSignupButton()
    {
        StartCoroutine(SignUpCoroutine(
            signNickname, signID, signPassword, signCorrectPassword, signPhoneNum, signEmail));
    }

    private IEnumerator SignUpCoroutine(
        string signNickname, string signID, string signPassword, string signCorrectPassword, string signPhoneNum, string signEmail)
    {
        signNickname = signNicknametmp.text;
        signID = signIDtmp.text;
        signPassword = signPasswordtmp.text;
        signCorrectPassword = signCorrectpasswordtmp.text;
        signPhoneNum = signPhoneNumtmp.text;
        signEmail = signEmailtmp.text;

        string uri = "http://127.0.0.1/DASignup.php";

        WWWForm form = new WWWForm();
        form.AddField("Nick", signNickname);
        form.AddField("id", signID);
        form.AddField("password", signPassword);
        form.AddField("phonenum", signPhoneNum);
        form.AddField("Email", signEmail);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {

            }
        }
    }
}