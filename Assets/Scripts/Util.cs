using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Sign
{
    Addition ,
    Subtraction ,
    Multiplication,
    Division
}
public class Util
{
    public static T GetRandomFromEnum<T>() where T : struct, IConvertible
    {
        var values = Enum.GetValues(typeof(T));
        var index = Random.Range(0, values.Length);
        return (T) values.GetValue(index);
    }
}
  

