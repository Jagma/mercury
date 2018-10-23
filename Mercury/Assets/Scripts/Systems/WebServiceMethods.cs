using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class WebServiceMethods : MonoBehaviour {

   // private const string baseUrl = "http://ec2-34-243-14-94.eu-west-1.compute.amazonaws.com/";
    private const string BASE_URL = "https://webserver-itrw324.herokuapp.com/";

    #region REST methods
    //Rest call
    private string restCall(string value, string type)
    {
        using (var wb = new WebClient())
        {
            var data = new NameValueCollection();
            var response = wb.UploadValues(BASE_URL + value, type , data);
            return Encoding.UTF8.GetString(response);
        }
    }
    
    //Calls a POST REST request. Value is the API field
    private string RestPOST(string value)
    {
        return restCall(value, "POST");
    }

    //Calls a PUT REST request. Value is the API field 
    private string RestPUT(string value)
    {
        return restCall(value, "PUT");
    }

    //Calls a Delete REST request.Value is the API field
    private string RestDelete(string value)
    {
        return restCall(value, "DELETE");
    }

    //Calls a get REST request. Value is the API field
    private string restGET(string value)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BASE_URL + value);
        WebResponse response = request.GetResponse();

        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
    #endregion

    #region updates data in the DB
    public string UpdateWeapon(int weaponID, string weaponName, string weaponType, int weaponFireRate, int numberOfPickups)
    {
        return RestPUT(string.Format("gq/updateWeapon?weaponID={0}&weaponName={1}&weaponType={2}&weaponFireRate={3}&numberOfPickups={4}", weaponID, weaponName, weaponType, weaponFireRate, numberOfPickups));
    }
    public string UpdateUser(int userID, int userDeviceID, string userName, int userAccuracy, string favouriteCharacter)
    {
        return RestPUT(string.Format("gq/updateUser?userID={0}&userDeviceID={1}&userName={2}&userAccuracy={3}&favouriteCharacter={4}", userID, userDeviceID, userName, userAccuracy, favouriteCharacter));
    }
    public string UpdateCharacter(int charID, string charName, int charHealth, string charSpecialAbility, int charMovementSpeed, int totalCharDeaths, int totalCharKills, int amountOfSpecialUses)
    {
        return RestPUT(string.Format("gq/updateCharacter?charID={0}&charName={1}&charHealth={2}&charSpecialAbility={3}&charMovementSpeed={4}&totalCharDeaths={5}&totalCharKills={6}&amountOfSpecialUses={7}", charID, charName, charHealth, charSpecialAbility, charMovementSpeed, totalCharDeaths, totalCharKills, amountOfSpecialUses));
    }
    public string UpdateEnemy(int enemyID, string enemyName, string enemyType, int enemyHealth, int enemyMovementSpeed, int totalCharKills)
    {
        return RestPUT(string.Format("gq/updateEnemy?enemyID={0}&enemyName={1}&enemyType={2}&enemyHealth={3}&enemyMovementSpeed={4}&totalCharKills={5}", enemyID, enemyName, enemyType, enemyHealth, enemyMovementSpeed, totalCharKills));
    }
    public string UpdateSession(int sessionID, int sessionStart, int sessionEnd, int sessionUsers)
    {
        return RestPUT(string.Format("gq/updateSession?sessionID={0}&sessionStart={1}&sessionEnd={2}&sessionUsers={3}", sessionID, sessionStart, sessionEnd, sessionUsers));
    }
    public string UpdateSessionWeapon(int sessionWeaponID, int sessionID, int weaponID, int weaponPickups)
    {
        return RestPUT(string.Format("gq/updateSessionWeapon?sessionWeaponID={0}&sessionID={1}&weaponID={2}&weaponPickups={3}", sessionWeaponID, sessionID, weaponID, weaponPickups));
    }
    public string UpdateSessionUser(int sessionUserID, int sessionID, int userID, int sessionUserKills, int sessionUserDeaths, int sessionUserAccuracy, int sessionCharID)
    {
        return RestPUT(string.Format("gq/updateSessionUser?sessionUserID={0}&sessionID={1}&userID={2}&sessionUserKills={3}&sessionUserDeaths={4}&sessionUserAccuracy={5}&sessionCharID={6}", sessionUserID, sessionID, userID, sessionUserKills, sessionUserDeaths, sessionUserAccuracy, sessionCharID));
    }
    public string UpdateSessionCharacter(int sessionCharID, int sessionID, int charID, int sessionCharKills, int sessionCharDeaths, int sessionCharSpecialUses)
    {
        return RestPUT(string.Format("gq/updateSessionCharacter?sessionCharID={0}&sessionID={1}&charID={2}&sessionCharKills={3}&sessionCharDeaths={4}&sessionCharSpecialUses={5}", sessionCharID, sessionID, charID, sessionCharKills, sessionCharDeaths, sessionCharSpecialUses));
    }
    public string UpdateSessionEnemy(int sessionEnemyID, int sessionID, int enemyID, int sessionCharKills)
    {
        return RestPUT(string.Format("gq/updateSessionEnemy?sessionEnemyID={0}&sessionID={1}&enemyID={2}&sessionCharKills={3}", sessionEnemyID, sessionID, enemyID, sessionCharKills));
    }
    #endregion

    #region Insert into the DB
    public string addWeapon(int weaponID, string weaponName, string weaponType, int weaponFireRate, int numberOfPickups)
    {
        return RestPOST(string.Format("gq/addWeapon?weaponID={0}&weaponName={1}&weaponType={2}&weaponFireRate={3}&numberOfPickups={4}", weaponID, weaponName, weaponType, weaponFireRate, numberOfPickups));
    }
    public string addUser(int userID, int userDeviceID, string userName, int userAccuracy, string favouriteCharacter)
    {
        return RestPOST(string.Format("gq/addUser?userID={0}&userDeviceID={1}&userName={2}&userAccuracy={3}&favouriteCharacter={4}", userID, userDeviceID, userName, userAccuracy, favouriteCharacter));
    }
    //update description by removing chardescriptions
    public string addCharacter(int charID, string charName, int charHealth, string charSpecialAbility, int charMovementSpeed, int totalCharDeaths, int totalCharKills, int amountOfSpecialUses)
    {
        return RestPOST(string.Format("gq/addCharacter?charID={0}&charName={1}&charHealth={2}&charSpecialAbility={3}&charMovementSpeed={4}&totalCharDeaths={5}&totalCharKills={6}&amountOfSpecialUses={7}", charID, charName, charHealth, charSpecialAbility, charMovementSpeed, totalCharDeaths, totalCharKills, amountOfSpecialUses));
    }
    public string addEnemy(int enemyID, string enemyName, string enemyType, int enemyHealth, int enemyMovementSpeed, int totalCharKills)
    {
        return RestPOST(string.Format("gq/addEnemy?enemyID={0}&enemyName={1}&enemyType={2}&enemyHealth={3}&enemyMovementSpeed={4}&totalCharKills={5}", enemyID, enemyName, enemyType, enemyHealth, enemyMovementSpeed, totalCharKills));
    }
    public string addSession(int sessionID, int sessionStart, int sessionEnd, int sessionUsers)
    {
        return RestPOST(string.Format("gq/addSession?sessionID={0}&sessionStart={1}&sessionEnd={2}&sessionUsers={3}", sessionID, sessionStart, sessionEnd, sessionUsers));
    }
    public string addSessionWeapon(int sessionWeaponID, int sessionID, int weaponID, int weaponPickups)
    {
        return RestPOST(string.Format("gq/addSessionWeapon?sessionWeaponID={0}&sessionID={1}&weaponID={2}&weaponPickups={3}", sessionWeaponID, sessionID, weaponID, weaponPickups));
    }
    public string addSessionUser(int sessionUserID, int sessionID, int userID, int sessionUserKills, int sessionUserDeaths, int sessionUserAccuracy, int sessionCharID)
    {
        return RestPOST(string.Format("gq/addSessionUser?sessionUserID={0}&sessionID={1}&userID={2}&sessionUserKills={3}&sessionUserDeaths={4}&sessionUserAccuracy={5}&sessionCharID={6}", sessionUserID, sessionID, userID, sessionUserKills, sessionUserDeaths, sessionUserAccuracy, sessionCharID));
    }
    public string addSessionCharacter(int sessionCharID, int sessionID, int charID, int sessionCharKills, int sessionCharDeaths, int sessionCharSpecialUses)
    {
        return RestPOST(string.Format("gq/addSessionCharacter?sessionCharID={0}&sessionID={1}&charID={2}&sessionCharKills={3}&sessionCharDeaths={4}&sessionCharSpecialUses={5}", sessionCharID, sessionID, charID, sessionCharKills, sessionCharDeaths, sessionCharSpecialUses));
    }
    public string addSessionEnemy(int sessionEnemyID, int sessionID, int enemyID, int sessionCharKills)
    {
        return RestPOST(string.Format("gq/addSessionEnemy?sessionEnemyID={0}&sessionID={1}&enemyID={2}&sessionCharKills={3}", sessionEnemyID, sessionID, enemyID, sessionCharKills));
    }
    #endregion

    #region The following rest methods delete a value from the DB
    public string deleteWeapon(int weaponID)
    {
        return RestDelete(string.Format("gq/deleteWeapon?weaponID={0}", weaponID));
    }
    public string deleteUser(int userID)
    {
        return RestDelete(string.Format("gq/deleteUser?userID={0}", userID));
    }
    public string deleteCharacter(int charID)
    {
        return RestDelete(string.Format("gq/deleteCharacter?charID={0}", charID));
    }
    public string deleteEnemy(int enemyID)
    {
        return RestDelete(string.Format("gq/deleteEnemy?enemyID={0}", enemyID));
    }
    public string deleteSession(int sessionID)
    {
        return RestDelete(string.Format("gq/deleteSession?sessionID={0}", sessionID));
    }
    public string deleteSessionWeapon(int sessionWeaponID)
    {
        return RestDelete(string.Format("gq/deleteSessionWeapon?sessionWeaponID={0}", sessionWeaponID));
    }
    public string deleteSessionUser(int sessionUserID)
    {
        return RestDelete(string.Format("gq/deleteSessionUser?sessionUserID={0}", sessionUserID));
    }
    public string deleteSessionCharacter(int sessionCharID)
    {
        return RestDelete(string.Format("gq/deleteSessionCharacter?sessionCharID={0}", sessionCharID));
    }
    public string deleteSessionEnemy(int sessionEnemyID)
    {
        return RestDelete(string.Format("gq/deleteSessionEnemy?sessionEnemyID={0}", sessionEnemyID));
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
