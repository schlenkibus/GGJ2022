using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Valve.VR.InteractionSystem.Player player;
    public TextMesh marketName;
    public TextMesh confirm;
    public TextMesh next;

    public string baseAddress = "http://192.168.178.165:8000";

    private bool playerInitialized = false;

    private List<string> availableMarkets = new List<string>();
    private int selectedMarket = 0;
    private bool marketConfirmed = false;

    public GameState gameState;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isInitialized() && !playerInitialized)
        {
            playerInitialized = true;
            StartCoroutine(listAvailableGames());
        }
    }

    IEnumerator listAvailableGames()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://192.168.178.165:8000/list-raw");
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            marketName.text = uwr.error;
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            
            string stripped = uwr.downloadHandler.text.Replace(']', ' ');
            stripped = stripped.Replace('[', ' ');
            stripped = stripped.Replace('"', ' ');
            stripped = stripped.Trim();
            availableMarkets.AddRange(stripped.Split(','));
            marketName.text = availableMarkets[selectedMarket];
            Debug.Log(availableMarkets);
        }
    }

    public void test()
    {
        Debug.Log("Button Pressed");
    }

    public void onNext()
    {
        if(!marketConfirmed)
        {
            selectedMarket += 1;
            selectedMarket = Mathf.Max(Mathf.Min(selectedMarket, availableMarkets.Count - 1), 0);
            marketName.text = availableMarkets[selectedMarket];
        }
    }

    public void onPrevious()
    {
        if(!marketConfirmed)
        {
            selectedMarket += -1;
            selectedMarket = Mathf.Max(Mathf.Min(selectedMarket, availableMarkets.Count - 1), 0);
            marketName.text = availableMarkets[selectedMarket];
        }
    }

    public void onConfirm()
    {
        marketConfirmed = true;
        StartCoroutine(getGameUpdate());
    }

    IEnumerator getGameUpdate()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://192.168.178.165:8000/game-update/" + availableMarkets[selectedMarket]);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            marketName.text = uwr.error;
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            gameState = JsonUtility.FromJson<GameState>(uwr.downloadHandler.text);
        }
    }
}
