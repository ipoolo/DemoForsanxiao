using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeValue
{
    A,
    B,
    C,
    D
}

public class Dot : System.Object
{
    public Vector2 dotPosition;
    public int typeValue;

    public Dot(Vector2 v,int tv) {
        dotPosition = v;
        typeValue = tv;
    }

}
