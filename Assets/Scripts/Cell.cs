using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite highlight;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    public void Normal()
    {
        gameObject.SetActive(true);
        spriteRenderer.color = Color.white;
        spriteRenderer.sprite = normal;
    }
    private void Highlight()
    {
        gameObject.SetActive(true);
        spriteRenderer.color = Color.white;
        spriteRenderer.sprite = highlight;
    }
    public void Hover()
    {
        gameObject.SetActive(true);
        spriteRenderer.color = new(1.0f, 1.0f, 1.0f, 0.5f);
        spriteRenderer.sprite = highlight;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
