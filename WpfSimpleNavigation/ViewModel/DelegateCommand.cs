using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfSimpleNavigation.ViewModel
{
    /// <summary>
    /// ICommand implementation that can be used for nearly all kind of function signaturs
    /// Inspired by the Xamarin implementation, see Xamarin.Forms.Command
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// Encapsulated the execute action
        /// </summary>
        private Action<object> execute;
        private Action executePL;

        /// <summary>
        /// Encapsulated the representation for the validation of the execute method
        /// </summary>
        private Func<object, bool> canExecute;
        

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public DelegateCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public DelegateCommand(Action execute)
            : this(execute, DefaultCanExecute)
        {
            this.executePL = execute;
        }



        /// <summary>
        /// Initializes a new instance of the RelayCommand class
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public DelegateCommand(Action execute, Func<object, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.executePL = execute;
            this.canExecute = canExecute;
        }




        #endregion // Constructors

        #region ICommand Members

        /// <summary>
        /// An event to raise when the CanExecute value is changed
        /// </summary>
        /// <remarks>
        /// Any subscription to this event will automatically subscribe to both 
        /// the local OnCanExecuteChanged method AND
        /// the CommandManager RequerySuggested event
        /// </remarks>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        /// <summary>
        /// An event to allow the CanExecuteChanged event to be raised manually
        /// </summary>
        private event EventHandler CanExecuteChangedInternal;

        /// <summary>
        /// Defines if command can be executed
        /// </summary>
        /// <param name="parameter">the parameter that represents the validation method</param>
        /// <returns>true if the command can be executed</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        /// <summary>
        /// Execute the encapsulated command
        /// </summary>
        /// <param name="parameter">the parameter that represents the execution method</param>
        public void Execute(object parameter)
        {
            if(this.execute != null)
                this.execute(parameter);
            else if (this.executePL != null)
                this.executePL();
        }

        #endregion // ICommand Members

        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Destroys this instance.
        /// </summary>
        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
            this.executePL = () => { return; };
        }

        /// <summary>
        /// Defines if command can be executed (default behaviour)
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Always true</returns>
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }


    }
}
