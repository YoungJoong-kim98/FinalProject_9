public static class Utilitys
{
    /*
    SetIfNegative�� ref�� ���� ���� �����̸� value������ �ٲ�� �޼����̴�.
    ���� �����Ǵ� �ڷ���
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
