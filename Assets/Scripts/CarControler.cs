using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControler : MonoBehaviour {
    public GameObject mCarObject;
    public static CarControler instance;

    private CameraCtrl mMainCamera;
    // Use this for initialization
    void Start () {
    }

    void Awake()
    {
        instance = this;
    }

    public void SetCarPose(Vector3 position)
    {
        mCarObject.transform.position = position;
    }

    public Vector3 GetCarPos()
    {
        return mCarObject.transform.position;
    }

    public void ResetCarPose()
    {

    }

    // Update is called once per frame
    void Update()
    {
        QuickKeyCheck();
        KeyControl();

        //模型贴地检测处理
        //Vector3 dstPos = mCarObject.transform.position;
        //dstPos.y += 50;

        //Vector3 dir = dstPos - mCarObject.transform.position;
        //Debug.Log(dir);
        //Ray ray = new Ray(mCarObject.transform.position, -dir * 9999);
        //RaycastHit hitInfo;
        //if (Physics.Raycast(ray, out hitInfo, 9999f))
        //{
        //    Debug.Log("-----");
        //}
    }

    //快捷键处理函数
    private void QuickKeyCheck()
    {
        //快捷键
        if (Input.GetKeyDown(KeyCode.F1))
        {
            CameraCtrl.instance.FlyTo(new Vector3(0, 150, 0),new Vector3(0.0f, -1.0f, 0.0f), 0);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            CameraCtrl.instance.FlyTo(new Vector3(0, 500, 200), new Vector3(0.0f, 1.0f, -2.0f), 1);
        }
    }

    //键盘控制汽车
    private void KeyControl()
    {
        //车辆控制
        if (Input.GetKey(KeyCode.W))
        {
            mCarObject.transform.Translate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            mCarObject.transform.Translate(Vector3.back);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            mCarObject.transform.Translate(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            mCarObject.transform.Translate(Vector3.right);
        }
    }
}
