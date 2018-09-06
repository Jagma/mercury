using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignLobbyJuice : MonoBehaviour {

    public Text titleText;
    public AnimationCurve titleCurve;
	void Start () {
       // StartCoroutine(ETitleJuice());
	}
	
	void Update () {
        //  titleText.transform.localScale = Vector3.Lerp(titleText.transform.localScale, Vector3.one, Time.deltaTime * 10);
        titleText.transform.localScale = Vector3.one * titleCurve.Evaluate(Time.time);
    }

    IEnumerator ETitleJuice () {
        titleText.transform.localScale += new Vector3(0.1f, 0.15f, 0);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ETitleJuice());
    }
}
