using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class WebServiceMethods : MonoBehaviour {

    private const string baseUrl = "http://ec2-34-243-14-94.eu-west-1.compute.amazonaws.com/";

    //Calls a get REST request. Value is the API field
    private string restGET(string value)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + value);
        WebResponse response = request.GetResponse();

        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    //Calls a post REST request. Value is the API field
    private string restPOST(string value)
    {
        using (var wb = new WebClient())
        {
            var data = new NameValueCollection();
            var response = wb.UploadValues(baseUrl + value, "POST", data);
            return Encoding.UTF8.GetString(response);
        }
    }

    #region Insert into the DB
    public string addWeapon(int weaponID, string weaponName, string weaponType, int weaponFireRate, int numberOfPickups)
    {
        return restPOST(string.Format("gq/addWeapon?weaponID={0}&weaponName={1}&weaponType={2}&weaponFireRate={3}&numberOfPickups={4}", weaponID, weaponName, weaponType, weaponFireRate, numberOfPickups));
    }
    public string addUser(int userID, string userDeviceID, string userName, int userAccuracy, string favouriteCharacter)
    {
        return restPOST(string.Format("gq/addUser?userID={0}&userDeviceID={1}&userName={2}&userAccuracy={3}&favouriteCharacter={4}", userID, userDeviceID, userName, userAccuracy, favouriteCharacter));
    }
    public string addCharacter(int charID, string charName, string charDescription, int charHealth, string charSpecialAbility, int charMovementSpeed, int totalCharDeaths, int totalCharKills, int amountOfSpecialUses)
    {
        return restPOST(string.Format("gq/addCharacter?charID={0}&charName={1}&charDescription={2}&charHealth={3}&charSpecialAbility={4}&charMovementSpeed={5}&totalCharDeaths={6}&totalCharKills={7}&amountOfSpecialUses={8}", charID, charName, charDescription, charHealth, charSpecialAbility, charMovementSpeed, totalCharDeaths, totalCharKills, amountOfSpecialUses));
    }
    public string addEnemy(int enemyID, string enemyName, string enemyType, int enemyHealth, int enemyMovementSpeed, int totalCharKills)
    {
        return restPOST(string.Format("gq/addEnemy?enemyID={0}&enemyName={1}&enemyType={2}&enemyHealth={3}&enemyMovementSpeed={4}&totalCharKills={5}", enemyID, enemyName, enemyType, enemyHealth, enemyMovementSpeed, totalCharKills));
    }
    public string addSession(int sessionID, int sessionStart, int sessionEnd, string sessionUsers)
    {
        return restPOST(string.Format("gq/addSession?sessionID={0}&sessionStart={1}&sessionEnd={2}&sessionUsers={3}", sessionID, sessionStart, sessionEnd, sessionUsers));
    }
    public string addSessionWeapon(int sessionWeaponID, int sessionID, int weaponID, string weaponPickups)
    {
        return restPOST(string.Format("gq/addSessionWeapon?sessionWeaponID={0}&sessionID={1}&weaponID={2}&weaponPickups={3}", sessionWeaponID, sessionID, weaponID, weaponPickups));
    }
    public string addSessionUser(int sessionUserID, int sessionID, int userID, int sessionUserKills, int sessionUserDeaths, int sessionUserAccuracy, int sessionCharID)
    {
        return restPOST(string.Format("gq/addSessionUser?sessionUserID={0}&sessionID={1}&userID={2}&sessionUserKills={3}&sessionUserDeaths={4}&sessionUserAccuracy={5}&sessionCharID={6}", sessionUserID, sessionID, userID, sessionUserKills, sessionUserDeaths, sessionUserAccuracy, sessionCharID));
    }
    public string addSessionCharacter(int sessionCharID, int sessionID, int charID, int sessionCharKills, int sessionCharDeaths, int sessionCharSpecialUses)
    {
        return restPOST(string.Format("gq/addSessionCharacter?sessionCharID={0}&sessionID={1}&charID={2}&sessionCharKills={3}&sessionCharDeaths={4}&sessionCharSpecialUses={5}", sessionCharID, sessionID, charID, sessionCharKills, sessionCharDeaths, sessionCharSpecialUses));
    }
    public string addSessionEnemy(int sessionEnemyID, int sessionID, int enemyID, int sessionCharKills)
    {
        return restPOST(string.Format("gq/addSessionEnemy?sessionEnemyID={0}&sessionID={1}&enemyID={2}&sessionCharKills={3}", sessionEnemyID, sessionID, enemyID, sessionCharKills));
    }
    #endregion

    #region The following REST methods return the entire DB tables
    public string getCharacterTable() { return restGET("table/Character"); }
    public string getEnemyTable() { return restGET("table/Enemy"); }
    public string getSessionTable() { return restGET("table/Session"); }
    public string getSessionCharacterTable() { return restGET("table/SessionCharacter"); }
    public string getSessionEnemyTable() { return restGET("table/SessionEnemy"); }
    public string getSessionUserTable() { return restGET("table/SessionUser"); }
    public string getSessionWeaponTable() { return restGET("table/SessionWeapon"); }
    public string getTableList() { return restGET("table/TableList"); }
    public string getUserTable() { return restGET("table/User"); }
    public string getWeaponTable() { return restGET("table/Weapon"); }
    #endregion
}
