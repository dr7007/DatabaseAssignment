using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine;

using TMPro;

using UnityEngine.Networking;
using UnityEngine.UI;

public class DBSignin : MonoBehaviour
{
    [SerializeField] private TMP_InputField idtmp;   //아이디 입력 UI InputField
    [SerializeField] private TMP_InputField passwordtmp;   //비밀번호 입력 UI InputField
    [SerializeField] private TextMeshProUGUI errortmp;  //로그인 실패 시 이유 출력할 텍스트
    [SerializeField] private Image loginpopup;  //로그인 성공, 실패에 대한 팝업

    private string id;  //idtmp에 입력한 값을 사용하기 위해
    private string password;    //passwordtmp에 입력한 값을 사용하기 위해

    private const string loginUri = "http://127.0.0.1/DASignin.php";   //값 가져올 php파일

    private enum LoginError //로그인 오류 발생 시
    {
        None,   //idtmp, passwordtmp 둘다 or 둘 중 하나에 값이 없을 경우
        IDNotFound, //idtmp 값과 DB의 ID값이 동일하지 않는 경우
        IncorrectPassword,  //passwordtmp 값과 DB의 Password값이 동일하지 않는 경우
        ConnectionError //서버 연결 자체의 문제일 경우
    }

    private void ErrorMessage(LoginError error) //각 상황에 따른 오류 메세지
    {
        switch (error)
        {
            case LoginError.None:
                errortmp.text = "ID 또는 비밀번호를 입력하지 않았습니다.";
                break;
            case LoginError.IDNotFound:
                errortmp.text = "아이디가 존재하지 않습니다.";
                break;
            case LoginError.IncorrectPassword:
                errortmp.text = "비밀번호가 옳지 않습니다.";
                break;
            case LoginError.ConnectionError:
                errortmp.text = "서버에 연결되어있지 않습니다.";
                break;
            default:
                errortmp.text = "";
                break;
        }

        //로그인 실패 시 1초 후 팝업 비활성화
        StartCoroutine(FailLoginPopupDelay());
    }

    private IEnumerator FailLoginPopupDelay()
    {
        yield return new WaitForSeconds(2f);

        loginpopup.gameObject.SetActive(false);
    }

    public void OnLoginButtonClicked()  //Login버튼 클릭 시
    {
        StartCoroutine(LoginCoroutine(id, password));
        errortmp.text = "로그인 중...";
        loginpopup.gameObject.SetActive(true);
    }

    private IEnumerator LoginCoroutine(string id, string password)  //로그인 코루틴
    {
        id = idtmp.text;    //id의 값을 초기화
        password = passwordtmp.text;    //password값을 초기화

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))  //값이 비었을 때
        {
            ErrorMessage(LoginError.None);  //None 오류메세지 표시
            yield break;
        }

        WWWForm loginform = new WWWForm();   //WWWForm 클래스: Unity에서 HTTP 요청 보낼 때 폼 데이터 전송 기능
        loginform.AddField("loginID", id);   //ID 탭에 idtmp값을 추가
        loginform.AddField("loginPass", password);   //PASSWORD 탭에 passwordtmp값을 추가

        using (UnityWebRequest www = UnityWebRequest.Post(loginUri, loginform))
        {
            yield return www.SendWebRequest();  //요청 보내기

            //서버 오류 발생 시
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.DataProcessingError)
            {
                ErrorMessage(LoginError.ConnectionError);
                yield break;
            }

            //서버 응답 받을 시
            string response = www.downloadHandler.text.Trim();  //서버 응답 받기

            if (response == "IDNotFound")    //ID가 존재하지 않을 시
            {
                ErrorMessage(LoginError.IDNotFound);
            }
            else if (response == "IncorrectPassword")   //비밀번호가 존재하지 않을 시
            {
                ErrorMessage(LoginError.IncorrectPassword);
            }
            else if (response == "Success") //로그인 성공 시
            {
                errortmp.text = "로그인 성공";
            }
            else
            {
                ErrorMessage(LoginError.None);
            }
        }
    }
}