using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Importante para usar SceneManager de Unity

public class ScenesManaging : MonoBehaviour
{
    public Button puc;
    public Button yes;
    public Button no;

    private void Start()
    {
        puc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().puc;
        yes = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().yes;
        no = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().no;
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene"); // Nombre de la escena que quieres cargar
    }
    
    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Nombre de la escena que quieres cargar
    }
    
    public void OpenPopUp()
    {
        puc.gameObject.SetActive(true);
        yes.gameObject.SetActive(true);
        no.gameObject.SetActive(true);
    }
    public void CancelExit()
    {
        puc.gameObject.SetActive(false);
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
    }
}
