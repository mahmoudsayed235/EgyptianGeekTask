using UnityEngine;

[CreateAssetMenu(
    fileName = "NewCardData",
    menuName = "Memory Game/Card Data",
    order = 1
)]
public class CardData : ScriptableObject
{
    [Header("Card Info")]
    public string id;
    public string cardName;
    public Sprite cardSprite;
}
