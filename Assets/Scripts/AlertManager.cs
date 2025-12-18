using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class AlertManager : MonoBehaviour
{
    public static AlertManager Instance { get; private set; }
    [SerializeField]
    GameObject alert, loader;
    [SerializeField]
    TextMeshProUGUI text;
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Show(string msg)
    {
        alert.SetActive(true);
        text.text = msg;
        Invoke("hide", 1f);
    }
    void hide()
    {

        alert.SetActive(false);
        text.text = "";
    }
    public void ShowLoader()
    {
        loader.SetActive(true);

    }
    public void HideLoader()
    {

        loader.SetActive(false);

    }
}
