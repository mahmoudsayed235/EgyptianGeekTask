using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] 
    private GameObject menuPanel;

    
    [SerializeField] 
    private Image menuButtonImage;
    [SerializeField] 
    private Sprite openSprite, closeSprite;

    public bool IsOpen { get; private set; }

    private void OnEnable()
    {
        closeMenu();
    }

    public void ToggleMenu()
    {
        if (IsOpen)
        {
            closeMenu();
        }else
        {
            openMenu();
        }
    }

    void openMenu()
    {
        IsOpen = true;
        menuPanel.SetActive(true);
        menuButtonImage.sprite = closeSprite;
    }

    void closeMenu()
    {
        IsOpen = false;
        menuPanel.SetActive(false);
        menuButtonImage.sprite = openSprite;
    }
}
