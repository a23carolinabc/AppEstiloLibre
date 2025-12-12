namespace EstiloLibreFront.Excepciones
{
    public class EstiloLibreException : ApplicationException
    {
        public EstiloLibreException(string msg, Exception excp) : base(msg, excp) { }
        public EstiloLibreException(string msg) : base(msg) { }
    }
}
