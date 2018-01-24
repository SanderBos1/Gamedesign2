using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public int damage;

    public GameObject LightNone;
    public GameObject LightPQR;
    public GameObject LightP;
    public GameObject LightQ;
    public GameObject LightR;
    public GameObject LightPQ;
    public GameObject LightQR;
    public GameObject LightPR;
    public GameObject DarkNone;
    public GameObject DarkPQR;
    public GameObject DarkP;
    public GameObject DarkQ;
    public GameObject DarkR;
    public GameObject DarkPQ;
    public GameObject DarkQR;
    public GameObject DarkPR;
    List<GameObject> Cubes;
    int[,] Grid;

    public List<int> Artic_Archer = new List<int> { 7, 3, 3 };
    public List<int> Dennis = new List<int> { 20, 5, 5 };
    public List<int> Death_Knight = new List<int> { 8, 3, 3 };
    public List<int> Enchantress = new List<int> { 6, 4, 2 };
    public List<int> King_Leopold = new List<int> { 20, 5, 5 };
    public List<int> Necromancer = new List<int> { 7, 3, 2 };
    public List<int> Skeleton_Archer = new List<int> { 5, 2, 1 };
    public List<int> Warrior = new List<int> { 9, 3, 4 };

    public static BoardManager Instance { set; get; }
    public bool[,] allowedMoves { set; get; }
    public Movement[,] Cards { set; get; }
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
        Cubes = new List<GameObject>();
        Grid = new int[8, 8];
        DrawChessBoard();
        SpawnAllMinions();
        MoveCamera();
    }

    private void Update()
    {
        UpdateSelection();
        DrawSelection();
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
        if (Cards[x, y] == null)
            return;
        if (Cards[x, y].isBottomteam != isBottomTurn)
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

            if (c != null && c.isBottomteam != isBottomTurn)
            {
                //From here starts health manage things

                //Find the attacker and set the damage to the attacker's corresponding damage
                var attacker = Cards[selectedCard.CurrentX, selectedCard.CurrentY];
                int i = selectedCard.CurrentX;
                int j = selectedCard.CurrentY;

                iTween.ScaleFrom(attacker.gameObject, new Vector3(1.6f, 3, 1), 2.5f);

                if (attacker.GetType() == typeof(Artic_Archer))
                {
                    damage = Artic_Archer[1];
                    Debug.Log(damage);
                    if (Grid[i, j] == 0)
                    {
                        damage *= 2;
                        Debug.Log(damage);
                    }
                }

                else if (attacker.GetType() == typeof(Dennis))
                {
                    damage = Dennis[1];
                    Debug.Log(damage);
                    if (Grid[i, j] == 1 | Grid[i, j] == 5 | Grid[i, j] == 7)
                    {
                        damage *= 2;
                        Debug.Log(damage);
                    }
                }

                else if (attacker.GetType() == typeof(Death_Knight))
                {
                    damage = Death_Knight[1];
                    Debug.Log(damage);
                    if (Grid[i, j] == 0 | Grid[i, j] == 7)
                    {
                        damage *= 2;
                        Debug.Log(damage);
                    }
                }

                else if (attacker.GetType() == typeof(Enchantress))
                {
                    damage = Enchantress[1];
                    Debug.Log(damage);
                    if (Grid[i, j] == 0 | Grid[i, j] == 4)
                    {
                        damage *= 2;
                        Debug.Log(damage);
                    }
                }

                else if (attacker.GetType() == typeof(King_Leopold))
                {
                    damage = King_Leopold[1];
                    Debug.Log(damage);
                    if (Grid[i, j] == 5 | Grid[i, j] == 3 | Grid[i, j] == 0)
                    {
                        damage *= 2;
                        Debug.Log(damage);
                    }
                }

                else if (attacker.GetType() == typeof(Necromancer))
                {
                    damage = Necromancer[1];
                    Debug.Log(damage);
                    if (Grid[i, j] == 0 | Grid[i, j] == 4)
                    {
                        damage *= 2;
                        Debug.Log(damage);
                    }
                }

                else if (attacker.GetType() == typeof(Skeleton_Archer))
                {
                    damage = Skeleton_Archer[1];
                    Debug.Log(damage);
                    if (Grid[i, j] == 0)
                    {
                        damage *= 2;
                        Debug.Log(damage);
                    }
                }

                else if (attacker.GetType() == typeof(Warrior))
                {
                    damage = Warrior[1];
                    Debug.Log(damage);
                    if (Grid[i, j] == 0 | Grid[i, j] == 7)
                    {
                        damage *= 2;
                        Debug.Log(damage);
                    }

                }

                //Now find the victim
                if (c.GetType() == typeof(Artic_Archer))
                {
                    if (dodge(Artic_Archer[2]) == false)
                    {
                        Artic_Archer[0] = Artic_Archer[0] - damage;
                        Debug.Log("Artic_Archer new health: " + Artic_Archer[0]);

                        if (Artic_Archer[0] <= 0)
                        {
                            Debug.Log("Victim died");
                            //remove component
                            activeCards.Remove(c.gameObject);
                            Destroy(c.gameObject);
                        }
                    }
                    else
                    {
                        iTween.ScaleFrom(c.gameObject, new Vector3(0.4f, 0.75f, 0.25f), 4f);
                    }
                }

                else if (c.GetType() == typeof(Death_Knight))
                {
                    if (dodge(Death_Knight[2]) == false)
                    {
                        Death_Knight[0] = Death_Knight[0] - damage;
                        Debug.Log("new health: " + Death_Knight[0]);

                        if (Death_Knight[0] <= 0)
                        {
                            Debug.Log("Victim died");
                            //remove component
                            activeCards.Remove(c.gameObject);
                            Destroy(c.gameObject);
                        }
                    }
                    else
                    {
                        iTween.ScaleFrom(c.gameObject, new Vector3(0.4f, 0.75f, 0.25f), 4f);
                    }
                }

                else if (c.GetType() == typeof(Enchantress))
                {
                    if (dodge(Enchantress[2]) == false)
                    {
                        Enchantress[0] = Enchantress[0] - damage;
                        Debug.Log("new health: " + Enchantress[0]);

                        if (Enchantress[0] <= 0)
                        {
                            Debug.Log("Victim died");
                            //remove component
                            activeCards.Remove(c.gameObject);
                            Destroy(c.gameObject);
                        }
                    }
                    else
                    {
                        iTween.ScaleFrom(c.gameObject, new Vector3(0.4f, 0.75f, 0.25f), 4f);
                    }
                }

                else if (c.GetType() == typeof(Necromancer))
                {
                    if (dodge(Necromancer[2]) == false)
                    {
                        Necromancer[0] = Necromancer[0] - damage;

                        if (Necromancer[0] <= 0)
                        {
                            Debug.Log("Victim died");
                            //remove component
                            activeCards.Remove(c.gameObject);
                            Destroy(c.gameObject);
                        }
                    }
                    else
                    {
                        iTween.ScaleFrom(c.gameObject, new Vector3(0.4f, 0.75f, 0.25f), 4f);
                    }
                }

                else if (c.GetType() == typeof(Skeleton_Archer))
                {
                    if (dodge(Skeleton_Archer[2]) == false)
                    {
                        Skeleton_Archer[0] = Skeleton_Archer[0] - damage;

                        if (Skeleton_Archer[0] <= 0)
                        {
                            Debug.Log("Victim died");
                            //remove component
                            activeCards.Remove(c.gameObject);
                            Destroy(c.gameObject);
                        }
                    }
                    else
                    {
                        iTween.ScaleFrom(c.gameObject, new Vector3(0.4f, 0.75f, 0.25f), 4f);
                    }
                }

                else if (c.GetType() == typeof(Warrior))
                {
                    if (dodge(Warrior[2]) == false)
                    {
                        Warrior[0] = Warrior[0] - damage;

                        if (Warrior[0] <= 0)
                        {
                            Debug.Log("Victim died");
                            //remove component
                            activeCards.Remove(c.gameObject);
                            Destroy(c.gameObject);
                        }
                    }
                    else
                    {
                        iTween.ScaleFrom(c.gameObject, new Vector3(0.4f, 0.75f, 0.25f), 4f);
                    }
                }

                else if (c.GetType() == typeof(Dennis))
                {
                    if (dodge(Dennis[2]) == false)
                    {
                        Dennis[0] = Dennis[0] - damage;
                        Debug.Log("Dennis new health: " + Dennis[0]);

                        if (Dennis[0] <= 0)
                        {
                            Debug.Log("Victim died, GAME ENDS");
                            activeCards.Remove(c.gameObject);
                            Destroy(c.gameObject);
                            //GAME ENDS
                            return;
                        }
                    }
                    else
                    {
                        iTween.ScaleFrom(c.gameObject, new Vector3(0.4f, 0.75f, 0.25f), 2.5f);
                    }

                }

                else if (c.GetType() == typeof(King_Leopold))
                {
                    if (dodge(King_Leopold[2]) == false)
                    {
                        King_Leopold[0] = King_Leopold[0] - damage;
                        Debug.Log("King Leopold new health: " + King_Leopold[0]);

                        if (King_Leopold[0] <= 0)
                        {
                            Debug.Log("Victim died, GAME ENDS");
                            activeCards.Remove(c.gameObject);
                            Destroy(c.gameObject);
                            //GAME ENDS
                            return;
                        }
                    }
                    else
                    {
                        iTween.ScaleFrom(c.gameObject, new Vector3(0.4f, 0.75f, 0.25f), 4f);
                    }
                }



                //END OF HEALTH MANAGE

                /*REMOVE THIS PART LATER, IT IS NOT NEEDED ANY MORE
                //destory a card
                activeCards.Remove(c.gameObject);
                Destroy(c.gameObject);
                //if it is the king
                if(c.GetType () == typeof(Dennis))
                {
                    //end game
                    return;
                }
                */
                isBottomTurn = !isBottomTurn;
                StartCoroutine(Wait(0.25f));
            }

            else
            {
                Cards[selectedCard.CurrentX, selectedCard.CurrentY] = null;
               // selectedCard.transform.position = GetTileCenter(x, y);
                iTween.MoveTo(selectedCard.gameObject, GetTileCenter(x, y), 1);
                selectedCard.SetPosition(x, y);
                Cards[x, y] = selectedCard;
                isBottomTurn = !isBottomTurn;
                StartCoroutine(Wait(0.25f));
            }
        }
        BoardHighLights.Instance.HideHighlights();
        selectedCard = null;
    }

    //calcutales if victim dodges incoming attack and returns true if it did
    private bool dodge(int defence)
    {

        int dodge = UnityEngine.Random.Range(1, 10);
        if (dodge < defence)
        {
            //the victim dodged the attack
            Debug.Log("Victim dodged!");
            return true;
        }

        else
        {
            return false;
        }
    }


    //mousepointing
    private void UpdateSelection()
    {
        //checks if theirs a main camera
        if (!Camera.main)
            return;
        RaycastHit hit;

        //checks which point in the world it is
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("ChessPlane")))
        {

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
        GameObject go = Instantiate(cardPrefabs[index], GetTileCenter(x, y), orientation) as GameObject;
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
        origin.y = 0.6f;
        return origin;
    }
    //draws chessboard
    private void DrawChessBoard()
    {
        Cubes.Add(LightNone);
        Cubes.Add(LightPQR);
        Cubes.Add(LightP);
        Cubes.Add(LightQ);
        Cubes.Add(LightR);
        Cubes.Add(LightPQ);
        Cubes.Add(LightQR);
        Cubes.Add(LightPR);
        Cubes.Add(DarkNone);
        Cubes.Add(DarkPQR);
        Cubes.Add(DarkP);
        Cubes.Add(DarkQ);
        Cubes.Add(DarkR);
        Cubes.Add(DarkPQ);
        Cubes.Add(DarkQR);
        Cubes.Add(DarkPR);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                float o = 0.5f;
                if ((i + j) % 2 == 0)
                {
                    int rnd = UnityEngine.Random.Range(0, 8);
                    Grid[i, j] = rnd;
                    Object.Instantiate(Cubes[rnd], new Vector3(i + o, -0.51f, j + o), Quaternion.identity);
                }
                else
                {
                    int rnd = UnityEngine.Random.Range(8, 15);
                    Grid[i, j] = rnd - 8;
                    Object.Instantiate(Cubes[rnd], new Vector3(i + o, -0.51f, j + o), Quaternion.identity);
                }
            }
        }
    }

    private void DrawSelection()
    {   //Draw the selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
               Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
            Debug.DrawLine(
            Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
           Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }
    private IEnumerator Wait(float n)
    {
        yield return new WaitForSeconds(n);
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 CameraPosition = new Vector3(4, 6, -1);
        Vector3 CameraPosition2 = new Vector3(4, 6, 9);

        Vector3 CameraRotation = new Vector3(55, 0, 0);
        Vector3 CameraRotation2 = new Vector3(55, 180, 0);
        if (isBottomTurn)
        {
            iTween.MoveTo(Camera.main.gameObject, CameraPosition, 1);
            iTween.RotateTo(Camera.main.gameObject, CameraRotation, 1);


        }
        else
        {
            iTween.MoveTo(Camera.main.gameObject, CameraPosition2, 1);
            iTween.RotateTo(Camera.main.gameObject, CameraRotation2, 1);

        }
    }



}