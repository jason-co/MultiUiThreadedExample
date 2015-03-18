using System;
using System.ComponentModel;
using System.Windows;

namespace Core.Presentation.ExtensionMethods
{
   public static class EventHandlerExtensions
   {
      public static bool IsRegistered(this EventHandler handler, Delegate prospectiveHandler)
      {
         return handler.IsEventHandlerRegistered(prospectiveHandler);
      }

      public static bool IsRegistered<T>(this EventHandler<T> handler, Delegate prospectiveHandler) where T : EventArgs
      {
         return handler.IsEventHandlerRegistered(prospectiveHandler);
      }

      public static bool IsRegistered(this DependencyPropertyChangedEventHandler handler, Delegate prospectiveHandler)
      {
         return handler.IsEventHandlerRegistered(prospectiveHandler);
      }

      public static bool IsRegistered(PropertyChangedEventHandler handler, Delegate prospectiveHandler)
      {
         if (handler != null)
         {
            foreach (Delegate existingHandler in handler.GetInvocationList())
            {
               if (existingHandler == prospectiveHandler)
               {
                  return true;
               }
            }
         }
         return handler.IsEventHandlerRegistered(prospectiveHandler);
      }

      private static bool IsEventHandlerRegistered(this MulticastDelegate multDelegate, Delegate prospectiveHandler)
      {
         if (multDelegate != null)
         {
            foreach (Delegate existingHandler in multDelegate.GetInvocationList())
            {
               if (existingHandler == prospectiveHandler)
               {
                  return true;
               }
            }
         }
         return false;
      }
   }
}
