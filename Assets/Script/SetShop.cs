﻿using UnityEngine;
using System.Collections;

// 노승현,상점 구매하기 스크립트

public class SetShop : MonoBehaviour {

    public static int gold = 1000; // 유저 골드
    public static int gem = 500; // 유저 보석
    public UILabel gold_Label, gem_Label; 
             
    void Awake()
    {
        gold_Label.text = gold.ToString();
        gem_Label.text = gem.ToString();
	}
	void Update () 
    {
	
	}
    public void BuyCharacter(UILabel label) // 골드로 캐릭터 구매 함수
    {
        int price = int.Parse(label.text);
        if (price <= gold)
        {
            gold = gold - price;
            gold_Label.text = gold.ToString();
            Debug.Log("구매성공");
        }
        else
        {
            Debug.Log("구매실패");
        }
    }
    public void BuyGold(UILabel label) // 보석으로 골드구매 함수
    {
        int choose = int.Parse(label.text);
        if (choose <= gem)
        {
            gem = gem - choose;
            gem_Label.text = gem.ToString();
            Debug.Log("구매성공");
        }
        else
        {
            Debug.Log("구매실패");
        }
    }
}