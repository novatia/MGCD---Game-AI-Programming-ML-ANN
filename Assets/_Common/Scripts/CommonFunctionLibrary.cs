using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public static class CommonFunctionLibrary
{
    public static void InitListNewElements<T>(List<T> i_List, int i_ElementCount) where T : new()
    {
        if (i_List == null)
        {
            return;
        }

        for (int index = 0; index < i_ElementCount; ++index)
        {
            T element = new T();
            i_List.Add(element);
        }
    }
}
