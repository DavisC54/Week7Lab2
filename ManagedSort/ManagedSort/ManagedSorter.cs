using System;

namespace ManagedSort
{
    /// <summary>
    /// Managed C# sort compiled outside Unity and imported as a DLL.
    /// </summary>
    public static class ManagedSorter
    {
        public static void Sort(int[] values)
        {
            Array.Sort(values);
        }
    }
}