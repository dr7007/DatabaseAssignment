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
        nameT.text = "이름 : " + name;
        goldT.text = "금액 : " + gold;
        functionT.text = "능력 : " + function;
    }
}
