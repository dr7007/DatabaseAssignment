using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Button Login_BT;

    public void LoginToScene()
    {
        SceneManager.LoadScene("Main");
    }
}
