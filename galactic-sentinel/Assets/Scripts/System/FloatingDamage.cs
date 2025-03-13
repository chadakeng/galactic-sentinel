using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    public float moveSpeed = 1f;  // How fast text moves up
    public float fadeSpeed = 0.5f;  // Slow down fading effect**
    public float lifetime = 1.5f;  // Increase lifetime for better visibility**
    public TextMeshProUGUI text;  

    private Transform target;
    private Vector3 randomOffset;
    private Camera mainCamera;
    private float elapsedTime = 0f;
    private Color textColor;

    void Start()
    {
        mainCamera = Camera.main;
        if (text == null) 
        {
            text = GetComponentInChildren<TextMeshProUGUI>(); 
            if (text == null) 
            {
                Debug.LogError("FloatingDamage: TextMeshProUGUI is MISSING!");
                return;
            }
        }

        textColor = text.color;  // Store original color
        randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(1.0f, 1.5f), 0); 
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Move upwards
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Slow down the fade-out effect**
        if (elapsedTime >= lifetime * 0.5f)  // Start fading after 50% of lifetime
        {
            textColor.a -= fadeSpeed * Time.deltaTime;  
            text.color = textColor;
        }

        // Always face the camera
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }

    public void SetDamage(float damageAmount, Transform enemyTransform, bool isHealing)
    {
        if (text == null)
        {
            Debug.LogError("FloatingDamage: TextMeshProUGUI is NULL!");
            return;
        }

        text.text = damageAmount.ToString("F0");  // Set damage number
        target = enemyTransform;
        text.color = isHealing ? Color.green : Color.red;

        // Ensure full opacity when spawned**
        textColor = text.color;
        textColor.a = 1f;  
        text.color = textColor;

        // Set starting position slightly above enemy to prevent overlap
        transform.position = enemyTransform.position + randomOffset;
    }
}