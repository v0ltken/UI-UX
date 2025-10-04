using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[Serializable]
public class SpriteData
{
    public string spriteName;
    public Sprite sprite;
    public int price;
}

public class SpriteDatabase : MonoBehaviour
{
    [SerializeField] private List<SpriteData> spriteList = new List<SpriteData>();

    [Header("UI References")]
    [SerializeField] private GameObject characterItemPrefab; // Prefab with Image, Texts, Button
    [SerializeField] private Transform contentParent;

        void Start()
    {
        PopulateScrollView();
    }

    void PopulateScrollView()
    {
        // Clear old items
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Create UI for each character
        foreach (SpriteData character in spriteList)
        {
            GameObject newItem = Instantiate(characterItemPrefab, contentParent);

            // Assign UI elements (assuming prefab has these children)
            newItem.transform.Find("CharacterImage").GetComponent<Image>().sprite = character.sprite;
            newItem.transform.Find("NameText").GetComponent< TextMeshProUGUI > ().text = character.spriteName;
            newItem.transform.Find("PriceText").GetComponent< TextMeshProUGUI > ().text = "" + character.price;

            // Optional: button functionality
            //Button buyButton = newItem.transform.Find("BuyButton").GetComponent<Button>();
            //buyButton.onClick.AddListener(() => OnBuyCharacter(character));
        }
    }

    void OnBuyCharacter(SpriteData character)
    {
        Debug.Log("Bought: " + character.spriteName + " for $" + character.price);
    }

    public Sprite GetSpriteByName(string name)
    {
        foreach (var data in spriteList)
        {
            if (data.spriteName == name)
                return data.sprite;
        }
        return null;
    }

    public List<SpriteData> GetAllSprites() => spriteList;
}
