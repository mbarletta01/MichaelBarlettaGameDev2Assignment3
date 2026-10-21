using UnityEngine;
using System;
using TMPro;


public class UpgradeShop : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text hireLabel;

    [SerializeField] private double sellAmount = 1;
    [SerializeField] private int baseHireCost = 10;
    [SerializeField] private float costGrowth = 1.15f;

    public double CurrentHireCost =>
        baseHireCost * Math.Pow(costGrowth, gameManager.HiredPromoters);

    public void Sell()
    {
        gameManager.AddMoney(sellAmount);
    }

    public void Hire()
    {
        if(gameManager.TrySpend(CurrentHireCost))
        {
            gameManager.AddPromoters(1);
        }
    }

    private void Update()
    {
        moneyText.text = "$" + gameManager.Money.ToString("N0");
        hireLabel.text = $"Hire Promoter (${CurrentHireCost:N0})\nOwned: {gameManager.HiredPromoters}";
    }
    
    
}



