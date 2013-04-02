namespace SampleApp.Domain
{
    public abstract class ModelState
    {
        public static readonly ModelState Idle = new IdleState();
        public static readonly ModelState Processing = new ProcessingState();
        public static ModelState Error(string errorMessage)
        {
            return new ErrorState(errorMessage);
        }

        private ModelState()
        { }
        public virtual bool IsProcessing { get { return false; } }
        public virtual bool HasError { get { return false; } }
        public virtual string ErrorMessage { get { return null; } }

        private sealed class IdleState : ModelState
        {
        }

        private sealed class ProcessingState : ModelState
        {
            public override bool IsProcessing { get { return true; } }
        }

        private sealed class ErrorState : ModelState
        {
            private readonly string _errorMessage;

            public ErrorState(string errorMessage)
            {
                _errorMessage = errorMessage;
            }

            public override bool HasError { get { return true; } }
            public override string ErrorMessage { get { return _errorMessage; } }
        }
    }
}