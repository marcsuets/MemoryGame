using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // Cartes + mats
    public GameObject[] cards = new GameObject[16];
    private List<GameObject> cardsList = new List<GameObject>();
    public List<Material> CardMaterials = new List<Material>();
    
    // State
    private bool isGameOver = false;

    // Gestió
    public int numTokensTouched;
    private int idToken1 = -1;
    private int idToken2 = -1;
    private Material mat1;
    private Material mat2;
    private int contadorWin;
    private float bestScore;
    
    // UI
    private float timer;
    private TextMeshProUGUI textTimer;
    private int tries;
    private TextMeshProUGUI textTries;
    private TextMeshProUGUI textWin;
    private TextMeshProUGUI textBestScore;
    private TextMeshProUGUI textNewBestTime;
    private Button nextButton;
    public Button puc;
    public Button yes;
    public Button no;
    
    // Audio
    public AudioSource audioSource;
    public AudioSource backgroundAudioSource;
    public AudioClip backgroundMusic;
    public AudioClip clickCard;
    public AudioClip match;
    public AudioClip noMatch;
    public AudioClip startShot;
    public AudioClip turnCard;
    public AudioClip victory;
    
    void Start()
    {
        numTokensTouched = 0;
        tries = 0;  
        isGameOver = false;
        contadorWin = 0;
        
        backgroundAudioSource.clip = backgroundMusic;
        backgroundAudioSource.volume = 0.3f;
        backgroundAudioSource.Play();
        Invoke("delayStartShot", 0.3f);

        // Cargar el BestScore de PlayerPrefs
        bestScore = PlayerPrefs.GetFloat("BestScore", 0f);

        // Encontrar los componentes de texto
        textBestScore = GameObject.FindGameObjectWithTag("TextBestScore").GetComponent<TextMeshProUGUI>();
        textWin = GameObject.FindGameObjectWithTag("Win").GetComponent<TextMeshProUGUI>();
        textTimer = GameObject.FindGameObjectWithTag("Timer").GetComponent<TextMeshProUGUI>();
        textTries = GameObject.FindGameObjectWithTag("Tries").GetComponent<TextMeshProUGUI>();
        textNewBestTime = GameObject.FindGameObjectWithTag("NewBestTime").GetComponent<TextMeshProUGUI>();
        nextButton = GameObject.FindGameObjectWithTag("NextButton").GetComponent<Button>();
        textNewBestTime.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        puc.gameObject.SetActive(false);
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        
        
        // Inicializar el texto
        textTries.text = "Tries: " + tries;
        textWin.gameObject.SetActive(false); // Asegúrate de que sea gameObject y no GameObject()
        textBestScore.text = "Best Time: " + bestScore.ToString("F2") + "s"; // Muestra el mejor tiempo

        // Asignar IDs y agregar cartas a la lista
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].transform.GetChild(0).GetComponent<Token>().id = i;
            cardsList.Add(cards[i]);
        }

        // Asignar materiales en pares
        for (int i = 0; i < 8; i++)
        {
            GameObject pair1 = GetRandToken();
            GameObject pair2 = GetRandToken();
            
            Material mat = CardMaterials[i];
            pair1.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = mat; // Material en el hijo 'Card'
            pair2.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = mat;
        }
    }

    private void Update()
    {
        if (!isGameOver)
        {
            timer += Time.deltaTime;
            textTimer.text = "Time: " + timer.ToString("F2") + " s";
   
            if (Win())
            {
                isGameOver = true; 
                audioSource.PlayOneShot(startShot);
                textWin.text = "You won!\nTime: " + timer.ToString("F2") + "s \nTries: " + tries;
                textWin.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);

                // Verificar y actualizar el mejor tiempo
                if (timer < bestScore || bestScore == 0)
                {
                    audioSource.PlayOneShot(victory);
                    textNewBestTime.gameObject.SetActive(true);
                    bestScore = timer;
                    textBestScore.text = "Best Time: " + bestScore.ToString("F2") + "s";
                    PlayerPrefs.SetFloat("BestScore", bestScore); // Guarda el nuevo mejor tiempo
                    PlayerPrefs.Save(); // Guarda de inmediato
                }
            } 
        }
    }

    private GameObject GetRandToken()
    {
        int randIndex = Random.Range(0, cardsList.Count);
        GameObject o = cardsList[randIndex];
        cardsList.RemoveAt(randIndex);
        return o;
    }

    public void SetSelectedCard(int id)
    {
        Token token = cards[id].transform.GetChild(0).GetComponent<Token>();

        if (numTokensTouched == 0)
        {
            idToken1 = id;
            numTokensTouched = 1;
            cards[idToken1].transform.GetChild(0).GetComponent<Token>().StartAnimation();
        }
        else if (numTokensTouched == 1 && idToken1 != id)
        {
            idToken2 = id;
            numTokensTouched = 2;
            cards[idToken2].transform.GetChild(0).GetComponent<Token>().StartAnimation();

            Invoke("CheckMatchAndProcess", 1.5f);
        }
    }

    private void CheckMatchAndProcess()
    {
        tries++;
        textTries.text = "Tries: " + tries;
        if (CheckForMatch())
        {
            audioSource.PlayOneShot(match);
            Invoke("DestroyAnimation", 0f);
            Invoke("resetNumTokensTouched", 1.5f);
            contadorWin++;
        }
        else
        {
            audioSource.PlayOneShot(noMatch);
            cards[idToken1].transform.GetChild(0).GetComponent<Animator>().SetTrigger("Hide");
            cards[idToken2].transform.GetChild(0).GetComponent<Animator>().SetTrigger("Hide");
            Invoke("resetNumTokensTouched", 1.5f); 
        }
    }

    private bool CheckForMatch()
    {
        mat1 = cards[idToken1].transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material;
        mat2 = cards[idToken2].transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material;
        
        return mat1.name == mat2.name;
    }

    private void resetNumTokensTouched()
    {
        numTokensTouched = 0;
    }
    
    private void DestroyAnimation()
    {
        cards[idToken1].transform.GetChild(0).GetComponent<Animator>().SetTrigger("Destroy");
        cards[idToken2].transform.GetChild(0).GetComponent<Animator>().SetTrigger("Destroy");
        Invoke("Destroy", 1.5f);
    }
    
    private void Destroy()
    {
        Destroy(cards[idToken1]);
        Destroy(cards[idToken2]);
    }
    
    private bool Win()
    {
     return contadorWin == 8;
    }

    private void delayStartShot()
    {
        audioSource.PlayOneShot(startShot);
    }
}
