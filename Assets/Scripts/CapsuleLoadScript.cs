using UnityEngine;
using System.Collections;

public class CapsuleLoadScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("CapsuleLoadScript started");
        RcRoadXMLParser roadDataParser = new RcRoadXMLParser();
        roadDataParser.loadXML();
        roadDataParser.createRoadScence();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
