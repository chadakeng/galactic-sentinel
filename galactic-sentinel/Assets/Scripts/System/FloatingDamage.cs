using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;
    public float lifetime = 1f;
    private TextMeshProUGUI text;
    private Color textColor;
    private Transform target;
    private Camera mainCamera;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        textColor = text.color;
        mainCamera = Camera.main; // Get the main camera
        Destroy(gameObject, lifetime); // Auto-destroy after 1 second
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, 1.5f, 0); // Follow enemy
        }

        // Rotate to always face the player (billboard effect)
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0); // Correct text orientation

        // Move up slightly over time
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Fade out
        textColor.a -= fadeSpeed * Time.deltaTime;
        text.color = textColor;
    }

    public void SetDamage(float damageAmount, Transform enemyTransform)
    {
        text.text = damageAmount.ToString("F0"); // Show whole number
        target = enemyTransform; // Attach to enemy so it moves with them
    }
}