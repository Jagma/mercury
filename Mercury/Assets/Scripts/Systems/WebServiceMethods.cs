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
    private string RestCall(string value, string type)
    {
        using (var wb = new WebClient())
        {
            var data = new NameValueCollection();
            var response = wb.UploadValues(BASE_URL + value, type, data);
            return Encoding.UTF8.GetString(response);
        }
    }

    //Calls a POST REST request. Value is the API field
    private string RestPOST(string value)
    {
        return RestCall(value, "POST");
    }

    //Calls a PUT REST request. Value is the API field 
    private string RestPUT(string value)
    {
        return RestCall(value, "PUT");
    }

    //Calls a Delete REST request.Value is the API field
    private string RestDelete(string value)
    {
        return RestCall(value, "DELETE");
    }

    //Calls a get REST request. Value is the API field
    private string RestGET(string value)
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
    public string AddWeapon(string weaponName, string weaponType, int weaponFireRate, int numberOfPickups)
    {
        return RestPOST(string.Format("gq/addWeapon?weaponName={0}&weaponType={1}&weaponFireRate={2}&numberOfPickups={3}", weaponName, weaponType, weaponFireRate, numberOfPickups));
    }
    public string AddUser(int userDeviceID, string userName, int userAccuracy, string favouriteCharacter, int userKills, int userDeaths)
    {
        return RestPOST(string.Format("gq/addUser?userDeviceID={0}&userName={1}&userAccuracy={2}&favouriteCharacter={3}&userKills={4}&userDeaths={5}", userDeviceID, userName, userAccuracy, favouriteCharacter, userKills, userDeaths));
    }
    //update description by removing chardescriptions
    public string AddCharacter(string charName, int charHealth, string charSpecialAbility, int charMovementSpeed, int totalCharDeaths, int totalCharKills, int amountOfSpecialUses)
    {
        return RestPOST(string.Format("gq/addCharacter?charName={0}&charHealth={1}&charSpecialAbility={2}&charMovementSpeed={3}&totalCharDeaths={4}&totalCharKills={5}&amountOfSpecialUses={6}", charName, charHealth, charSpecialAbility, charMovementSpeed, totalCharDeaths, totalCharKills, amountOfSpecialUses));
    }
    public string AddEnemy(string enemyName, string enemyType, int enemyHealth, int enemyMovementSpeed, int totalCharKills)
    {
        return RestPOST(string.Format("gq/addEnemy?enemyName={0}&enemyType={1}&enemyHealth={2}&enemyMovementSpeed={3}&totalCharKills={4}", enemyName, enemyType, enemyHealth, enemyMovementSpeed, totalCharKills));
    }
    public string AddSession(int sessionUsers)
    {
        return RestPOST(string.Format("gq/addSession?sessionUsers={0}", sessionUsers));
    }
    public string AddSessionWeapon(int sessionID, int weaponID, int weaponPickups)
    {
        return RestPOST(string.Format("gq/addSessionWeapon?sessionID={0}&weaponID={1}&weaponPickups={2}", sessionID, weaponID, weaponPickups));
    }
    public string AddSessionUser(int sessionID, int userID, int sessionUserKills, int sessionUserDeaths, int sessionUserAccuracy, int sessionCharID)
    {
        return RestPOST(string.Format("gq/addSessionUser?sessionID={0}&userID={1}&sessionUserKills={2}&sessionUserDeaths={3}&sessionUserAccuracy={4}&sessionCharID={5}", sessionID, userID, sessionUserKills, sessionUserDeaths, sessionUserAccuracy, sessionCharID));
    }
    public string AddSessionCharacter(int sessionID, int charID, int sessionCharKills, int sessionCharDeaths, int sessionCharSpecialUses)
    {
        return RestPOST(string.Format("gq/addSessionCharacter?sessionID={0}&charID={1}&sessionCharKills={2}&sessionCharDeaths={3}&sessionCharSpecialUses={4}", sessionID, charID, sessionCharKills, sessionCharDeaths, sessionCharSpecialUses));
    }
    public string AddSessionEnemy(int sessionID, int enemyID, int sessionCharKills)
    {
        return RestPOST(string.Format("gq/addSessionEnemy?sessionID={0}&enemyID={1}&sessionCharKills={2}", sessionID, enemyID, sessionCharKills));
    }
    #endregion

    #region The following rest methods delete a value from the DB
    public string DeleteWeapon(int weaponID)
    {
        return RestDelete(string.Format("gq/deleteWeapon?weaponID={0}", weaponID));
    }
    public string DeleteUser(int userID)
    {
        return RestDelete(string.Format("gq/deleteUser?userID={0}", userID));
    }
    public string DeleteCharacter(int charID)
    {
        return RestDelete(string.Format("gq/deleteCharacter?charID={0}", charID));
    }
    public string DeleteEnemy(int enemyID)
    {
        return RestDelete(string.Format("gq/deleteEnemy?enemyID={0}", enemyID));
    }
    public string DeleteSession(int sessionID)
    {
        return RestDelete(string.Format("gq/deleteSession?sessionID={0}", sessionID));
    }
    public string DeleteSessionWeapon(int sessionWeaponID)
    {
        return RestDelete(string.Format("gq/deleteSessionWeapon?sessionWeaponID={0}", sessionWeaponID));
    }
    public string DeleteSessionUser(int sessionUserID)
    {
        return RestDelete(string.Format("gq/deleteSessionUser?sessionUserID={0}", sessionUserID));
    }
    public string DeleteSessionCharacter(int sessionCharID)
    {
        return RestDelete(string.Format("gq/deleteSessionCharacter?sessionCharID={0}", sessionCharID));
    }
    public string DeleteSessionEnemy(int sessionEnemyID)
    {
        return RestDelete(string.Format("gq/deleteSessionEnemy?sessionEnemyID={0}", sessionEnemyID));
    }
    #endregion


    #region The following REST methods return the entire DB tables
    public string GetCharacterTable() { return RestGET("table/Character"); }
    public string GetEnemyTable() { return RestGET("table/Enemy"); }
    public string GetSessionTable() { return RestGET("table/Session"); }
    public string GetSessionCharacterTable() { return RestGET("table/SessionCharacter"); }
    public string GetSessionEnemyTable() { return RestGET("table/SessionEnemy"); }
    public string GetSessionUserTable() { return RestGET("table/SessionUser"); }
    public string GetSessionWeaponTable() { return RestGET("table/SessionWeapon"); }
    public string GetTableList() { return RestGET("table/TableList"); }
    public string GetUserTable() { return RestGET("table/User"); }
    public string GetWeaponTable() { return RestGET("table/Weapon"); }
    #endregion
}
