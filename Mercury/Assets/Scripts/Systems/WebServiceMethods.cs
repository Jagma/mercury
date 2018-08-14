using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class WebServiceMethods : MonoBehaviour {

    private const string baseUrl = "http://ec2-34-243-14-94.eu-west-1.compute.amazonaws.com/";

    //Calls a REST request. Value is the API field
    private string restCall(string value)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + value);
        WebResponse response = request.GetResponse();

        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    //The following REST methods return the entire DB tables
    public string getCharacterTable() { return restCall("table/Character"); }
    public string getEnemyTable() { return restCall("table/Enemy"); }
    public string getSessionTable() { return restCall("table/Session"); }
    public string getSessionCharacterTable() { return restCall("table/SessionCharacter"); }
    public string getSessionEnemyTable() { return restCall("table/SessionEnemy"); }
    public string getSessionUserTable() { return restCall("table/SessionUser"); }
    public string getSessionWeaponTable() { return restCall("table/SessionWeapon"); }
    public string getTableList() { return restCall("table/TableList"); }
    public string getUserTable() { return restCall("table/User"); }
    public string getWeaponTable() { return restCall("table/Weapon"); }

}
