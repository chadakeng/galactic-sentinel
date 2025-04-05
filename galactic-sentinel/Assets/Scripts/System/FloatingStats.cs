using TMPro;
using UnityEngine;

public class FloatingStats : MonoBehaviour
{
    public float moveUpSpeed = 30f;
    public float fadeDuration = 1f;

    private TextMeshProUGUI text;
    private Color originalColor;
    private float timer = 0f;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalColor = text.color;
    }

    void OnEnable()
    {
        timer = 0f;
        text.color = originalColor;
        transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Move upward
        transform.localPosition += Vector3.up * moveUpSpeed * Time.deltaTime;

        // Fade out
        Color c = text.color;
        c.a = Mathf.Lerp(originalColor.a, 0, timer / fadeDuration);
        text.color = c;

        // Disable after time
        if (timer > fadeDuration)
        {
            gameObject.SetActive(false);
        }
    }

    public void Show(string message)
    {
        text.text = message;
        gameObject.SetActive(true);
    }
}