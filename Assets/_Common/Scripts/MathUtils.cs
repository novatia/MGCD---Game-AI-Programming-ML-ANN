using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public static class MathUtils
{
    public static float GetMappedRangeValueClamped(Vector2 i_InputRange, Vector2 i_OutputRange, float i_Value)
    {
        return GetMappedRangeValueClamped(i_InputRange.x, i_InputRange.y, i_OutputRange.x, i_OutputRange.y, i_Value);
    }

    public static float GetMappedRangeValueClamped(float i_InputRangeMin, float i_InputRangeMax, float i_OuputRangeMin, float i_OutputRangeMax, float i_Value)
    {
        float inputRangePct = GetRangePct(i_InputRangeMin, i_InputRangeMax, i_Value);
        float outputValue = Mathf.Lerp(i_OuputRangeMin, i_OutputRangeMax, i_Value);
        return outputValue;
    }

    public static float GetRangePctClamped(Vector2 i_Range, float i_Value)
    {
        return Internal_GetRangePct(i_Range.x, i_Range.y, i_Value, true);
    }

    public static float GetRangePctClamped(float i_RangeMin, float i_RangeMax, float i_Value)
    {
        return Internal_GetRangePct(i_RangeMin, i_RangeMax, i_Value, true);
    }

    public static float GetRangePct(Vector2 i_Range, float i_Value)
    {
        return Internal_GetRangePct(i_Range.x, i_Range.y, i_Value, false);
    }

    public static float GetRangePct(float i_RangeMin, float i_RangeMax, float i_Value)
    {
        return Internal_GetRangePct(i_RangeMin, i_RangeMax, i_Value, false);
    }

    private static float Internal_GetRangePct(float i_RangeMin, float i_RangeMax, float i_Value, bool i_ClampResult)
    {
        float rangeSize = i_RangeMax - i_RangeMin;
        bool bRangeValid = Mathf.Abs(rangeSize) > Mathf.Epsilon;
        float rangePct = (bRangeValid) ? ((i_Value - i_RangeMin) / rangeSize) : (0.5f);
        return (i_ClampResult) ? Mathf.Clamp01(rangePct) : rangePct;
    }
}