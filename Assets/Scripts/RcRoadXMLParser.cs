using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParkPoint
{
    public int index;
    public string direct;
    public float x, y, z;
}
public class SPWall
{
    public List<ParkPoint> StartPoints = new List<ParkPoint>();
    public List<ParkPoint> ForeArcPoints = new List<ParkPoint>();
    public List<ParkPoint> TopPoint = new List<ParkPoint>();
    public List<ParkPoint> BackArcPoint = new List<ParkPoint>();
    public List<ParkPoint> EndPoint = new List<ParkPoint>();
}

public class SubSPArea
{
    public string m_subspName;
    public List<ParkPoint> LeftLine = new List<ParkPoint>();
    public List<ParkPoint> RightLine = new List<ParkPoint>();
    public List<ParkPoint> ControlLine1 = new List<ParkPoint>();
    public List<ParkPoint> ControlLine2 = new List<ParkPoint>();
    public List<ParkPoint> ControlLine3 = new List<ParkPoint>();
    public SPWall m_wall;
}
public class SPArea
{
    ////坡道名
    //public string m_SPName;
    //大坡左墙右墙
    public SPWall m_LeftWall;
    public SPWall m_RightWall;
    //子坡道
    public List<SubSPArea> m_subSPArea = new List<SubSPArea>();
}
public class RcRoadXMLParser {

    //倒车入库顶点序列
    private static Dictionary<string, List<Vector3>> m_dicRePark = new Dictionary<string, List<Vector3>>();
    public static Dictionary<string, List<Vector3>> reverseParkingList()
    {
        return RcRoadXMLParser.m_dicRePark;
    }
    //侧方停车顶点序列
    private static Dictionary<string, List<Vector3>> m_dicPalPark = new Dictionary<string, List<Vector3>>();
    public static Dictionary<string, List<Vector3>> parallelParkingList()
    {
        return RcRoadXMLParser.m_dicPalPark;
    }
    //直角转弯顶点序列
    private static Dictionary<string, List<Vector3>> m_dicQuarTurn = new Dictionary<string, List<Vector3>>();
    public static Dictionary<string, List<Vector3>> quarTurnList()
    {
        return RcRoadXMLParser.m_dicQuarTurn;
    }

    //曲线行驶顶点序列
    private static Dictionary<string, Dictionary<string, List<Vector3>>> m_dicCurveDriving = new Dictionary<string, Dictionary<string, List<Vector3>>>();
    public static Dictionary<string, Dictionary<string, List<Vector3>>> curveDrivingList()
    {
        return RcRoadXMLParser.m_dicCurveDriving;
    }

    //坡道顶点停车顶点序列
    private static Dictionary<string, SPArea> m_dicSlopParking = new Dictionary<string, SPArea>();
    public static Dictionary<string, SPArea> slopParkingList()
    {
        return RcRoadXMLParser.m_dicSlopParking;
    }

    //返回顶点序列
    public static List<Vector3> subjectVerts(string strName)
    {
        List<Vector3> listVerts = new List<Vector3>();
        if (strName.IndexOf("RP") > -1)
        {
            listVerts = m_dicRePark[strName];
        }
        else if (strName.IndexOf("PP") > -1)
        {
            listVerts = m_dicRePark[strName];
        }
        else
        {
            Debug.Log("error");
        }
        return listVerts;
    }

    public void loadXML()
    {
        //加载XML文件
        XmlDocument oXmlDoc = new XmlDocument();
        XmlReaderSettings oXmlSet = new XmlReaderSettings();
        //忽略注释影响
        oXmlSet.IgnoreComments = true;
        oXmlDoc.Load(XmlReader.Create((Application.streamingAssetsPath + "/Map_Field.xml"), oXmlSet));

        //得到Site节点下的所有子节点
        XmlNodeList xmlNodeList = oXmlDoc.SelectSingleNode("Site").ChildNodes;
        foreach (XmlElement oElementI in xmlNodeList)
        {
            if (oElementI.Name == "ReverseParkingNode")
            {
                foreach (XmlElement oElementII in oElementI.ChildNodes)
                {
                    string strCode = oElementII.GetAttribute("CODE");
                    List<Vector3> oList = new List<Vector3>();
                    foreach (XmlElement oElementIII in oElementII.ChildNodes)
                    {
                        foreach (XmlElement oElementIV in oElementIII)
                        {
                            float fX = float.Parse(oElementIV.GetAttribute("x"));
                            float fY = float.Parse(oElementIV.GetAttribute("y"));
                            float fZ = float.Parse(oElementIV.GetAttribute("z"));
                            //地图文件采用右手坐标系，unity为左手坐标系，x->-x
                            //Vector3 oVec3 = new Vector3(-fX, fY, fZ + RcGlobal.RcIncreaseZ);
                            //地图文件采用右手坐标系，unity为左手坐标系，x->- x, y->-z,z->y; 
                            Vector3 oVec3 = new Vector3(-fX, fZ + RcGlobal.RcIncreaseZ, -fY);
                            oList.Add(oVec3);
                        }
                    }
                    m_dicRePark.Add(strCode, oList);
                }
            }
            else if (oElementI.Name == "ParallelParkingNode")
            {
                foreach (XmlElement oElementII in oElementI.ChildNodes)
                {
                    string strCode = oElementII.GetAttribute("CODE");
                    List<Vector3> oList = new List<Vector3>();
                    foreach (XmlElement oElementIII in oElementII.ChildNodes)
                    {
                        foreach (XmlElement oElementIV in oElementIII)
                        {
                            float fX = float.Parse(oElementIV.GetAttribute("x"));
                            float fY = float.Parse(oElementIV.GetAttribute("y"));
                            float fZ = float.Parse(oElementIV.GetAttribute("z"));
                            //Vector3 oVec3 = new Vector3(-fX, fY, fZ + RcGlobal.RcIncreaseZ);
                            //地图文件采用右手坐标系，unity为左手坐标系，x->- x, y->-z,z->y; 
                            Vector3 oVec3 = new Vector3(-fX, fZ + RcGlobal.RcIncreaseZ, -fY);
                            oList.Add(oVec3);
                        }
                    }
                    m_dicPalPark.Add(strCode, oList);
                }
            }
            else if (oElementI.Name == "QuarterTurnNode")
            {
                foreach (XmlElement oElementII in oElementI.ChildNodes)
                {
                    string strCode = oElementII.GetAttribute("CODE");
                    List<Vector3> oList = new List<Vector3>();
                    foreach (XmlElement oElementIII in oElementII.ChildNodes)
                    {
                        foreach (XmlElement oElementIV in oElementIII)
                        {
                            float fX = float.Parse(oElementIV.GetAttribute("x"));
                            float fY = float.Parse(oElementIV.GetAttribute("y"));
                            float fZ = float.Parse(oElementIV.GetAttribute("z"));
                            Vector3 oVec3 = new Vector3(-fX, fZ + RcGlobal.RcIncreaseZ, -fY);
                            oList.Add(oVec3);
                        }
                    }
                    m_dicQuarTurn.Add(strCode, oList);
                }
            }
            else if (oElementI.Name == "CurveDrivingNode")
            {
                foreach (XmlElement oElementII in oElementI.ChildNodes)
                {
                    string strCode = oElementII.GetAttribute("CODE");
                    Dictionary<string, List<Vector3>> partsDic = new Dictionary<string, List<Vector3>>();
                    m_dicCurveDriving.Add(strCode, partsDic);
                    foreach (XmlElement oElementIII in oElementII.ChildNodes)
                    {
                        List<Vector3> oList = new List<Vector3>();
                        if (oElementIII.GetAttribute("Type").ToString().Equals("LeftLine"))
                        {
                            oList.Clear();
                            foreach (XmlElement oElementIV in oElementIII)
                            {
                                float fX = float.Parse(oElementIV.GetAttribute("x"));
                                float fY = float.Parse(oElementIV.GetAttribute("y"));
                                float fZ = float.Parse(oElementIV.GetAttribute("z"));
                                Vector3 oVec3 = new Vector3(-fX, fZ + RcGlobal.RcIncreaseZ, -fY);
                                oList.Add(oVec3);
                            }
                            m_dicCurveDriving[strCode].Add("LeftLine", oList);

                        }
                        else if (oElementIII.GetAttribute("Type").ToString().Equals("RightLine"))
                        {
                            oList.Clear();
                            foreach (XmlElement oElementIV in oElementIII)
                            {
                                float fX = float.Parse(oElementIV.GetAttribute("x"));
                                float fY = float.Parse(oElementIV.GetAttribute("y"));
                                float fZ = float.Parse(oElementIV.GetAttribute("z"));
                                Vector3 oVec3 = new Vector3(-fX, fZ + RcGlobal.RcIncreaseZ, -fY);
                                oList.Add(oVec3);
                            }
                            m_dicCurveDriving[strCode].Add("RightLine", oList);
                        }
                    }
                }
            }
            else if (oElementI.Name == "SlopeParkingNode")
            {
                foreach (XmlElement oElementII in oElementI.ChildNodes)
                {
                    SPArea spNode = new SPArea();
                    string strCode = oElementII.GetAttribute("CODE");
                    foreach (XmlElement oElementIII in oElementII.ChildNodes)
                    {
                        if (oElementIII.Name.Equals("PointType"))
                        {
                            SPWall wall = new SPWall();
                            if (oElementIII.GetAttribute("Type").Equals("LeftWall"))
                            {
                                foreach (XmlElement oElementIV in oElementIII.ChildNodes)
                                {
                                    if (oElementIV.GetAttribute("Type").Equals("StartPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.StartPoints);
                                        }
                                    }
                                    else if (oElementIV.GetAttribute("Type").Equals("ForeArcPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.ForeArcPoints);
                                        }
                                    }
                                    else if (oElementIV.GetAttribute("Type").Equals("TopPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.TopPoint);
                                        }
                                    }
                                    else if (oElementIV.GetAttribute("Type").Equals("BackArcPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.BackArcPoint);
                                        }
                                    }
                                    else if (oElementIV.GetAttribute("Type").Equals("EndPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.EndPoint);
                                        }
                                    }
                                    spNode.m_LeftWall = wall;
                                }
                            }
                            else if (oElementIII.GetAttribute("Type").Equals("RightWall"))
                            {
                                foreach (XmlElement oElementIV in oElementIII.ChildNodes)
                                {
                                    if (oElementIV.GetAttribute("Type").Equals("StartPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.StartPoints);
                                        }
                                    }
                                    else if (oElementIV.GetAttribute("Type").Equals("ForeArcPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.ForeArcPoints);
                                        }
                                    }
                                    else if (oElementIV.GetAttribute("Type").Equals("TopPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.TopPoint);
                                        }
                                    }
                                    else if (oElementIV.GetAttribute("Type").Equals("BackArcPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.BackArcPoint);
                                        }
                                    }
                                    else if (oElementIV.GetAttribute("Type").Equals("EndPoint"))
                                    {
                                        foreach (XmlElement oElementV in oElementIV)
                                        {
                                            parseToAdd(oElementV, ref wall.EndPoint);
                                        }
                                    }
                                    spNode.m_RightWall = wall;
                                }
                            }
                        }
                        else if (oElementIII.Name.Equals("Slope"))
                        {
                            //子坡道
                            SubSPArea tempSubSpArea = new SubSPArea();
                            tempSubSpArea.m_subspName = oElementIII.GetAttribute("CODE");
                            foreach (XmlElement oElementIV in oElementIII)
                            {
                                if (oElementIV.GetAttribute("Type").Equals("LeftLine"))
                                {
                                    foreach (XmlElement oElementV in oElementIV)
                                    {
                                        parseToAdd(oElementV, ref tempSubSpArea.LeftLine);
                                    }
                                }
                                else if (oElementIV.GetAttribute("Type").Equals("RightLine"))
                                {
                                    foreach (XmlElement oElementV in oElementIV)
                                    {
                                        parseToAdd(oElementV, ref tempSubSpArea.RightLine);
                                    }
                                }
                                else if (oElementIV.GetAttribute("Type").Equals("ControlLine1"))
                                {
                                    foreach (XmlElement oElementV in oElementIV)
                                    {
                                        parseToAdd(oElementV, ref tempSubSpArea.ControlLine1);
                                    }
                                }
                                else if (oElementIV.GetAttribute("Type").Equals("ControlLine2"))
                                {
                                    foreach (XmlElement oElementV in oElementIV)
                                    {
                                        parseToAdd(oElementV, ref tempSubSpArea.ControlLine2);
                                    }
                                }
                                else if (oElementIV.GetAttribute("Type").Equals("ControlLine3"))
                                {
                                    foreach (XmlElement oElementV in oElementIV)
                                    {
                                        parseToAdd(oElementV, ref tempSubSpArea.ControlLine3);
                                    }
                                }
                            }

                            spNode.m_subSPArea.Add(tempSubSpArea);
                        }
                    }
                    m_dicSlopParking.Add(strCode, spNode);
                }
            }
        }
        Debug.Log("Parse XML File Already!");
    }

    private void parseToAdd(XmlElement oElementV, ref List<ParkPoint> outList)
    {
        //Vector3 oVec3 = new Vector3(-fX, fZ + RcGlobal.RcIncreaseZ, -fY);
        ParkPoint tempNode = new ParkPoint();
        tempNode.index = int.Parse(oElementV.GetAttribute("Index"));
        tempNode.direct = oElementV.GetAttribute("Direc");
        tempNode.x = -float.Parse(oElementV.GetAttribute("x"));
        tempNode.y = float.Parse(oElementV.GetAttribute("z")) + RcGlobal.RcIncreaseZ;
        tempNode.z = -float.Parse(oElementV.GetAttribute("y"));
        outList.Add(tempNode);
    }

    public void createRoadScence()
    {
        Dictionary<string, List<Vector3>> dicReverseParking = RcRoadXMLParser.reverseParkingList();
        foreach (var itemNode in dicReverseParking)
        {
            //绘制每一个倒车入库
            createReverseParking(itemNode.Key, itemNode.Value);
        }

        //生成侧方停车边界线
        Dictionary<string, List<Vector3>> dicParallelParking = RcRoadXMLParser.parallelParkingList();
        foreach (var itemNode in dicParallelParking)
        {
            createParallelParking(itemNode.Key, itemNode.Value);
        }

        //生成直角转弯库位线
        Dictionary<string, List<Vector3>> dicQuarTurn = RcRoadXMLParser.quarTurnList();
        foreach (var itemNode in dicQuarTurn)
        {
            createQuarTurn(itemNode.Key, itemNode.Value);
        }

        //生成曲线行驶库位线
        Dictionary<string, Dictionary<string, List<Vector3>>> dicCurveDriving = RcRoadXMLParser.curveDrivingList();
        foreach (var itemNode in dicCurveDriving)
        {
            createCurveDriving(itemNode.Key, itemNode.Value);
        }

        Dictionary<string, SPArea> dicSlopParking = RcRoadXMLParser.slopParkingList();
        foreach (var itemNode in dicSlopParking)
        {
            createSlopParking(itemNode.Key, itemNode.Value);
        }
    }

    //倒车入库边库位线
    public void createReverseParking(string code, List<Vector3> listVec3)
    {
        //生成左边线
        List<Vector3> listVertsTemp = new List<Vector3>();
        for (int i = 0; i < RcGlobal.RcParkLeftSize; i++)
        {
            listVertsTemp.Add(listVec3[i]);
        }
        List<Vector3> listVertsPolygon = new List<Vector3>();
        RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, RcGlobal.RcRoadWidth);
        for (int i = listVertsTemp.Count - 1; i >= 0; i--)
        {
            listVertsPolygon.Add(listVertsTemp[i]);
        }
        Vector3[] oVertsQuad = listVertsPolygon.ToArray();
        RcGeometry.createQuad(oVertsQuad, Color.white, code + "_Left");
        //生成右边线
        for (int i = RcGlobal.RcParkLeftSize; i < listVec3.Count - 1; i++)
        {
            listVertsTemp.Clear();
            if (i == (int)RcVertIndex.VertIndex3)
            {
                listVertsTemp.Add(listVec3[i]);
                Vector3 oVector = listVec3[i + 1] - listVec3[i];
                oVector.Normalize();
                listVertsTemp.Add(listVec3[i + 1] + oVector * RcGlobal.RcRoadWidth);
            }
            else if (i == (int)RcVertIndex.VertIndex4)
            {
                Vector3 oVector = listVec3[i + 1] - listVec3[i];
                oVector.Normalize();
                listVertsTemp.Add(listVec3[i] - oVector * RcGlobal.RcRoadWidth);
                listVertsTemp.Add(listVec3[i + 1] + oVector * RcGlobal.RcRoadWidth);
            }
            else
            {
                listVertsTemp.Add(listVec3[i]);
                listVertsTemp.Add(listVec3[i + 1]);
            }
            RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, -RcGlobal.RcRoadWidth);
            for (int j = listVertsPolygon.Count - 1; j >= 0; j--)
            {
                listVertsTemp.Add(listVertsPolygon[j]);
            }
            oVertsQuad = listVertsTemp.ToArray();
            RcGeometry.createQuad(oVertsQuad, Color.white, code + "_Right" + i.ToString());
        }

        //黄线 1-2  7-0
        List<Vector3> outPts = new List<Vector3>();
        RcGeometry.splitLine(listVec3[0], listVec3[7], ref outPts, RcGlobal.RcRPSplitSegCount);
        for (int segIndex = 0; segIndex <= outPts.Count - 2; segIndex += 2)
        {
            listVertsTemp.Clear(); listVertsPolygon.Clear();
            listVertsTemp.Add(outPts[segIndex]);
            listVertsTemp.Add(outPts[segIndex + 1]);

            RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, RcGlobal.RcRoadWidth);
            for (int j = listVertsTemp.Count - 1; j >= 0; j--)
            {
                listVertsPolygon.Add(listVertsTemp[j]);
            }
            oVertsQuad = listVertsPolygon.ToArray();
            RcGeometry.createQuad(oVertsQuad, Color.yellow, code + "_Yellow_Seg");
        }


        outPts.Clear();
        RcGeometry.splitLine(listVec3[1], listVec3[2], ref outPts, RcGlobal.RcRPSplitSegCount);
        for (int segIndex = 0; segIndex < outPts.Count; segIndex += 2)
        {
            listVertsTemp.Clear(); listVertsPolygon.Clear();
            listVertsTemp.Add(outPts[segIndex]);
            listVertsTemp.Add(outPts[segIndex + 1]);

            RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, RcGlobal.RcRoadWidth);
            for (int j = listVertsTemp.Count - 1; j >= 0; j--)
            {
                listVertsPolygon.Add(listVertsTemp[j]);
            }
            oVertsQuad = listVertsPolygon.ToArray();
            RcGeometry.createQuad(oVertsQuad, Color.yellow, code + "_Yellow_Seg");
        }
    }

    //生成侧方停车边界线
    public void createParallelParking(string code, List<Vector3> listVec3)
    {
        //生成左边线
        List<Vector3> listVertsTemp = new List<Vector3>();
        for (int i = 0; i < RcGlobal.RcParkLeftSize; i++)
        {
            listVertsTemp.Add(listVec3[i]);
        }
        List<Vector3> listVertsPolygon = new List<Vector3>();
        RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, RcGlobal.RcRoadWidth);
        for (int i = listVertsTemp.Count - 1; i >= 0; i--)
        {
            listVertsPolygon.Add(listVertsTemp[i]);
        }
        Vector3[] oVertsQuad = listVertsPolygon.ToArray();
        RcGeometry.createQuad(oVertsQuad, Color.white, code + "_Left");
        //生成右边线
        for (int i = RcGlobal.RcParkLeftSize; i < listVec3.Count - 1; i++)
        {
            listVertsTemp.Clear();
            if (i == (int)RcVertIndex.VertIndex3)
            {
                listVertsTemp.Add(listVec3[i]);
                Vector3 oVector = listVec3[i + 1] - listVec3[i];
                oVector.Normalize();
                listVertsTemp.Add(listVec3[i + 1] + oVector * RcGlobal.RcRoadWidth);
            }
            else if (i == (int)RcVertIndex.VertIndex4)
            {
                Vector3 oVector = listVec3[i + 1] - listVec3[i];
                oVector.Normalize();
                listVertsTemp.Add(listVec3[i] - oVector * RcGlobal.RcRoadWidth);
                listVertsTemp.Add(listVec3[i + 1] + oVector * RcGlobal.RcRoadWidth);
            }
            else
            {
                listVertsTemp.Add(listVec3[i]);
                listVertsTemp.Add(listVec3[i + 1]);
            }
            RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, -RcGlobal.RcRoadWidth);
            for (int j = listVertsPolygon.Count - 1; j >= 0; j--)
            {
                listVertsTemp.Add(listVertsPolygon[j]);
            }
            oVertsQuad = listVertsTemp.ToArray();
            RcGeometry.createQuad(oVertsQuad, Color.white, code + "_Right");
        }

        //黄色虚线
        List<Vector3> outPts = new List<Vector3>();
        RcGeometry.splitLine(listVec3[0], listVec3[7], ref outPts, RcGlobal.RcPPSplitSegCount1);
        for (int segIndex = 0; segIndex < outPts.Count; segIndex += 2)
        {
            listVertsTemp.Clear(); listVertsPolygon.Clear();
            listVertsTemp.Add(outPts[segIndex]);
            listVertsTemp.Add(outPts[segIndex + 1]);

            RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, RcGlobal.RcRoadWidth);
            for (int j = listVertsTemp.Count - 1; j >= 0; j--)
            {
                listVertsPolygon.Add(listVertsTemp[j]);
            }
            oVertsQuad = listVertsPolygon.ToArray();
            RcGeometry.createQuad(oVertsQuad, Color.yellow, code + "_Yellow_Seg");
        }

        outPts.Clear();
        RcGeometry.splitLine(listVec3[1], listVec3[2], ref outPts, RcGlobal.RcPPSplitSegCount1);
        for (int segIndex = 0; segIndex < outPts.Count; segIndex += 2)
        {
            listVertsTemp.Clear(); listVertsPolygon.Clear();
            listVertsTemp.Add(outPts[segIndex]);
            listVertsTemp.Add(outPts[segIndex + 1]);

            RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, RcGlobal.RcRoadWidth);
            for (int j = listVertsTemp.Count - 1; j >= 0; j--)
            {
                listVertsPolygon.Add(listVertsTemp[j]);
            }
            oVertsQuad = listVertsPolygon.ToArray();
            RcGeometry.createQuad(oVertsQuad, Color.yellow, code + "_Yellow_Seg");
        }

        outPts.Clear();
        RcGeometry.splitLine(listVec3[3], listVec3[6], ref outPts, RcGlobal.RcPPSplitSegCount2);
        for (int segIndex = 0; segIndex < outPts.Count; segIndex += 2)
        {
            listVertsTemp.Clear(); listVertsPolygon.Clear();
            listVertsTemp.Add(outPts[segIndex]);
            listVertsTemp.Add(outPts[segIndex + 1]);

            RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, -RcGlobal.RcRoadWidth);
            for (int j = listVertsPolygon.Count - 1; j >= 0; j--)
            {
                listVertsTemp.Add(listVertsPolygon[j]);
            }
            oVertsQuad = listVertsTemp.ToArray();
            RcGeometry.createQuad(oVertsQuad, Color.yellow, code + "_Yellow_Seg222");
        }
    }

    public void createQuarTurn(string code, List<Vector3> listVec3)
    {
        List<Vector3> listVertsTemp = new List<Vector3>();
        //生成左边线
        for (int i = 0; i < RcGlobal.RcQuarterLeftSize - 1; i++)
        {
            listVertsTemp.Clear();
            listVertsTemp.Add(listVec3[i]);
            listVertsTemp.Add(listVec3[i + 1]);

            List<Vector3> listVertsPolygon = new List<Vector3>();
            RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, -RcGlobal.RcRoadWidth);
            for (int j = listVertsPolygon.Count - 1; j >= 0; j--)
            {
                listVertsTemp.Add(listVertsPolygon[j]);
            }
            Vector3[] oVertsQuad = listVertsTemp.ToArray();
            RcGeometry.createQuad(oVertsQuad, Color.white, code + "_Left" + i.ToString());
        }

        //生成右边线
        for (int i = RcGlobal.RcQuarterLeftSize; i < listVec3.Count - 1; i++)
        {
            listVertsTemp.Clear();
            listVertsTemp.Add(listVec3[i]);
            listVertsTemp.Add(listVec3[i + 1]);

            List<Vector3> listVertsPolygon = new List<Vector3>();
            RcGeometry.createPolygonVertexs(listVertsTemp, ref listVertsPolygon, RcGlobal.RcRoadWidth);
            for (int j = listVertsTemp.Count - 1; j >= 0; j--)
            {
                listVertsPolygon.Add(listVertsTemp[j]);
            }
            Vector3[] oVertsQuad = listVertsPolygon.ToArray();
            RcGeometry.createQuad(oVertsQuad, Color.white, code + "_Left" + i.ToString());
        }
    }

    public void createCurveDriving(string code, Dictionary<string, List<Vector3>> dataVecList)
    {
        //左侧边线
        List<Vector3> leftLine = dataVecList["LeftLine"];
        List<Vector3> listVertsPolygon = new List<Vector3>();
        RcGeometry.createPolygonVertexs(leftLine, ref listVertsPolygon, -RcGlobal.RcRoadWidth);

        List<Vector3> tempListVerts = new List<Vector3>();
        RcGeometry.ConvertForCD(listVertsPolygon, ref tempListVerts);

        Vector3[] oVertsLeft1 = leftLine.ToArray();
        Vector3[] oVertsLeft2 = tempListVerts.ToArray();
        RcGeometry.createPoly(oVertsLeft2, oVertsLeft1, Color.white, code + "_LeftLine");

        //右侧边线
        List<Vector3> rightLine = dataVecList["RightLine"];
        listVertsPolygon.Clear();
        RcGeometry.createPolygonVertexs(rightLine, ref listVertsPolygon, RcGlobal.RcRoadWidth);

        tempListVerts.Clear();
        RcGeometry.ConvertForCD(listVertsPolygon, ref tempListVerts);

        Vector3[] oVertsRight1 = rightLine.ToArray();
        Vector3[] oVertsRight2 = tempListVerts.ToArray();
        RcGeometry.createPoly(oVertsRight1, oVertsRight2, Color.white, code + "_RightLine");
    }

    private void createSlopParking(string code, SPArea item)
    {
        List<Vector3> leftWall = new List<Vector3>();
        List<Vector3> rightWall = new List<Vector3>();
        buildSlopParkingWallData(item, ref leftWall, ref rightWall);

        Vector3 oVectorLine = leftWall[1] - leftWall[0];
        Vector3 oVectorPlane = new Vector3(0, 0, 1);
        float fInterRad = RcGeometry.intersectionRadLinePlane(oVectorLine, oVectorPlane);
        float fHeightRef = (float)(RcGlobal.RcRoadWidth * Math.Sin(fInterRad));

        foreach (var subspItem in item.m_subSPArea)
        {
            List<Vector3> linePoints = new List<Vector3>();
            List<Vector3> linePoints2 = new List<Vector3>();
            List<Vector3> tempListVerts = new List<Vector3>();
            //控制线1
            {
                linePoints.Add(new Vector3(subspItem.ControlLine1[0].x, subspItem.ControlLine1[0].y, subspItem.ControlLine1[0].z));
                linePoints.Add(new Vector3(subspItem.ControlLine1[1].x, subspItem.ControlLine1[1].y, subspItem.ControlLine1[1].z));

                RcGeometry.createPolygonVertexs(linePoints, ref linePoints2, -RcGlobal.RcRoadWidth);
                RcGeometry.ConvertForCD(linePoints2, ref tempListVerts);

                Vector3[] oVertsLeft1 = linePoints.ToArray();
                Vector3[] oVertsLeft2 = tempListVerts.ToArray();
                RcGeometry.createPoly(oVertsLeft2, oVertsLeft1, Color.white, code + "_CtrlLine1");
            }

            //控制线2
            {
                linePoints.Clear(); linePoints2.Clear(); tempListVerts.Clear();

                linePoints.Add(new Vector3(subspItem.ControlLine2[0].x, subspItem.ControlLine2[0].y, subspItem.ControlLine2[0].z));
                linePoints.Add(new Vector3(subspItem.ControlLine2[1].x, subspItem.ControlLine2[1].y, subspItem.ControlLine2[1].z));

                RcGeometry.createPolygonVertexs(linePoints, ref linePoints2, -2.0f * RcGlobal.RcRoadWidth);
                RcGeometry.ConvertForCD(linePoints2, ref tempListVerts);

                Vector3[] oVertsLeft1 = linePoints.ToArray();
                Vector3[] oVertsLeft2 = tempListVerts.ToArray();
                RcGeometry.createPoly(oVertsLeft2, oVertsLeft1, Color.yellow, code + "_CtrlLine2");
            }

            //控制线3
            {
                linePoints.Clear(); linePoints2.Clear(); tempListVerts.Clear();

                linePoints.Add(new Vector3(subspItem.ControlLine3[0].x, subspItem.ControlLine3[0].y, subspItem.ControlLine3[0].z));
                linePoints.Add(new Vector3(subspItem.ControlLine3[1].x, subspItem.ControlLine3[1].y, subspItem.ControlLine3[1].z));

                RcGeometry.createPolygonVertexs(linePoints, ref linePoints2, -RcGlobal.RcRoadWidth);
                RcGeometry.ConvertForCD(linePoints2, ref tempListVerts);

                Vector3[] oVertsLeft1 = linePoints.ToArray();
                Vector3[] oVertsLeft2 = tempListVerts.ToArray();
                RcGeometry.createPoly(oVertsLeft2, oVertsLeft1, Color.white, code + "_CtrlLine3");
            }

            //左线
            {
                linePoints.Clear(); linePoints2.Clear(); tempListVerts.Clear();

                linePoints.Add(new Vector3(subspItem.LeftLine[0].x, subspItem.LeftLine[0].y, subspItem.LeftLine[0].z));
                linePoints.Add(new Vector3(subspItem.LeftLine[1].x, subspItem.LeftLine[1].y, subspItem.LeftLine[1].z));

                RcGeometry.createPolygonVertexs(linePoints, ref linePoints2, -RcGlobal.RcRoadWidth);
                RcGeometry.ConvertForCD(linePoints2, ref tempListVerts);

                Vector3[] oVertsLeft1 = linePoints.ToArray();
                Vector3[] oVertsLeft2 = tempListVerts.ToArray();
                RcGeometry.createPoly(oVertsLeft2, oVertsLeft1, Color.white, code + "_LeftLine");
            }

            //右线
            {
                linePoints.Clear(); linePoints2.Clear(); tempListVerts.Clear();

                linePoints.Add(new Vector3(subspItem.RightLine[0].x, subspItem.RightLine[0].y, subspItem.RightLine[0].z));
                linePoints.Add(new Vector3(subspItem.RightLine[1].x, subspItem.RightLine[1].y, subspItem.RightLine[1].z));

                RcGeometry.createPolygonVertexs(linePoints, ref linePoints2, -RcGlobal.RcRoadWidth);
                RcGeometry.ConvertForCD(linePoints2, ref tempListVerts);

                Vector3[] oVertsLeft1 = linePoints.ToArray();
                Vector3[] oVertsLeft2 = tempListVerts.ToArray();
                RcGeometry.createPoly(oVertsLeft2, oVertsLeft1, Color.white, code + "_RightLine");
            }
        }
    }

    private void buildSlopParkingWallData(SPArea item, ref List<Vector3> outLeftList, ref List<Vector3> outRightList)
    {
        //左墙
        outLeftList.Add(new Vector3(item.m_LeftWall.StartPoints[0].x, item.m_LeftWall.StartPoints[0].y, item.m_LeftWall.StartPoints[0].z));
        outLeftList.Add(new Vector3(item.m_LeftWall.TopPoint[0].x, item.m_LeftWall.StartPoints[0].y, item.m_LeftWall.TopPoint[0].z));
        outLeftList.Add(new Vector3(item.m_LeftWall.EndPoint[0].x, item.m_LeftWall.StartPoints[0].y, item.m_LeftWall.EndPoint[0].z));

        //右墙
        outRightList.Add(new Vector3(item.m_RightWall.StartPoints[0].x, item.m_RightWall.StartPoints[0].y, item.m_RightWall.StartPoints[0].z));
        outRightList.Add(new Vector3(item.m_RightWall.TopPoint[0].x, item.m_RightWall.StartPoints[0].y, item.m_RightWall.TopPoint[0].z));
        outRightList.Add(new Vector3(item.m_RightWall.EndPoint[0].x, item.m_RightWall.StartPoints[0].y, item.m_RightWall.EndPoint[0].z));
    }

}
