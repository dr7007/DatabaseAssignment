using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TexTTT : MonoBehaviour
{
    public TextMeshProUGUI nameT;
    public TextMeshProUGUI goldT;
    public TextMeshProUGUI functionT;

    public void InventoryT(string name, string gold, string function)
    {
        nameT.text = "�̸� : " + name;
        goldT.text = "�ݾ� : " + gold;
        functionT.text = "�ɷ� : " + function;
    }
}
