using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "CardDeck",
    menuName = "Memory Game/Card Deck",
    order = 2
)]
public class CardDeck : ScriptableObject
{
    public List<CardData> cards;
}
