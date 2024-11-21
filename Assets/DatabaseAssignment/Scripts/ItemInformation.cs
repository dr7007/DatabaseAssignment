using TMPro;
using UnityEngine;

using System.Collections.Generic;

public class ItemInformation : MonoBehaviour
{
    public Sprite itemSprite;  // 아이템 이미지
    public int quantity = 0;       // 아이템 보유량

    // 보유량 증가 함수
    public void IncreaseQuantity()
    {
        quantity++;
    }

    // 보유량 설정 함수
    public void SetQuantity(int amount)
    {
        quantity = amount;
    }
}
