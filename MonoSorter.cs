using System;
using UnityEngine;

/// <summary>
/// Same sort written just in unity
/// </summary>
public class MonoSorter : MonoBehaviour
{
    public void Sort(int[] values)
    {
        Array.Sort(values);
    }
}