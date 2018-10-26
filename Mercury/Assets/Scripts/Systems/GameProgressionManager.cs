using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using System;

// The Game progression manager is a singleton
public class GameProgressionManager : MonoBehaviour
{
    public WebServiceMethods wsm;

    private int counter;
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

    private IEnumerator<WaitForSecondsRealtime> IFuckme()
    {
        int currentSession = 0;
        //asdas
        var data = new NameValueCollection();
        UnityWebRequest request = UnityWebRequest.Post("https://webserver-itrw324.herokuapp.com/gq/addSessions?sessionPlayTime=" + totalTimePlayed.ToString(), data.ToString());
        request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        request.SendWebRequest();

        UnityWebRequest request2 = UnityWebRequest.Get("https://webserver-itrw324.herokuapp.com/table/LatestSession");
        request2.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        request2.SendWebRequest();
        LatestSession ls = JsonConvert.DeserializeObject<LatestSession>(request2.downloadHandler.text);
        //int i = ls.results[0].max;
        counter = int.Parse(Readf());
        currentSession = counter;
        //Debug.Log(counter.ToString());

        yield return new WaitForSecondsRealtime(0.5f);

        UnityWebRequest request3 = UnityWebRequest.Post("https://webserver-itrw324.herokuapp.com/gq/addSessionEnvironment?sessionID=" + currentSession.ToString() + "&sessionWallsDestroyed=" + wallsDestroyedTotal.ToString(), data.ToString());
        request3.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        request3.SendWebRequest();
        yield return new WaitForSecondsRealtime(0.5f);

        UnityWebRequest request4 = UnityWebRequest.Post("https://webserver-itrw324.herokuapp.com/gq/addSessionEnemies?sessionID=" + currentSession.ToString() + "&sessionEnemiesKilled=" + enemiesKilledTotal.ToString(), data.ToString());
        request4.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        request4.SendWebRequest();
        yield return new WaitForSecondsRealtime(0.5f);

        UnityWebRequest request5 = UnityWebRequest.Post("https://webserver-itrw324.herokuapp.com/gq/addSessionWeapons?sessionID=" + currentSession.ToString() + "&sessionBulletsFired=" + numOfBulletsUsedTotal.ToString(), data.ToString());
        request5.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        request5.SendWebRequest();
        yield return new WaitForSecondsRealtime(0.5f);

        UnityWebRequest request6 = UnityWebRequest.Post("https://webserver-itrw324.herokuapp.com/gq/addSessionPlayers?sessionID=" + currentSession.ToString() + "&sessionDamageTaken=" + damageTakenTotal.ToString(), data.ToString());
        request6.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
        request6.SendWebRequest();

        Writef();
        //   yield return new WaitForSecondsRealtime(0.5f);
        ///Resets Game
        Start(); //reset variables.
        for (int i = 0; i < playerIDList.Count; i++)//set player down in gameprogression manager to false.
        {
            playerDown[i] = false;
        }
        //asd
    }

    #region Database Updating
    void SendDataServer()
    {
        int currentSession = 0;

        //send data to database.
        ////WebServiceMethods wsm = new WebServiceMethods();
        StartCoroutine(  IFuckme());
        //Debug.Log(counter);
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
    }

    #endregion

    #region Files
    public static string Readf()
    {
        string path = "Assets/Resources/counter.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string s  =reader.ReadLine();
        reader.Close();
        return s;
    }

    public void Writef()
    {
        counter++;
        string path = "Assets/Resources/counter.txt";

        //Write some text to the test.txt file
        System.IO.File.WriteAllText(path, counter.ToString());

    }
    #endregion

}

public class LatestSession
{
    public Result[] results { get; set; }
}

public class Result
{
    public int max { get; set; }
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
