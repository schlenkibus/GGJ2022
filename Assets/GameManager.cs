using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    public UnityEvent<bool> onShelvesActivateStateChanged;
    public UnityEvent<int> onNumShelvesItemNeededChanged;
    public UnityEvent<int> onNumShelvesItemStockedChanged;
    public UnityEvent<int> onShelvesTimeChanged;
    public UnityEvent<int> onLevelChanged;

    public UnityEvent<bool> onRegisterActivateStateChanged;
    public UnityEvent<int> onNumRegisterItemNeededChanged;
    public UnityEvent<int> onNumRegisterItemStockedChanged;
    public UnityEvent<int> onRegisterTimeChanged;
    public UnityEvent<int> onRegisterLevelChanged;

    public UnityEvent<float> onPlayerMoneyChanged;
    public UnityEvent<float> onBossMoneyChanged;

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
            string stripped = uwr.downloadHandler.text.Replace(']', ' ');
            stripped = stripped.Replace('[', ' ');
            stripped = stripped.Replace('"', ' ');
            stripped = stripped.Trim();
            availableMarkets.AddRange(stripped.Split(','));
            marketName.text = availableMarkets[selectedMarket];
        }
    }

    public void onItemStocked()
    {
        if(gameState.activated)
        {
            StartCoroutine(sendItemUpdate());
        }
    }

    public void onItemScanned()
    {
        if(gameState.register_activated)
        {
            Debug.Log("onItemScanned");
            StartCoroutine(sendScannedItem());
        }
    }

    IEnumerator sendItemUpdate()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://192.168.178.165:8000/stock-shelf/" + availableMarkets[selectedMarket]);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    IEnumerator sendScannedItem()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://192.168.178.165:8000/scan-item/" + availableMarkets[selectedMarket]);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
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
        while (true)
        {
            UnityWebRequest uwr = UnityWebRequest.Get("http://192.168.178.165:8000/game-update/" + availableMarkets[selectedMarket]);
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                GameState state = JsonUtility.FromJson<GameState>(uwr.downloadHandler.text);
                applyChanges(state);
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void applyChanges(GameState newState)
    {
        //Shelf!
        onShelvesActivateStateChanged.Invoke(newState.activated);
        onNumShelvesItemStockedChanged.Invoke(newState.itemsStocked);
        onNumShelvesItemNeededChanged.Invoke(newState.itemsNeeded);
        onShelvesTimeChanged.Invoke(newState.timer);
        onLevelChanged.Invoke(newState.level);

        //Global!
        onPlayerMoneyChanged.Invoke(newState.playerMoney);
        onBossMoneyChanged.Invoke(newState.bossMoney);

        //Register
        onRegisterActivateStateChanged.Invoke(newState.register_activated);
        onRegisterLevelChanged.Invoke(newState.register_level);
        onRegisterTimeChanged.Invoke(newState.register_timer);
        onNumRegisterItemNeededChanged.Invoke(newState.register_itemsNeeded);
        onNumRegisterItemStockedChanged.Invoke(newState.register_itemsStocked);

        gameState = newState;
    }
}
