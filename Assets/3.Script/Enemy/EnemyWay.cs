using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Node
{
    public bool isWall;
    public Node ParentNode;
    public int X, Y;
    public int G;
    public int H;

    public int F
    {
        get
        {
            return G + H;
        }
    }

    public Node(bool iswall, int x, int y)
    {
        isWall = iswall;
            X = x;
        Y = y;
    }
}
public class EnemyWay : MonoBehaviour
{
    public GameObject StartPos_ob, EndPos_ob;
    public GameObject BottomLeft_ob, TopRight_ob;

    public Vector2Int bottomLeft, topRight, startPos, endPos;
    public List<Node> Complete_Node;
    public bool AllowDiagonal = true;
    public bool DontCrossCorner = true;

    private int SizeX, SizeY;
    private Node[,] nodeArray;
    private Node Startnode, EndNode, CurNode;
    private List<Node> OpenList, CloseList;

    [Header("경로보기")]
    [SerializeField] private LineRenderer lineRenderer; //라인렌더러가 또 있음 (별게 다 있음)
    [SerializeField] private MapData mapData;
    [SerializeField] private float showScreen = 2f;
    [SerializeField] private GameObject showAsset;

    private void SetPosition()
    {
        bottomLeft = new Vector2Int((int)BottomLeft_ob.transform.position.x, (int)BottomLeft_ob.transform.position.y);
        topRight = new Vector2Int((int)TopRight_ob.transform.position.x, (int)TopRight_ob.transform.position.y);
        startPos = new Vector2Int((int)StartPos_ob.transform.position.x, (int)StartPos_ob.transform.position.y);
        endPos = new Vector2Int((int)EndPos_ob.transform.position.x, (int)EndPos_ob.transform.position.y);
    }
    public void PathFinding() //버튼 말고 키보드 클릭으로 해야 하긴 함
    {
        SetPosition();
        SizeX = topRight.x - bottomLeft.x + 1;
        SizeY = topRight.y - bottomLeft.y + 1;


        nodeArray = new Node[SizeX, SizeY];
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {

                //bool iswall = false;
                //foreach(Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i+bottomLeft.x, j+bottomLeft.y),0.4f))
                //{
                //    if(col.gameObject.layer.Equals(LayerMask.NameToLayer("Wall")))
                //    {
                //        iswall = true;
                //    }
                //}
                //nodeArray[i, j] = new Node(iswall, i + bottomLeft.x, j + bottomLeft.y);

                Vector3Int cellPos = new Vector3Int(i + bottomLeft.x, j + bottomLeft.y, 0);
                bool iswall = !CheckTile(cellPos);
                nodeArray[i, j] = new Node(iswall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }
        Startnode = nodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        EndNode = nodeArray[endPos.x - bottomLeft.x, endPos.y - bottomLeft.y];

        OpenList = new List<Node>();
        CloseList = new List<Node>();
        Complete_Node = new List<Node>();

        OpenList.Add(Startnode);

        while (OpenList.Count > 0)
        {
            //CurNode = OpenList[0];
            //for(int i = 0; i<OpenList.Count; i++)
            //{
            //    if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
            //    {
            //        CurNode = OpenList[i];
            //    }
            //    OpenList.Remove(CurNode);
            //    CloseList.Add(CurNode);
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].F < CurNode.F || (OpenList[i].F == CurNode.F && OpenList[i].H < CurNode.H))
                {
                    CurNode = OpenList[i];
                }
            }

            OpenList.Remove(CurNode);
            CloseList.Add(CurNode);

           

            if (CurNode == EndNode)
            {


                Node targetnode = EndNode;
                while (targetnode != Startnode)
                {
                    Complete_Node.Add(targetnode);
                    targetnode = targetnode.ParentNode;
                }
                Complete_Node.Add(Startnode);
                Complete_Node.Reverse();
                return;

            }
            if (AllowDiagonal)
            {


                openListAdd(CurNode.X + 1, CurNode.Y + 1);
                openListAdd(CurNode.X - 1, CurNode.Y - 1);
                openListAdd(CurNode.X + 1, CurNode.Y - 1);
                openListAdd(CurNode.X - 1, CurNode.Y + 1);

            }

            openListAdd(CurNode.X + 1, CurNode.Y);
            openListAdd(CurNode.X - 1, CurNode.Y);
            openListAdd(CurNode.X, CurNode.Y + 1);
            openListAdd(CurNode.X, CurNode.Y - 1);

        }
    }
    private void openListAdd(int checkX, int checkY)
    {
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 &&
            !nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall
            && !CloseList.Contains(nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            if (AllowDiagonal)
            {
                if (nodeArray[CurNode.X - bottomLeft.x, checkY - bottomLeft.y].isWall &&
                    nodeArray[checkX - bottomLeft.x, CurNode.Y - bottomLeft.y].isWall)
                {
                    return;
                }
            }
            if (DontCrossCorner)
            {
                if (nodeArray[CurNode.X - bottomLeft.x, checkY - bottomLeft.y].isWall &&
                    nodeArray[checkX - bottomLeft.x, CurNode.Y - bottomLeft.y].isWall)
                {
                    return;
                }
            }
            Node neighbor_node = nodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int moveCost = CurNode.G + (CurNode.X - checkX == 0 || CurNode.Y - checkY == 0 ? 10 : 14);

            if (moveCost < neighbor_node.G || !OpenList.Contains(neighbor_node))
            {
                neighbor_node.G = moveCost;
                neighbor_node.H = (Mathf.Abs(neighbor_node.X - EndNode.X) + Math.Abs(neighbor_node.Y - EndNode.Y)) * 10;

                neighbor_node.ParentNode = CurNode;
                OpenList.Add(neighbor_node);
            }
        }
    }
    private void OnDrawGizmos()
    {
        //Scene뷰에 Debug용도로 그림을 그릴 때 사용
        if (Complete_Node != null)
        {
            for (int i = 0; i < Complete_Node.Count - 1; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector2(Complete_Node[i].X, Complete_Node[i].Y),
                    new Vector2(Complete_Node[i + 1].X, Complete_Node[i + 1].Y));
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PathFinding();
            DrawWayOnScreen();
            StartCoroutine(ShowWay());
        }
    }
    private void DrawWayOnScreen()
    {
        if (lineRenderer == null || Complete_Node == null || Complete_Node.Count == 0)
        {
            return;
        }

        Vector3[] wayposition = new Vector3[Complete_Node.Count+1];
        wayposition[0] = new Vector3(-9f, -5f, -0.5f);

        for (int i = 1; i <= Complete_Node.Count; i++)
        {
            Node node = Complete_Node[i-1];
            float x = node.X ;
            float y = node.Y ;
            wayposition[i] = new Vector3(x, y, -0.5f);
        }
        lineRenderer.enabled = true;
        lineRenderer.positionCount = wayposition.Length;
        lineRenderer.SetPositions(wayposition);

    }
    private IEnumerator ShowWay()
    {
        if (lineRenderer == null || Complete_Node == null || Complete_Node.Count == 0)
        {
            yield break;
        }
        DrawWayOnScreen();
        yield return new WaitForSeconds(showScreen);

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;

        }
    }
    private bool CheckTile(Vector3Int cellPos) //장애물이 있는지 체크
    {
        if (mapData.Wall.GetTile(cellPos) != null)
        {
            return false;
        }
        if (mapData.installTile.GetTile(cellPos) != null)
        {
            return false;
        }
        if (mapData.mapTile.GetTile(cellPos) == null)
        {
            return false;
        }
        return true;
    }

}