using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_Knight : Movement {

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Movement c;
        int i, j;

        //Top Side
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i > 0 || i < 8)
                {
                    c = BoardManager.Instance.Cards[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isBottomteam != c.isBottomteam)
                        r[i, j] = true;
                }
                i++;
            }
        }

        //Bottom side
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i > 0 || i < 8)
                {
                    c = BoardManager.Instance.Cards[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isBottomteam != c.isBottomteam)
                        r[i, j] = true;
                }
                i++;
            }
        }

        //Middle left
        if (CurrentX != 0)
        {
            c = BoardManager.Instance.Cards[CurrentX - 1, CurrentY];
            if (c == null)
                r[CurrentX - 1, CurrentY] = true;
            else if (isBottomteam != c.isBottomteam)
                r[CurrentX - 1, CurrentY] = true;

        }
        //Middle right

        if (CurrentX != 7)
        {
            c = BoardManager.Instance.Cards[CurrentX + 1, CurrentY];
            if (c == null)
                r[CurrentX + 1, CurrentY] = true;
            else if (isBottomteam != c.isBottomteam)
                r[CurrentX + 1, CurrentY] = true;
        }
        return r;
    }

}
