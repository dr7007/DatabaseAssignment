using System.Collections;
using System.Collections.Generic;
using Unity.UI;
using UnityEngine;

public class Pop : MonoBehaviour
{
    public GameObject explanePanel;

    private void Start()
    {
        explanePanel.SetActive(false);
    }

    //private void Update()
    //{
    //    if (explanePanel.activeSelf)
    //    {
    //        Vector3 offset = new Vector3(-200, -200, 0);
    //        explanePanel.transform.position = Input.mousePosition + offset;
    //    }
    //}

    public void OnPointEnter()
    {
        StartCoroutine("PointEnterDelay");
        //explanePanel.SetActive(true);
    }

    IEnumerator PointEnterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        explanePanel.SetActive(true);
    }

    public void OnPointExit()
    {
        StopCoroutine("PointEnterDelay");
        explanePanel.SetActive(false);
    }
}
