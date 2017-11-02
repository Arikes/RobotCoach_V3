using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControler : MonoBehaviour {
    public GameObject mCarObject;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //Debug.Log("W按键");
            mCarObject.transform.Translate(Vector3.forward);
        } else if (Input.GetKey(KeyCode.S))
        {
            //Debug.Log("S按键");
            mCarObject.transform.Translate(Vector3.back);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //Debug.Log("A按键");
            mCarObject.transform.Translate(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //Debug.Log("D按键");
            mCarObject.transform.Translate(Vector3.right);
        }

        Vector3 dstPos = mCarObject.transform.position;
        dstPos.y += 50;

        Vector3 dir = dstPos - mCarObject.transform.position;
        Ray ray = new Ray(mCarObject.transform.position, -dir * 9999);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 9999f))
        {
            Debug.Log("-----");
        }
    }
}
