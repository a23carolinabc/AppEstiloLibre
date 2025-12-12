using EstiloLibreFront.Traducciones.TrGeneral;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace EstiloLibreFront.Utils;

public class UtilsFormat
{    
    public static string FormatearFecha(DateTime dtFecha)
    {
        if (dtFecha == default)
        {
            return string.Empty;
        }
        else
        {
            return dtFecha.ToString("dd/MM/yyyy");
        }
    }

    public static string FormatearFechaLarga(DateTime dtFecha, IStringLocalizer<TrGeneral> traductor)
    {        
        if (dtFecha == default)
        {
            return string.Empty;
        }
        else
        {
            string fechaCompleta = $"{dtFecha.ToString("dd")} de {traductor[dtFecha.ToString("MMMM")]} " +
                                    $"{traductor["de"]} {dtFecha.ToString("yyyy")}";
            return fechaCompleta;
        }
    }

    public static string FormatearHora(DateTime dtFecha)
    {
        if (dtFecha == default)
        {
            return string.Empty;
        }
        else
        {
            return dtFecha.ToString("HH:mm");
        }
    }

    public static string FormatearFechaHora(DateTime dtFecha)
    {
        if (dtFecha == default)
        {
            return string.Empty;
        }
        else
        {
            return dtFecha.ToString("dd/MM/yyyy HH:mm");
        }
    }

    public static string FormatearNumero(decimal decNumero, int iNumDecimales = 2, bool bCeroComoCadenaVacia = false)
    {
        NumberFormatInfo nfi;

        if (bCeroComoCadenaVacia && decNumero == 0) return "";
        nfi = (NumberFormatInfo) Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
        nfi.NumberDecimalDigits = iNumDecimales;
        return decNumero.ToString("N", nfi);
    }

    public static string FormatearImporte(decimal decImporte, int iNumDecimales = 2, bool bCeroComoCadenaVacia = false)
    {
        NumberFormatInfo nfi;

        if (bCeroComoCadenaVacia && decImporte == 0) return "";
        nfi = (NumberFormatInfo) Thread.CurrentThread.CurrentCulture.NumberFormat.Clone();
        nfi.CurrencyDecimalDigits = iNumDecimales;
        return decImporte.ToString("C", nfi);
    }

    public static string FormatearEntero(int iNumero, bool bCeroComoCadenaVacia = false)
    {
        return FormatearNumero(iNumero, 0, bCeroComoCadenaVacia);
    }

    public static string FormatearValorComoPuntoKilometrico(decimal decNumero)
    {
        decimal decParteEntera;
        decimal decParteDecimal;

        decParteEntera = Math.Truncate(decNumero);
        decParteDecimal = (decNumero - decParteEntera) * 10000;

        return $"{decParteEntera:00}+{decParteDecimal:0000}";
    }
}
