using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Economy
    [SerializeField] private double startingMoney = 0;
    [SerializeField] private float baseCustomersPerSecond = 0.5f;
    [SerializeField] private float customersPerSecondPerPromoter = 1f;
    
    public double Money { get; private set; }
    public int HiredPromoters { get; private set; }
    
    public float CustomersPerSecond =>
        baseCustomersPerSecond + HiredPromoters * customersPerSecondPerPromoter;

    private void Awake()
    {
        Money = startingMoney;
    }
    
    public void AddMoney(double amount)
    {
        Money += amount;
    }

    public bool TrySpend(double amount)
    {
        if (Money < amount) return false;
        Money -= amount;
        return true;
    }
    
    public void AddPromoters(int count)
    {
        HiredPromoters += count;
    }
}
