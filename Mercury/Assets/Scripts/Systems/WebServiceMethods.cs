using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class WebServiceMethods : MonoBehaviour {

	public string GetJouMaSe____ByName(string aName)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://ec2-34-243-14-94.eu-west-1.compute.amazonaws.com/jou/" + aName);
        WebResponse response = request.GetResponse();

        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

}
