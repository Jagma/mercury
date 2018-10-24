using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// The Game progression manager is a singleton
public class GameProgressionManager : MonoBehaviour
{
    public WebServiceMethods wsm;

    private int numEnemiesLeftLevel;
    private int numEnemiesStartLevel;
    private int numOfBulletsUsedTotal;
    private int numOfBulletsUsedLevel;
    private int enemiesKilledTotal;
    private int enemiesKilledLevel;
    private int wallsDestroyedLevel;
    private int wallsDestroyedTotal;

    
    private float damageTakenTotal;
    private float damageTakenLevel;

    private float currentStageTime;
    private float totalTimePlayed;

    public List<string> playerIDList = new List<string>();
    public List<string> playerName = new List<string>();
    public List<bool> playerDown = new List<bool>();
    public static GameProgressionManager instance;

    bool gameStart = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    #region Init
    // Use this for initialization
    void Start()
    {
        numOfBulletsUsedTotal = 0;
        numOfBulletsUsedLevel = 0;
        enemiesKilledTotal = 0;
        enemiesKilledLevel = 0;
        wallsDestroyedTotal = 0;
        wallsDestroyedLevel = 0;
        damageTakenTotal = 0;
        damageTakenLevel = 0;
        currentStageTime = 0;
        totalTimePlayed = 0;
        numEnemiesLeftLevel = 0;
        numEnemiesStartLevel = 0;
    }

    private void Update()
    {

    }
    public void SetPlayerList(string players,string name)
    {
        playerIDList.Add(players);
        playerName.Add(name);
        playerDown.Add(false);
    }

    public void StartGame()
    {
        gameStart = true;
    }
    #endregion

    #region LevelComplete/Restart

    public void RestartLevel()
    {
        numOfBulletsUsedLevel = 0;
        enemiesKilledLevel = 0;
        wallsDestroyedLevel = 0;
        damageTakenLevel = 0;
        currentStageTime = 0;
        numEnemiesLeftLevel = 0;
        numEnemiesStartLevel = 0;
    }

    public void LevelComplete()
    {
        RestartLevel();
        SceneManager.LoadScene("LevelComplete");
    }

    #endregion

    #region Game Updates

    public int getPlayerCount()
    {
        return playerIDList.Count;
    }

    public void IncreaseEnemyKills()
    {
        enemiesKilledLevel += 1;
    }

    public void IncreaseWallsDestroyed()
    {
        wallsDestroyedLevel += 1;
    }

    public void IncreaseBulletAmountUsed()
    {
        numOfBulletsUsedLevel += 1;
    }

    public void IncreasePlayerDamageTaken(float damage)
    {
        damageTakenLevel += damage;
    }
    public void IncreaseEnemyCount()
    {
        numEnemiesStartLevel += 1;
        numEnemiesLeftLevel  += 1;
    }

    public void DecreaseEnemiesLeft()
    {
        numEnemiesLeftLevel -= 1;
    }

    public bool getPlayerDownCount()
    {
        int count = 0;
        for (int i = 0; i < playerDown.Count; i++) //loop to check if all players are down.
        {
            if (playerDown[i] == true)
            {
                count++;
            }
        }

        if (count >= playerDown.Count) //checks to see if the countDown is equal or larger than the players down count.
            return true;
        else
            return false;
    }

    public void SetPlayerDown(string playerID, bool value)
    {
        for (int i = 0; i < playerIDList.Count; i++)//set player down in gameprogression manager to false.
        {
            if (playerIDList[i] == playerID)
            {
                playerDown[i] = value;
                return;
            }
        }

    }
    #endregion

    #region Database Updating
    void SendDataServer()
    {
        int currentSession = 0;

        //send data to database.
        WebServiceMethods wsm = new WebServiceMethods();
        var data = new NameValueCollection();
        UnityWebRequest request = UnityWebRequest.Post("https://webserver-itrw324.herokuapp.com/gq/addSessions?sessionPlayTime=" + totalTimePlayed.ToString(),data.ToString());
        request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
       // wsm.AddSessions(totalTimePlayed);
        //currentSession = int.Parse(wsm.GetLatestSession());

        //wsm.AddSessionEnemies(currentSession, enemiesKilledLevel);
        //wsm.AddSessionEnvironment(currentSession, wallsDestroyedTotal);
        //wsm.AddSessionWeapons(currentSession, numOfBulletsUsedTotal);
        //wsm.AddSessionPlayers(currentSession, damageTakenTotal);
    }
    #endregion

    #region Game Over
    public void GameOver()
    {
        SetFinalValues();
        SceneManager.LoadScene("GameOver");
    }

    void SetFinalValues()
    {
        numOfBulletsUsedTotal += numOfBulletsUsedLevel;
        enemiesKilledTotal += enemiesKilledLevel;
        wallsDestroyedTotal += wallsDestroyedLevel;
        damageTakenTotal += damageTakenLevel;
        currentStageTime = Time.timeSinceLevelLoad;
        totalTimePlayed += currentStageTime;      
    }


    public void Reset()
    {
        SendDataServer();
        Start(); //reset variables.
        for (int i = 0; i < playerIDList.Count; i++)//set player down in gameprogression manager to false.
        {
            playerDown[i] = false;
        }
    }

    #endregion

}

class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
{

    // Encoded RSAPublicKey
    private static string PUB_KEY = "mypublickey";
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        if (pk.ToLower().Equals(PUB_KEY.ToLower()))
            return true;
        return true;
    }
}
