using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ManagedSort;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// Loads the Random.org numbers and times all three sorts against identical copies of the data
/// </summary>
public class SortTimer : MonoBehaviour
{
    [SerializeField] private TextAsset numbersFile;

    private MonoSorter monoSorter;
    private int[] numbers;

    [DllImport("NativeSort")]
    private static extern void TestSort(int[] a, int length);

    private void Awake()
    {
        monoSorter = GetComponent<MonoSorter>();
        numbers = ParseNumbers(numbersFile);
    }

    private void Start()
    {
        if (numbers == null)
        {
            return;
        }

        WarmUp();

        TimeSort("C++ native", values => TestSort(values, values.Length));
        TimeSort("C# managed DLL", ManagedSorter.Sort);
        TimeSort("C# MonoBehaviour", monoSorter.Sort);
    }

    private int[] ParseNumbers(TextAsset file)
    {
        if (file == null)
        {
            Debug.LogError("no numbers file assigned");
            return null;
        }

        // split on any whitespace because Random.org output uses spaces
        string[] tokens = file.text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        int[] parsed = new int[tokens.Length];

        for (int i = 0; i < tokens.Length; i++)
        {
            parsed[i] = int.Parse(tokens[i]);
        }

        Debug.Log($"loaded {parsed.Length} ints");
        return parsed;
    }

    // untimed first run so one-time load costs don't skew the results
    private void WarmUp()
    {
        TestSort((int[])numbers.Clone(), numbers.Length);
        ManagedSorter.Sort((int[])numbers.Clone());
        monoSorter.Sort((int[])numbers.Clone());
    }

    // Sorts a fresh copy each time so no method gets handed data that is already sorted
    private void TimeSort(string label, Action<int[]> sort)
    {
        // Clone before starting the stopwatch so the copy isn't part of the time
        int[] copy = (int[])numbers.Clone();

        Stopwatch stopwatch = Stopwatch.StartNew();
        sort(copy);
        stopwatch.Stop();

        // ElapsedMilliseconds rounds to whole ms and just shows 0 at this size
        Debug.Log($"{label}: {stopwatch.Elapsed.TotalMilliseconds:F3} ms");
    }
}