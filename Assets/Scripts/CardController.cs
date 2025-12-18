using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CardController : MonoBehaviour
{
    private Sprite back;
    public Sprite Face;
    bool isBack;
    [HideInInspector]
    public string id, cardName;
    [HideInInspector]
    public int index;
    public bool isActive;
    [SerializeField]
    bool isOpened = false;

    public void Click()
    {
        if (!isOpened&&isActive)
        {
            GameController.Instance.SelectCard(this.gameObject);
        }
    }
    public void CreateCard(CardData cardData, int index)
    {
        isBack = true;
        back = GetComponent<Image>().sprite;
        id = cardData.id;
        cardName = cardData.cardName;
        Face = cardData.cardSprite;
        this.index = index;
        isActive = true;
    }
    public void CreateCard(string id,string cardName,Sprite cardSprite, int index, bool isActive)
    {
        this.isActive= isActive;
        if (isActive)
        {
            isBack = true;
            back = GetComponent<Image>().sprite;
            this.id = id;
            this.cardName = cardName;
            Face = cardSprite;
            this.index = index;
        }
        else
        {
            Hide();
        }
    }
    public void Hide()
    {
        isActive = false;
        GetComponent<Button>().enabled = false;
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
    public void FlipCard()
    {
        isOpened = !isOpened;
        GetComponent<Animator>().SetTrigger("flip");
        Invoke("changeCardFace", 0.25f);
    }
    void changeCardFace()
    {
        if (isBack)
        {
            GetComponent<Image>().sprite = Face;
        }
        else
        {
            GetComponent<Image>().sprite = back;
        }
        isBack = !isBack;
    }

}
