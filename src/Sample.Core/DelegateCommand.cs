using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sample.Core
{
    public class DelegateCommand : DelegateCommandBase
    {
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, (Func<bool>)(() => true))
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base((Action<object>)(o => executeMethod()), (Func<object, bool>)(o => canExecuteMethod()))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", "DelegateCommand delegates cannot be null");
        }

        /// <summary>
        /// Executes the command.
        /// 
        /// </summary>
        public void Execute()
        {
            base.Execute((object)null);
        }

        /// <summary>
        /// Determines if the command can be executed.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// Returns <see langword="true"/> if the command can execute,otherwise returns <see langword="false"/>.
        /// </returns>
        public bool CanExecute()
        {
            return base.CanExecute((object)null);
        }
    }
    public abstract class DelegateCommandBase : ICommand
    {
        private readonly Action<object> executeMethod;
        private readonly Func<object, bool> canExecuteMethod;
        private bool _isActive;
        private List<WeakReference> _canExecuteChangedHandlers;

        


        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute. You must keep a hard
        ///             reference to the handler to avoid garbage collection and unexpected results. See remarks for more information.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// When subscribing to the <see cref="E:System.Windows.Input.ICommand.CanExecuteChanged"/> event using
        ///             code (not when binding using XAML) will need to keep a hard reference to the event handler. This is to prevent
        ///             garbage collection of the event handler because the command implements the Weak Event pattern so it does not have
        ///             a hard reference to this handler. An example implementation can be seen in the CompositeCommand and CommandBehaviorBase
        ///             classes. In most scenarios, there is no reason to sign up to the CanExecuteChanged event directly, but if you do, you
        ///             are responsible for maintaining the reference.
        /// 
        /// </remarks>
        /// 
        /// <example>
        /// The following code holds a reference to the event handler. The myEventHandlerReference value should be stored
        ///             in an instance member to avoid it from being garbage collected.
        /// 
        /// <code>
        /// EventHandler myEventHandlerReference = new EventHandler(this.OnCanExecuteChanged);
        ///             command.CanExecuteChanged += myEventHandlerReference;
        /// 
        /// </code>
        /// 
        /// </example>
        public event EventHandler CanExecuteChanged;

        protected DelegateCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", "DelegateCommand delegates cannot be null");
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Raises <see cref="E:System.Windows.Input.ICommand.CanExecuteChanged"/> on the UI thread so every
        ///             command invoker can requery <see cref="M:System.Windows.Input.ICommand.CanExecute(System.Object)"/> to check if the
        ///             <see cref="T:Microsoft.Practices.Prism.Commands.CompositeCommand"/> can execute.
        /// 
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="E:Microsoft.Practices.Prism.Commands.DelegateCommandBase.CanExecuteChanged"/> on the UI thread so every command invoker
        ///             can requery to check if the command can execute.
        /// 
        /// <remarks>
        /// Note that this will trigger the execution of <see cref="M:Microsoft.Practices.Prism.Commands.DelegateCommandBase.CanExecute(System.Object)"/> once for each invoker.
        /// </remarks>
        /// 
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.OnCanExecuteChanged();
        }

        
        void ICommand.Execute(object parameter)
        {
            this.Execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command with the provided parameter by invoking the <see cref="T:System.Action`1"/> supplied during construction.
        /// 
        /// </summary>
        /// <param name="parameter"/>
        protected void Execute(object parameter)
        {
            this.executeMethod(parameter);
        }

        /// <summary>
        /// Determines if the command can execute with the provided parameter by invoing the <see cref="T:System.Func`2"/> supplied during construction.
        /// 
        /// </summary>
        /// <param name="parameter">The parameter to use when determining if this command can execute.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the command can execute.  <see langword="False"/> otherwise.
        /// </returns>
        protected bool CanExecute(object parameter)
        {
            if (this.canExecuteMethod != null)
                return this.canExecuteMethod(parameter);
            else
                return true;
        }
    }
}
