using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Camera mMainCamera;
    public GameObject mCarObject;
    public static CameraCtrl instance;
    
    //相对于车的相对位置，初始
    private Vector3 mOldPos;
    //相对于车的相对位置，目标位置
    private Vector3 mNewPos;
    //相机定位方式,1-直线;2-抛物线
    private int mFlyType;

    void Start()
    {
    }

    void Awake()
    {
        instance = this;
    }

    /*
     * dstPos:目标位置
     * dir:相机朝向
     * type:是否相对于汽车,1:是,0:否
     */
    public bool FlyTo(Vector3 dstPos,Vector3 dir,int type)
    {
        mFlyType = type;
        if (mFlyType == 1)
        {
            //相对于车的位置
            mMainCamera.transform.parent = mCarObject.transform;
            Vector3 rotationVector3 = new Vector3(90f, 0f, 0f);
            Quaternion rotation = Quaternion.Euler(rotationVector3);
            mMainCamera.transform.rotation = rotation;
            mMainCamera.transform.position = mCarObject.transform.position;

        } else if (mFlyType == 0)
        {
            //绝对坐标
            mMainCamera.transform.parent = null;
        }
        return true;
    }

}
