using System.Globalization;

namespace EstiloLibreFront.Utils;

public class UtilsConversion
{ 
    public static int GetNumeroEntero(string strValor)
    {
        int iResultado;

        iResultado = 0;
        int.TryParse(strValor, out iResultado);

        return iResultado;
    }

    public static bool EsNumeroDecimal(string strNumero)
    {
        decimal iResultado;

        iResultado = 0.0m;
        return decimal.TryParse(strNumero, NumberStyles.Any, Thread.CurrentThread.CurrentCulture, out iResultado);
    }

    public static decimal GetNumeroDecimal(string strNumero)
    {
        decimal iResultado;

        iResultado = 0.0m;
        decimal.TryParse(strNumero, NumberStyles.Any, Thread.CurrentThread.CurrentCulture, out iResultado);
        return iResultado;
    }

    public static decimal GetNumeroDecimalGenerico(string strNumero)
    {
        decimal iResultado;

        iResultado = 0.0m;
        decimal.TryParse(strNumero, NumberStyles.Any, CultureInfo.InvariantCulture, out iResultado);
        return iResultado;
    }

    public static decimal GetPorcentaje(string strPorcentaje)
    {
        decimal decResultado;

        decResultado = 0.0m;
        decimal.TryParse(strPorcentaje.Replace("%", ""), NumberStyles.Any, Thread.CurrentThread.CurrentCulture, out decResultado);
        return decResultado;
    }

    public static decimal GetImporteMoneda(string strImporte)
    {
        decimal decResultado;

        decResultado = 0.0m;
        decimal.TryParse(strImporte, NumberStyles.Currency, Thread.CurrentThread.CurrentCulture, out decResultado);
        return decResultado;
    }

    public static DateTime GetFecha(string strFecha)
    {
        DateTime dtResultado;

        dtResultado = DateTime.MinValue;
        DateTime.TryParse(strFecha, Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out dtResultado);
        return dtResultado;
    }
}
