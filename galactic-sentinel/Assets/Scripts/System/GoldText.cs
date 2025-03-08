using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    void Update()
    {
        goldText.text = "Gold: " + GameManager.Instance.gold;
    }
}