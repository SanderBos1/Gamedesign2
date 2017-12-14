using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public Movement[,] Cards {set; get;}
    private Movement selectedCard;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = 1;

    public List<GameObject> cardPrefabs;
    private List<GameObject> activeCards;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    public bool isBottomTurn = true;
    private void Start()
    {
        SpawnAllMinions();
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessBoard();
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedCard == null)
                {
                    SelectCards(selectionX, selectionY);
                }

                else
                {
                    //Move the card
                    MoveCards(selectionX, selectionY);
                }
                }
            }
        }
    


    private void SelectCards(int x, int y)
    {
        if(Cards[x, y] == null)
        return;
        if (Cards[x, y].isBottomteam!= isBottomTurn)
        return;

        selectedCard = Cards[x, y];
    }

    private void MoveCards(int x, int y)
    {
        if (selectedCard.PossibleMove(x, y))
        {
            Cards[selectedCard.CurrentX, selectedCard.CurrentY] = null;
            selectedCard.transform.position = GetTileCenter(x, y);
            Cards[x, y] = selectedCard;
            isBottomTurn = !isBottomTurn;
        }
        selectedCard = null;
    }
    //mousepointing
    private void UpdateSelection()
    {
            //checks if theirs a main camera
        if (!Camera.main)
            return;
        RaycastHit hit;

        //checks which point in the world it is
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("ChessPlane"))){

            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
        
    }
    

    //spawns the minions in the corerct place in the world
    private void SpawnMinions(int index, int x, int y)
    {
        GameObject go = Instantiate(cardPrefabs[index], GetTileCenter(x,y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Cards[x, y] = go.GetComponent<Movement>();
        Cards[x, y].SetPosition(x, y); //current position in the world
        activeCards.Add(go);
    }

    private void SpawnAllMinions()
    {

         activeCards = new List<GameObject>();
        Cards = new Movement[8, 8];
        //spawn the bottom team

        //king
        SpawnMinions(0,3,0);

        //Queen
        SpawnMinions(1, 4, 0);

        //Rooks
        SpawnMinions(2,0,0);
        SpawnMinions(2,7,0);

        //Bishops
        SpawnMinions(3,2,0);
        SpawnMinions(3,5,0);

        //Knights
        SpawnMinions(4,1,0);
        SpawnMinions(4,6,0);

        //Pawns
        for (int i = 0; i < 8; i++)
            SpawnMinions(5,i,1);
        
        //upper team

        //king
        SpawnMinions(6,3,7);

        //Queen
        SpawnMinions(7,4,7);

        //Rooks
        SpawnMinions(8,0,7);
        SpawnMinions(8,7,7);

        //Bishops
        SpawnMinions(9,2,7);
        SpawnMinions(9,5,7);

        //Knights
        SpawnMinions(10,1,7);
        SpawnMinions(10,6,7);

        //Pawns
        for (int i = 0; i < 8; i++)
            SpawnMinions(11,i,6);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }
    //draws chessboard
    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for(int i=0;i <=8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for(int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        //Draw the selection
        if(selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
            Debug.DrawLine(
            Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
            Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }
}