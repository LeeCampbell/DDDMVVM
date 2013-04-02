using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SampleApp.UI
{
    public sealed class TextBoxBehavior : DependencyObject
    {
        #region CoerceValue Attached property
 
        public static bool GetCoerceValue(DependencyObject obj)
        {
            return (bool)obj.GetValue(CoerceValueProperty);
        }
        public static void SetCoerceValue(DependencyObject obj, bool value)
        {
            obj.SetValue(CoerceValueProperty, value);
        }
 
        /// <summary>
        /// Gets or Sets whether the TextBox should reevaluate the binding after it pushes a change (either on LostFocus or PropertyChanged depending on the binding).
        /// </summary>
        public static readonly DependencyProperty CoerceValueProperty =
            DependencyProperty.RegisterAttached("CoerceValue", typeof(bool), typeof(TextBoxBehavior), new UIPropertyMetadata(false, CoerceValuePropertyChanged));
 
        static void CoerceValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as TextBox;
            if (textbox == null)
                return;
 
            if ((bool)e.NewValue)
            {
                if (textbox.IsLoaded)
                {
                    PrepareTextBox(textbox);
                }
                else
                {
                    textbox.Loaded += OnTextBoxLoaded;
                }
            }
            else
            {
                textbox.TextChanged -= OnCoerceText;
                textbox.LostFocus -= OnCoerceText;
                textbox.Loaded -= OnTextBoxLoaded;
            }
        }
 
        static void OnTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            PrepareTextBox(textbox);
            textbox.Loaded -= OnTextBoxLoaded;
        }
 
        static void OnCoerceText(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var selectionStart = textBox.SelectionStart;
            var selectionLength = textBox.SelectionLength;
 
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
 
            if (selectionStart < textBox.Text.Length) textBox.SelectionStart = selectionStart;
            if (selectionStart + selectionLength < textBox.Text.Length) textBox.SelectionLength = selectionLength;
        }
 
        private static void PrepareTextBox(TextBox textbox)
        {
            if (textbox == null) return;
            var bindingExpression = textbox.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression == null) return;
            var binding = bindingExpression.ParentBinding;
            var newBinding = binding.Clone();
            newBinding.ValidatesOnDataErrors = true;
            textbox.SetBinding(TextBox.TextProperty, newBinding);
 
            if (newBinding.UpdateSourceTrigger == UpdateSourceTrigger.PropertyChanged)
            {
                textbox.TextChanged += OnCoerceText;
            }
            else if (newBinding.UpdateSourceTrigger == UpdateSourceTrigger.LostFocus || newBinding.UpdateSourceTrigger == UpdateSourceTrigger.Default)
            {
                textbox.LostFocus += OnCoerceText;
            }
        }
        #endregion
 
        #region SelectAllOnFocus Attached property
 
        public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnFocus",
            typeof(bool),
            typeof(TextBoxBehavior),
            new PropertyMetadata(false, OnSelectAllOnFocusPropertyChanged));
 
        private static void OnSelectAllOnFocusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                if ((e.NewValue as bool?).GetValueOrDefault(false))
                {
                    textBox.GotKeyboardFocus += OnKeyboardFocusSelectText;
                    textBox.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                }
                else
                {
                    textBox.GotKeyboardFocus -= OnKeyboardFocusSelectText;
                    textBox.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                }
            }
        }
 
        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var orgSource = e.OriginalSource as DependencyObject;
            var textBox = orgSource.FindParent<TextBox>();
            if (textBox == null)
            {
                return;
            }
            if (!textBox.IsKeyboardFocusWithin)
            {
                textBox.Focus();
                e.Handled = true;
            }
        }
 
        private static void OnKeyboardFocusSelectText(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }
 
        [AttachedPropertyBrowsableForChildrenAttribute(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetSelectAllOnFocus(DependencyObject @object)
        {
            return (bool)@object.GetValue(SelectAllOnFocusProperty);
        }
 
        public static void SetSelectAllOnFocus(DependencyObject @object, bool value)
        {
            @object.SetValue(SelectAllOnFocusProperty, value);
        }
 
        #endregion
    }
}