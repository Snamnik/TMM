namespace TMM.Domain.SeedWork
{
    public abstract class TMMException : Exception
    {
        protected TMMException(string message, params object[] arguments) : base(message)
        {
            AddArguments(arguments);
        }

        protected TMMException(string message, Exception innerException, params object[] arguments) : base(message, innerException)
        {
            AddArguments(arguments);
        }

        public object[] Arguments { get; private set; }  
        private void AddArguments(params object[] arguments)
        {
            Arguments = new object[arguments.Length];
            arguments.CopyTo(Arguments, 0);
        }

    }
}
