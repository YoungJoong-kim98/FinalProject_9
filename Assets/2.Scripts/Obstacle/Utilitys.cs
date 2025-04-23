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
}
