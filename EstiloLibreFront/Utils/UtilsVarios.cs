namespace EstiloLibreFront.Utils;

public static class UtilsVarios
{
    public static bool ConContenido<T>(this IEnumerable<T> enumerable)
    {
        return enumerable != null && enumerable.Any();
    }

    public static bool T1AnteriorT2(DateTime? t1, DateTime? t2)
    {
        int iResultado;

        iResultado = DateTime.Compare(t1.GetValueOrDefault(), t2.GetValueOrDefault());

        if (iResultado < 0)
        {
            return true;
        }
        return false;
    }

    public static bool T1AnteriorT2(TimeSpan? t1, TimeSpan? t2)
    {
        int iResultado;

        iResultado = t1.GetValueOrDefault().CompareTo(t2.GetValueOrDefault());

        if (iResultado < 0)
        {
            return true;
        }
        return false;
    }

    public static bool T1AnteriorOIgualT2(DateTime? t1, DateTime? t2)
    {
        int iResultado;

        iResultado = DateTime.Compare(t1.GetValueOrDefault(), t2.GetValueOrDefault());

        if (iResultado <= 0)
        {
            return true;
        }
        return false;
    }

    public static bool T1AnteriorOIgualT2(TimeSpan? t1, TimeSpan? t2)
    {
        int iResultado;

        iResultado = t1.GetValueOrDefault().CompareTo(t2.GetValueOrDefault());

        if (iResultado <= 0)
        {
            return true;
        }
        return false;
    }

    public static bool T1IgualT2(DateTime? t1, DateTime? t2)
    {
        int iResultado;

        iResultado = DateTime.Compare(t1.GetValueOrDefault(), t2.GetValueOrDefault());

        if (iResultado == 0)
        {
            return true;
        }
        return false;
    }

    public static bool T1IgualT2(TimeSpan? t1, TimeSpan? t2)
    {
        int iResultado;

        iResultado = t1.GetValueOrDefault().CompareTo(t2.GetValueOrDefault());

        if (iResultado == 0)
        {
            return true;
        }
        return false;
    }
}
