using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion6 : Movement {

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Movement c, c2;

        //Bottom Team move
        if(isBottomteam)

        {
            //diagonal left
            if(CurrentX !=0 && CurrentY != 7)
            {
                c = BoardManager.Instance.Cards[CurrentX - 1, CurrentY + 1 ];
                if (c != null && !c.isBottomteam)
                    r[CurrentX - 1, CurrentY + 1] = true;
            }
            //diagonal right
            if (CurrentX != 7 && CurrentY != 7)
            {
                c = BoardManager.Instance.Cards[CurrentX + 1, CurrentY + 1 ];
                if (c != null && !c.isBottomteam)
                    r[CurrentX + 1, CurrentY + 1] = true;
            }
            //middle
            if(CurrentY != 7)
            {
                c = BoardManager.Instance.Cards[CurrentX, CurrentY + 1];
                if(c == null)
                    r[CurrentX, CurrentY +1] = true;
            }
            //middle on first move
            if (CurrentY == 1)
            {
                c = BoardManager.Instance.Cards[CurrentX, CurrentY + 1];
                c2 = BoardManager.Instance.Cards[CurrentX, CurrentY + 2];
                if (c == null & c2 == null)
                    r[CurrentX, CurrentY + 2] = true;
            }
        }

        //upperteam
        else
        {
            //diagonal left
            if (CurrentX != 0 && CurrentY != 0)
            {
                c = BoardManager.Instance.Cards[CurrentX - 1, CurrentY - 1];
                if (c != null && c.isBottomteam)
                    r[CurrentX - 1, CurrentY - 1] = true;
            }
            //diagonal right
            if (CurrentX != 7 && CurrentY != 0)
            {
                c = BoardManager.Instance.Cards[CurrentX + 1, CurrentY - 1];
                if (c != null && c.isBottomteam)
                    r[CurrentX + 1, CurrentY - 1] = true;
            }
            //middle
            if (CurrentY != 0)
            {
                c = BoardManager.Instance.Cards[CurrentX, CurrentY -1 ];
                if (c == null)
                    r[CurrentX, CurrentY - 1] = true;
            }
            //middle on first move
            if (CurrentY == 6)
            {
                c = BoardManager.Instance.Cards[CurrentX, CurrentY -1 ];
                c2 = BoardManager.Instance.Cards[CurrentX, CurrentY -1];
                if (c == null & c2 == null)
                    r[CurrentX, CurrentY - 2] = true;
            }
        }
        return r;
    }

}
