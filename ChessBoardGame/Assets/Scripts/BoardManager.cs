using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public static BoardManager Instance { set; get; }
    public bool[,] allowedMoves { set; get;}
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
        Instance = this;
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

        allowedMoves = Cards[x, y].PossibleMove();
        selectedCard = Cards[x, y];
        BoardHighLights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveCards(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Movement c = Cards[x, y];

            if(c != null && c.isBottomteam != isBottomTurn)
            {
                //destory a card
                activeCards.Remove(c.gameObject);
                Destroy(c.gameObject);
                //if it is the king
                if(c.GetType () == typeof(Minion1))
                {
                    //end game
                    return;
                }
            }
            Cards[selectedCard.CurrentX, selectedCard.CurrentY] = null;
            selectedCard.transform.position = GetTileCenter(x, y);
            selectedCard.SetPosition(x, y);
            Cards[x, y] = selectedCard;
            isBottomTurn = !isBottomTurn;
        }
        BoardHighLights.Instance.HideHighlights();
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

        //Dennis
        SpawnMinions(0, 3, 0);

        //Death_Knight
        SpawnMinions(1, 4, 0);

        //Necromancer
        SpawnMinions(2, 0, 0);

        //Skeleton Archer
        SpawnMinions(3, 2, 0);

        //King Leopold
        SpawnMinions(4, 1, 7);

        //Enchantress
        SpawnMinions(5, 2, 7);

        //Artic_Archer
        SpawnMinions(6, 3, 7);

        //Warrior 1
        SpawnMinions(7, 4, 7);

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