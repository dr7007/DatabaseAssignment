using TMPro;
using UnityEngine;

using System.Collections.Generic;

public class ItemInformation : MonoBehaviour
{
    public Sprite itemSprite;  // ������ �̹���
    public int quantity = 0;       // ������ ������

    // ������ ���� �Լ�
    public void IncreaseQuantity()
    {
        quantity++;
    }

    // ������ ���� �Լ�
    public void SetQuantity(int amount)
    {
        quantity = amount;
    }
}
