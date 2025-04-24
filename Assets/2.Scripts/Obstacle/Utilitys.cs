using UnityEngine;

public static class Utilitys
{
    /*
    SetIfNegative는 ref로 들어온 값이 음수이면 value값으로 바뀌는 메서드이다.
    현재 지원되는 자료형
    int, float, double, long
    */
    public static void SetIfNegative(ref int field, int value)
    {
        if (field < 0)
        {
            field = value;
        }
    }

    public static void SetIfNegative(ref float field, float value)
    {
        if (field < 0f)
        {
            field = value;
        }
    }

    public static void SetIfNegative(ref double field, double value)
    {
        if (field < 0.0)
        {
            field = value;
        }
    }

    public static void SetIfNegative(ref long field, long value)
    {
        if (field < 0L)
        {
            field = value;
        }
    }

    public static Vector3 FloatToVecter3(float[] floats)
    {
        if (floats == null || floats.Length != 3)
        {
            return Vector3.zero;
        }
        Vector3 result = new Vector3(floats[0], floats[1], floats[2]);
        return result;
    }

    public static float[] Vector3ToFloat(Vector3 vector)
    {
        return new float[3] { vector.x, vector.y, vector.z };
    }
}
