/*********************************************/
/* Copyright(c) 2017BEIJING YIJIAJIA COMPANY corporation,
/* All Rights Reserved.
/*
/* 文件描述
/*
/* 时间：2017-05-28
/* 作者： lijt
/* 描述：几何计算单元
/* 备注：
/*********************************************/


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RcGeometry
{
    //zx修改，右手坐标系转换为左手坐标系
    public static void createPolygonVertexs(List<Vector3> lstVertexsIn, ref List<Vector3> lstVertexsOut, float fWidth)
    {
        lstVertexsOut.Clear();
        for (int i = 0; i < lstVertexsIn.Count - 1; i++)
        {
            int nCurr = i;
            int nNext = i + 1;
            //求解直线方程的系数
            double fLineA = lstVertexsIn[nCurr].z - lstVertexsIn[nNext].z;
            double fLineB = lstVertexsIn[nNext].x - lstVertexsIn[nCurr].x;

            double fLineC = lstVertexsIn[nCurr].x * lstVertexsIn[nNext].z
               - lstVertexsIn[nNext].x * lstVertexsIn[nCurr].z;
            //求平行线的系数
            float fParallelLineC = (float)(fWidth * Math.Sqrt(fLineA * fLineA + fLineB * fLineB) + fLineC);
            //计算垂足
            float fCurrX = (float)((fLineB * fLineB * lstVertexsIn[nCurr].x - fLineA * fLineB * lstVertexsIn[nCurr].z -
                fLineA * fParallelLineC) / (fLineA * fLineA + fLineB * fLineB));
            float fCurrY = (float)((-fLineA * fLineB * lstVertexsIn[nCurr].x + fLineA * fLineA * lstVertexsIn[nCurr].z -
                fLineB * fParallelLineC) / (fLineA * fLineA + fLineB * fLineB));
            //输出数据
            //Vector3 oVec3 = new Vector3(fCurrX, fCurrY, lstVertexsIn[nCurr].z);
            Vector3 oVec3 = new Vector3(fCurrX, lstVertexsIn[nCurr].y, fCurrY);
            lstVertexsOut.Add(oVec3);

            double fNextX = (fLineB * fLineB * lstVertexsIn[nNext].x - fLineA * fLineB * lstVertexsIn[nNext].z -
                fLineA * fParallelLineC) / (fLineA * fLineA + fLineB * fLineB);
            double fNextY = (-fLineA * fLineB * lstVertexsIn[nNext].x + fLineA * fLineA * lstVertexsIn[nNext].z -
                fLineB * fParallelLineC) / (fLineA * fLineA + fLineB * fLineB);

            //oVec3.x = (float)fNextX;
            // oVec3.y = (float)fNextY;
            //oVec3.z = (float)lstVertexsIn[nNext].z;
            oVec3.x = (float)fNextX;
            oVec3.y = (float)lstVertexsIn[nNext].y;
            oVec3.z = (float)fNextY;
            lstVertexsOut.Add(oVec3);
        }
    }

    //清除序列中的无效数据
    //lstVertexsMid-生成矩形另一侧点数据，含冗余数据
    //lstVertexsOut-最终输出的可绘图的顶点数据
    public static void ConvertForCD(List<Vector3> lstVertexsIn ,ref List<Vector3> lstVertexsOut)
    {
        //for (int i = 0; i < lstVertexsIn.Count; i++)
        //{
        //    if (i % 2 == 0)
        //        lstVertexsOut.Add(lstVertexsIn[i]);
        //    if(i == lstVertexsIn.Count - 1)
        //        lstVertexsOut.Add(lstVertexsIn[i]);
        //}

        lstVertexsOut.Add(lstVertexsIn[0]);
        for (int i = 1; i < lstVertexsIn.Count - 1; i++)
        {
            if (i % 2 != 0)
            {
                lstVertexsOut.Add(lstVertexsIn[i]);
            }
        }
        lstVertexsOut.Add(lstVertexsIn[lstVertexsIn.Count - 1]);
    }

    public static void splitLine(Vector3 beginPoint,Vector3 endPoint,ref List<Vector3> outPoints,int segCount)
    {
        Vector3 sub = SubVec3(beginPoint,endPoint);
        sub.x =(float) (sub.x / (segCount*2));
        sub.y = (float)(sub.y / (segCount * 2));
        sub.z = (float)(sub.z / (segCount * 2));

        for (int i = 0; i < segCount*2; i ++)
        {
            outPoints.Add(new Vector3(beginPoint.x + (i) * sub.x, beginPoint.y + (i) * sub.y, beginPoint.z + (i) * sub.z));
        }
    }
    //生成多边形
    public static void createQuad(Vector3[] oVerts,Color color,string name)
    {
        //设置法线
        Vector3[] vecNormals = new Vector3[oVerts.Length];
        for (int i = 0; i < oVerts.Length; i++)
        {
            vecNormals[i] = Vector3.up;
        }
        int nTrianglesCount = oVerts.Length - 2;
        int[] vecTriangles = new int[nTrianglesCount * 3];
        //三角形顶点索引
        for (int i = 0; i < nTrianglesCount; i++)
        {
            for (int j = 0; j < 3; ++j)
            {
                vecTriangles[i * 3 + j] = j == 0 ? 0 : 3 - j + i;
            }
        }
        Mesh oMesh = new Mesh();
        oMesh.vertices = oVerts;
        oMesh.triangles = vecTriangles;
        oMesh.normals = vecNormals;

        GameObject oRoadRoot = GameObject.Find("RoadRoot");
        GameObject oPolygon = new GameObject();
        oPolygon.name = name;
        oPolygon.transform.parent = oRoadRoot.transform;
        oPolygon.AddComponent<MeshFilter>();
        oPolygon.AddComponent<MeshRenderer>();
        oPolygon.GetComponent<MeshFilter>().mesh = oMesh;
        oPolygon.GetComponent<Renderer>().material.color = color;
    }
    public static void create3dText(string text,Vector3 pos,Vector3 rotation)
    {
        GameObject oRoadRoot = GameObject.Find("RoadRoot");
        GameObject objj = new GameObject();
        TextMesh textMesh = objj.AddComponent<TextMesh>() as TextMesh;
        textMesh.text = text;
        objj.transform.position = pos;
        objj.transform.rotation = Quaternion.Euler(rotation);
        objj.transform.parent = oRoadRoot.transform;
        objj.GetComponent<Renderer>().material.color = Color.red;
    }

    public static void createPoly(Vector3[] oLeft1, Vector3[] oLeft2,Color useColor,string lineName)
    {
        List<Vector3> listVerts = new List<Vector3>();
        foreach (Vector3 item in oLeft1)
        {
            listVerts.Add(item);
        }
        foreach (Vector3 item in oLeft2)
        {
            listVerts.Add(item);
        }
        //所有顶点保存在oVerts
        Vector3[] oVerts = listVerts.ToArray();
        //顶点总数(0,half-1) 左侧正序，(half-end-1)左侧又一遍
        int triangleCount = (oLeft1.Length * 2) - 2;

        //设置法线
        Vector3[] vecNormals = new Vector3[oVerts.Length];
        for (int i = 0; i < oVerts.Length; i++)
        {
            vecNormals[i] = Vector3.up;
        }

        //计算三角形索引
        int[] vecTrianglesIndex = new int[triangleCount * 3];
        int leftSubIndex = 0,rightSubIndex = oVerts.Length/2;
        for (int i = 0; i < triangleCount; i++)
        {
            if (i % 2 == 0)
            {
                vecTrianglesIndex[i * 3] = leftSubIndex;
                vecTrianglesIndex[i * 3 + 1] = leftSubIndex+1;
                vecTrianglesIndex[i * 3 + 2] = rightSubIndex;
                leftSubIndex++;
            }
            else
            {
                vecTrianglesIndex[i * 3] = rightSubIndex;
                vecTrianglesIndex[i * 3 + 1] = leftSubIndex;
                vecTrianglesIndex[i * 3 + 2] = rightSubIndex + 1 ;
                rightSubIndex++;
            }

        }

        //绘制网格
        Mesh oMesh = new Mesh();
        oMesh.vertices = oVerts;
        oMesh.triangles = vecTrianglesIndex;
        oMesh.normals = vecNormals;

        GameObject oRoadRoot = GameObject.Find("RoadRoot");
        GameObject oPolygon = new GameObject();
        oPolygon.name = lineName;
        oPolygon.transform.parent = oRoadRoot.transform;
        oPolygon.AddComponent<MeshFilter>();
        oPolygon.AddComponent<MeshRenderer>();
        oPolygon.GetComponent<MeshFilter>().mesh = oMesh;
        oPolygon.GetComponent<Renderer>().material.color = useColor;
    }
    
    //求解中点
    public static Vector3 midPoint(Vector3 oStartPoint, Vector3 oEndPoint)
    {
        return (oStartPoint + oEndPoint) / 2;
    }
    //返回科目顶点0，7的中点
    public static Vector3 initPoint(List<Vector3> listVerts)
    {
        return midPoint(listVerts[(int)RcVertIndex.VertIndex0], listVerts[(int)RcVertIndex.VertIndex7]);
    }

    public static float intersectionRadLinePlane(Vector3 oVectorLine, Vector3 oVectorPlane)
    {
        double res = MultiVec3(oVectorLine, oVectorPlane);
        return (float)Math.Asin(res / (Length(oVectorLine) * Length(oVectorPlane)));
    }

    public static double MultiVec3(Vector3 vec1,Vector3 vec2)
    {
        return (double)(vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z);

    }
    public static float Length(Vector3 vec3)
    {
        return (float)(Math.Sqrt(vec3.x * vec3.x + vec3.y * vec3.y + vec3.z * vec3.z));
    }

    public static float Distance(Vector3 vec1,Vector3 vec2)
    {
        //return (float)(Math.Sqrt(vec1.x*vec2.x+vec1.y*vec2.y+vec1.z*vec2.z));
        return (float)(Math.Sqrt( (vec2.x- vec1.x) * (vec2.x - vec1.x) +  (vec2.y- vec1.y)* (vec2.y - vec1.y) +  (vec2.z- vec1.z)* (vec2.z - vec1.z)));
    }

    public static Vector3 SubVec3(Vector3 begin, Vector3 end)
    {
        return new Vector3(end.x-begin.x,end.y-begin.y,end.z-begin.z);
    }
}