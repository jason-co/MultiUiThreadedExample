using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using Core.Presentation.ExtensionMethods;
using Expression = System.Linq.Expressions.Expression;

namespace Core.Presentation.Bases
{
   public class ViewModelBase : INotifyPropertyChanged
   {
      #region private fields

      private event PropertyChangedEventHandler _propertyChanged;

      #endregion

      #region public properties

      public event PropertyChangedEventHandler PropertyChanged
      {
         add
         {
            if (!EventHandlerExtensions.IsRegistered(_propertyChanged, value))
            {
               _propertyChanged += value;
            }
            else
            {
               if (Debugger.IsAttached)
               {
                  // For the developer, for some reason you are hooking into the same PropertyChanged event with the same delegate multiple times.
                  //  The 'IsRegistered' check avoided hooking into this delegate, but you should be aware that your logic is doing something unnecessary.
                  Debugger.Break();
               }
            }
         }
         remove { _propertyChanged -= value; }
      }

      #endregion

      #region protected methods

      protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
      {
         if (Equals(storage, value) || String.IsNullOrEmpty(propertyName))
         {
            return false;
         }

         storage = value;
         OnPropertyChanged(propertyName);
         return true;
      }

      protected void SetProperty<T>(ref T refValue, T value, params Expression<Func<object>>[] propertyExprs)
      {
         if (Equals(value, refValue))
         {
            return;
         }
         refValue = value;
         foreach (var propertyExpr in propertyExprs)
         {
            OnPropertyChanged(propertyExpr);
         }
      }

      protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         var eventHandler = _propertyChanged;
         if (eventHandler != null)
         {
            eventHandler(this, new PropertyChangedEventArgs(propertyName));
         }
      }

      protected virtual void OnPropertyChanged<TPropertyType>(Expression<Func<TPropertyType>> propertyExpr)
      {
         string propertyName = GetPropertySymbol(propertyExpr);

         if (_propertyChanged != null)
         {
            _propertyChanged(this, new PropertyChangedEventArgs(propertyName));
         }
      }

      protected virtual void OnPropertiesChanged(params Expression<Func<object>>[] propertyExprs)
      {
         foreach (var propertyExpr in propertyExprs)
         {
            OnPropertyChanged(propertyExpr);
         }
      }

      protected string GetPropertySymbol<T>(Expression<Func<T>> expr)
      {
         Expression exprBody = expr.Body;
         if (exprBody is UnaryExpression)
         {
            exprBody = ((UnaryExpression)expr.Body).Operand;
         }

         return ((MemberExpression)exprBody).Member.Name;
      }

      protected void BeginUpdateUI(Action action)
      {
         if (!Application.Current.Dispatcher.CheckAccess())
            Application.Current.Dispatcher.BeginInvoke(action);
         else
            action();
      }

      protected void UpdateUi(Action action)
      {
         if (Application.Current == null) return;
         if (!Application.Current.Dispatcher.CheckAccess())
            Application.Current.Dispatcher.Invoke(action);
         else
            action();
      }

      #endregion
   }
}
