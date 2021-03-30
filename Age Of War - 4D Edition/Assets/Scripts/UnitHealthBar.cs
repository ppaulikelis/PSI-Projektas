using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthBar : MonoBehaviour
{
    public GameObject fill;
    public GameObject background;
    public Color low;
    public Color high;
    public Vector2 offset;

    private SpriteRenderer fillRenderer;

    private void Start()
    {
        background.transform.localPosition = offset;
        fillRenderer = fill.GetComponent<SpriteRenderer>();
    }

    public void SetHealth(float health, float maxHealth)
    {
        float normalized = health / maxHealth;
        fill.transform.localPosition = new Vector2(normalized / 2 ,0);
        fill.transform.localScale = new Vector2(normalized, 1);
        fillRenderer.color = Color.Lerp(low, high, normalized);
    }
}
