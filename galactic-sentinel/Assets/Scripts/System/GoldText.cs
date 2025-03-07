using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    public TextMeshProUGUI goldText; // Drag GoldText here

    void Update()
    {
        goldText.text = "Gold: " + GameManager.Instance.gold;
    }
}