using System.Collections.Generic;
using UnityEngine;

public class CustomerPool : MonoBehaviour
{
    // prefab, starting count, cap
    [SerializeField] private Customer prefab;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private int maxSize = 100;

    // waiting for reuse
    private Stack<Customer> inactive = new Stack<Customer>();

    // total ever made
    private int totalCreated;

    // prewarm at load
    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Customer c = CreateNew();
            c.gameObject.SetActive(false);
            inactive.Push(c);
        }
    }

    // only instantiate
    private Customer CreateNew()
    {
        totalCreated++;
        return Instantiate(prefab, transform);
    }

    // hand one out
    public Customer Get()
    {
        Customer c;

        // reuse
        if (inactive.Count > 0)
            c = inactive.Pop();
        // make new, under cap
        else if (totalCreated < maxSize)
            c = CreateNew();
        // at cap
        else
            return null;

        // turn on
        c.gameObject.SetActive(true);
        return c;
    }

    // take back, turn off, store
    public void Release(Customer c)
    {
        c.gameObject.SetActive(false);
        inactive.Push(c);
    }
}