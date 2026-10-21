using System.Collections.Generic;
using UnityEngine;

public class CustomerLine : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform standPoint;
    [SerializeField] private float spacing = 0.7f;
    [SerializeField] private int maxLength = 12;
    [SerializeField] private float slowestServeTime = 1f;
    [SerializeField] private float fastestServeTime = 0.15f;

    // index 0 = front
    private List<Customer> line = new List<Customer>();

    public bool IsFull => line.Count >= maxLength;

    // faster rhythm with more customers
    public float ServeTime =>
        Mathf.Clamp(1f / gameManager.CustomersPerSecond, fastestServeTime, slowestServeTime);

    public void Join(Customer c)
    {
        line.Add(c);
    }

    public void Leave(Customer c)
    {
        line.Remove(c);
    }

    public bool IsFront(Customer c)
    {
        return line.Count > 0 && line[0] == c;
    }

    // stand, then one step back per place
    public Vector3 GetSpot(Customer c)
    {
        int place = line.IndexOf(c);
        return standPoint.position + Vector3.left * spacing * place;
    }
}