using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    // refs
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CustomerPool pool;
    [SerializeField] private CustomerLine line;

    // path
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform exitPoint;

    // progress to next spawn
    private float spawnProgress;

    private void Update()
    {
        // build up spawns
        spawnProgress += gameManager.CustomersPerSecond * Time.deltaTime;

        // one per full point
        while (spawnProgress >= 1f)
        {
            spawnProgress -= 1f;

            // line full, skip
            if (line.IsFull) continue;

            // borrow, not instantiate
            Customer c = pool.Get();

            // skip if capped
            if (c != null)
            {
                c.Init(gameManager, pool, line, spawnPoint.position, exitPoint.position);
                line.Join(c);
            }
        }
    }
}