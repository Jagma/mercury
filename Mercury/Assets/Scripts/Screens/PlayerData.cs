using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData {

    public static Dictionary<string, string> players = new Dictionary<string, string>();
   
    public static string GetCharacterName(string aPlayerID)
    {
        return players[aPlayerID];
    }

    public static void AddPlayer(string aPlayerID, string aCharacterName)
    {
        players.Add(aPlayerID, aCharacterName);
    }
}
