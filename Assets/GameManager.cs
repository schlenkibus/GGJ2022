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

    public Transform corner1;
    public Transform corner2;

    void Start()
    {
        
    }

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
        UnityWebRequest uwr = UnityWebRequest.Get("http://schlenkibus.de:8000/list-raw");
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
            marketName.text = uwr.error;
        else
        {            
            string stripped = uwr.downloadHandler.text.Replace(']', ' ').Replace('[', ' ').Replace('"', ' ').Trim();
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
            StartCoroutine(sendScannedItem());
        }
    }

    IEnumerator sendItemUpdate()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://schlenkibus.de:8000/stock-shelf/" + availableMarkets[selectedMarket]);
        yield return uwr.SendWebRequest();
    }

    IEnumerator sendScannedItem()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://schlenkibus.de:8000/scan-item/" + availableMarkets[selectedMarket]);
        yield return uwr.SendWebRequest();
    }

    IEnumerator sendPlayerPostion()
    {
        while(true)
        {
            Vector3 pos = player.transform.position;
            Vector3 maxV = Vector3.Max(corner1.transform.position, corner2.transform.position);
            Vector3 minV = Vector3.Min(corner1.transform.position, corner2.transform.position);

            pos -= minV; maxV -= minV;
            pos.x /= maxV.x; pos.x = Mathf.Max(0, Mathf.Min(1 - pos.x, 1));
            pos.z /= maxV.z; pos.z= Mathf.Max(0, Mathf.Min(pos.z, 1));

            UnityWebRequest uwr = UnityWebRequest.Get("http://schlenkibus.de:8000/player-position/" + availableMarkets[selectedMarket] + "/" + pos.x + "/" + pos.z);
            yield return uwr.SendWebRequest();
            yield return new WaitForSeconds(1);
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
        StartCoroutine(sendPlayerPostion());
    }

    IEnumerator getGameUpdate()
    {
        while (true)
        {
            UnityWebRequest uwr = UnityWebRequest.Get("http://schlenkibus.de:8000/game-update/" + availableMarkets[selectedMarket]);
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
                Debug.Log("Error While Sending: " + uwr.error);
            else
                applyChanges(JsonUtility.FromJson<GameState>(uwr.downloadHandler.text));

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void applyChanges(GameState newState)
    {
        onShelvesActivateStateChanged.Invoke(newState.activated);
        onNumShelvesItemStockedChanged.Invoke(newState.itemsStocked);
        onNumShelvesItemNeededChanged.Invoke(newState.itemsNeeded);
        onShelvesTimeChanged.Invoke(newState.timer);
        onLevelChanged.Invoke(newState.level);
        onRegisterActivateStateChanged.Invoke(newState.register_activated);
        onRegisterLevelChanged.Invoke(newState.register_level);
        onRegisterTimeChanged.Invoke(newState.register_timer);
        onNumRegisterItemNeededChanged.Invoke(newState.register_itemsNeeded);
        onNumRegisterItemStockedChanged.Invoke(newState.register_itemsStocked);
        onPlayerMoneyChanged.Invoke(newState.playerMoney);
        onBossMoneyChanged.Invoke(newState.bossMoney);
        gameState = newState;
    }
}
