using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    Dot[,] dots = new Dot[5, 5];
    int[] r1 = { 1, 1, 1, 2, 3 };
    int[] r2 = { 1, 2, 2, 2, 3 };
    int[] r3 = { 1, 4, 4, 1, 3 };
    int[] r4 = { 2, 2, 2, 2, 1 };
    int[] r5 = { 4, 4, 1, 2, 3 };
    GameObject prefab;
    // Start is called before the first frame update

    private void Awake()
    {
        prefab = (GameObject)Resources.Load("DOT");
        ConfigValue();

    }

    private void ConfigValue()
    {
        for(int i =0; i < 5; i++)
        {
            int[] tmpR = null;
            switch (i)
            {
                case 0:
                    tmpR = r1;
                    break;
                case 1:
                    tmpR = r2;
                    break;
                case 2:
                    tmpR = r3;
                    break;
                case 3:
                    tmpR = r4;
                    break;
                case 4:
                    tmpR = r5;
                    break;
            }
            for (int j =0; j < 5; j++)
            {
              
                
                Dot tmp = new Dot(new Vector2(j,i*-1 ), tmpR[j]); ;
                dots[j, i] = tmp;
                DotView dv = Instantiate(prefab, new Vector3(j, i * -1, 0), Quaternion.identity).GetComponent<DotView>();
                dv.ConfigColor(GetColor(tmp.typeValue));
                dv.name = "dots[j, i]" + i + "|" + j + ":|value" + tmp.typeValue;


            }
        }
    }

    private Color GetColor(int type)
    {
        Color tmpColor = new Color(type / 4.0f, type / 4.0f, 1.0f);
        return tmpColor;
    }

    List<List<Dot>> FireList = new List<List<Dot>> ();

    public void CheckSame()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5;j++)
            {
                //4向查找
                int checkMax = 4;
                bool checkRight = true;
                bool checkDown = true;
                while (checkMax > 1) {
                    if (checkRight) { 
                        if (check2Distance(i, j,  0, checkMax))
                        {
                            checkRight = false;
                        }
                    }
                    if (checkDown)  
                    {
                        if (check2Distance(i, j, 2, checkMax))
                        {
                            checkDown = false;
                        }
                    }
                    if (!checkRight && !checkDown)
                    {
                        break;
                    }
                    checkMax--;
                }
            }

        }
    }

    public bool check2Distance(int i, int j,int _direction,int Offset)
    {

        List<Dot> dotlist;
        bool result = false;
        if(_direction == 0) { 
            int checkValue = i + Offset;
            if (checkValue < 5)
            {
                if(dots[j, i].typeValue == dots[j, checkValue].typeValue)
                {
                    dotlist = new List<Dot>();
                    dotlist.Add(new Dot(new Vector2(j,checkValue), dots[j, checkValue].typeValue));
                    if( check1Distance(i, j, _direction, Offset - 1, dotlist)) {
                        dotlist.Add(new Dot(new Vector2(j, i), dots[j, i].typeValue));
                        FireList.Add(dotlist);
                        result = true;
                    }
                }
            }
        }
        if (_direction == 2)
        {
            int checkValue = j + Offset;
            if (checkValue < 5)
            {
                if (dots[j, i].typeValue == dots[checkValue, i].typeValue)
                {
                    dotlist = new List<Dot>();
                    _direction = 2;
                    dotlist.Add(new Dot(new Vector2(checkValue, i), dots[checkValue, i].typeValue));
                    if (check1Distance(i, j, _direction, Offset - 1, dotlist))
                    {
                        dotlist.Add(new Dot(new Vector2(j, i), dots[j, i].typeValue));
                        FireList.Add(dotlist);
                        result = true;
                    }
                }
            }
        }
        return result;
    }

    public bool check1Distance(int i, int j, int _direction, int Offset, List<Dot> dotlist)
    {
        bool result = false;
        int checkValue = 0;

        switch (_direction)
        {
            case 0:
                checkValue = i + Offset;
                if (dots[j, i].typeValue == dots[j,checkValue].typeValue)
                {
                    result = true;
                    dotlist.Add(new Dot(new Vector2(j,checkValue), dots[j, checkValue].typeValue));
                }
                break;
            case 2:
                checkValue = j + Offset;
                if (dots[j, i].typeValue == dots[checkValue,i].typeValue)
                {
                    result = true;
                    dotlist.Add(new Dot(new Vector2(checkValue, i), dots[checkValue, i].typeValue));
                }
                break;
        }
        
        if (result)
        {
            if(Offset - 1 > 0) { 
                result = check1Distance(i,j,_direction,Offset-1, dotlist);
            }
        }
        return result;

    }

    void Start()
    {
        CheckSame();
        checkFireListContaint();
        CheckDouble();
        foreach (List<Dot> dots in FireList)
        {
            Debug.Log("=====");
            foreach(Dot dt in dots)
            {
                Debug.Log("dt"+ dt.dotPosition.x+ "| "+ dt.dotPosition.y);
            }
        }
    }

    public void CheckDouble()
    {
        List<List<Dot>> bm = new List<List<Dot>>();
        List<List<Dot>> rm = new List<List<Dot>>();
        for (int i =0; i<FireList.Count; i++)
        {
            for (int j= i+1; j < FireList.Count; j++)
            {
                if(i != j)
                {
                    Dot sameDot;
                    if (checkRepetition(FireList[i], FireList[j],out sameDot))
                    {
                        List<Dot> tmp = new List<Dot>();
                        FireList[i].Remove(sameDot);
                        tmp.InsertRange(0, FireList[i]);
                        tmp.InsertRange(0, FireList[j]);
                        bm.Add(tmp);
                        rm.Add(FireList[i]);
                        rm.Add(FireList[j]);

                    }
                }
            }
               

        }

        bm.ForEach((dotlist) => {
            FireList.Add(dotlist);
        });

        rm.ForEach((dotlist) => {
            FireList.Remove(dotlist);
        });

    }

    public bool checkRepetition(List<Dot> list, List<Dot> clist, out Dot sameDot)
    {
        foreach (Dot dot in list)
        {
            foreach (Dot dot2 in clist)
            {
                if(dot.dotPosition == dot2.dotPosition)
                {
                    sameDot = dot;
                    return true;
                }
            }
        }
        sameDot = null;
        return false;
    }

    public void checkFireListContaint()
    {
        List<List<Dot>> need2removeList = new List<List<Dot>>();
        foreach (List<Dot> dotlistCompare in FireList)
        {
            checkCompareDoList(dotlistCompare, need2removeList);

        }
        need2removeList.ForEach(dotList =>
        {
            if (FireList.Contains(dotList)) { 
                FireList.Remove(dotList);
            }
        });

    }

    public void checkCompareDoList(List<Dot> dotlistCompare, List<List<Dot>> need2removeList)
    {
        foreach (List<Dot> dotList in FireList)
        {
            if (dotlistCompare.Count > dotList.Count)
            {
                for (int i = 0; i < dotList.Count; i++)
                {
                    Dot dot = dotList[i];
                    Dot resultDot = dotlistCompare.Find((cdot) => {
                        if (dot.dotPosition == cdot.dotPosition)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });
                    if (resultDot == null)
                    {
                        break;
                    }
                    else
                    {
                        if (i == dotList.Count - 1)
                        {
                            need2removeList.Add(dotList);
                        }
                    }
                }

            }
        }
    }


    void Update()
    {
        
    }
}
