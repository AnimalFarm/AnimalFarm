using UnityEngine;
using System.Collections;

// 노승현,상점 구매하기 스크립트

public class SetShop : MonoBehaviour {

    public static int gold = 100; // 유저 골드
    public static int gem = 500; // 유저 보석
    public UILabel gold_Label, gem_Label;
    public bool buyCharacter = false;
    public UIPanel shoptUIPanel;
    public UIGrid shopCharacterGrid;
    public CharacterChoice character;
             
    void Awake()
    {
        gold_Label.text = gold.ToString();
        gem_Label.text = gem.ToString();
	}
    public void BuyCharacter(string label,GameObject character) // 골드로 캐릭터 구매 함수
    {
        int price = int.Parse(label);
        SuccessCharacter(character);
        gold = gold - price;
        gold_Label.text = gold.ToString();
    }
    public void BuyGold(string label, string pay) // 보석으로 골드구매 함수
    {
        int choose = int.Parse(label);
        int price = int.Parse(pay);
        gem = gem - choose;
        gold = gold + price;
        gem_Label.text = gem.ToString();
        gold_Label.text = gold.ToString();
    }

    public void SuccessCharacter(GameObject buyCharacter)
    {
        buyCharacter.SetActive(false);
        shopCharacterGrid.Reposition();
        character.Character(buyCharacter);
    }
}
