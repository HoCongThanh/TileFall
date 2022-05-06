using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeUtils
{
    public static int GetTimestampSecond()
    {
        System.Int32 unixTimestamp = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        return unixTimestamp;
    }
}
