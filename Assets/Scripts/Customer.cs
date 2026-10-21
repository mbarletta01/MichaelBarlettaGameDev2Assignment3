using TMPro;
using UnityEngine;

public class Customer : MonoBehaviour
{
    // tuning
    [SerializeField] private float speed = 3f;
    [SerializeField] private double payAmount = 1;

    // sale popup
    [SerializeField] private TMP_Text saleText;
    [SerializeField] private float saleTextDuration = 0.8f;
    [SerializeField] private float saleTextRiseSpeed = 1.5f;
    private float saleTextTimer;

    // walk states
    private enum Phase { InLine, WalkingOut }

    // per-trip data, reset in init
    private Phase phase;
    private float waitTimer;
    private Vector3 exitPos;
    private GameManager gameManager;
    private CustomerPool pool;
    private CustomerLine line;

    // cached once
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // reset on every reuse
    public void Init(GameManager gm, CustomerPool owningPool, CustomerLine customerLine, Vector3 spawnPos, Vector3 exit)
    {
        // refs + path
        gameManager = gm;
        pool = owningPool;
        line = customerLine;
        exitPos = exit;

        // back to start
        transform.position = spawnPos;
        phase = Phase.InLine;
        waitTimer = -1f; // not being served yet

        // new color, shows reuse
        spriteRenderer.color = Random.ColorHSV();

        // hide popup from last trip
        saleText.gameObject.SetActive(false);
        saleTextTimer = 0f;
    }

    private void Update()
    {
        UpdateSaleText();

        switch (phase)
        {
            // walk to my spot in line
            case Phase.InLine:
                Vector3 spot = line.GetSpot(this);
                transform.position = Vector3.MoveTowards(transform.position, spot, speed * Time.deltaTime);

                // front + at stand: get served
                if (line.IsFront(this) && transform.position == spot)
                {
                    // start serve timer once
                    if (waitTimer < 0f)
                        waitTimer = line.ServeTime;

                    waitTimer -= Time.deltaTime;

                    // pay, leave line
                    if (waitTimer <= 0f)
                    {
                        gameManager.AddMoney(payAmount);
                        ShowSaleText();
                        line.Leave(this);
                        phase = Phase.WalkingOut;
                    }
                }
                break;

            // walk off
            case Phase.WalkingOut:
                transform.position = Vector3.MoveTowards(transform.position, exitPos, speed * Time.deltaTime);
                if (transform.position == exitPos)
                {
                    Leave();
                }
                break;
        }
    }

    // show popup at sale
    private void ShowSaleText()
    {
        saleText.text = "+$" + payAmount.ToString("N0");
        saleText.transform.localPosition = new Vector3(0f, 1.2f, 0f);
        saleText.alpha = 1f;
        saleText.gameObject.SetActive(true);
        saleTextTimer = saleTextDuration;
    }

    // float up, fade, hide
    private void UpdateSaleText()
    {
        if (saleTextTimer <= 0f) return;

        saleTextTimer -= Time.deltaTime;
        saleText.transform.localPosition += Vector3.up * saleTextRiseSpeed * Time.deltaTime;
        saleText.alpha = saleTextTimer / saleTextDuration;

        if (saleTextTimer <= 0f)
        {
            saleText.gameObject.SetActive(false);
        }
    }

    // back to pool, no destroy
    private void Leave()
    {
        pool.Release(this);
    }
}