using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace MultiUiThreadedExample
{
	public class FrozenProcessControl : UiThreadSeparatedControl
	{
		private const double PollingIntervalInMs = 500;
		private const double NonResponseWaitIntervalInMs = 1000;

		private readonly Process _mainWindowProcess;

		private readonly Timer _pollMainWindowTimer;
		private readonly Timer _nonResponseTimer;

		private bool _isContentDisplaying;

		public FrozenProcessControl()
		{
			_mainWindowProcess = Process.GetCurrentProcess();
			_pollMainWindowTimer = new Timer( PollingIntervalInMs );
			_pollMainWindowTimer.Elapsed += PollMainWindowTimerOnElapsed;

			_nonResponseTimer = new Timer( NonResponseWaitIntervalInMs );
			_nonResponseTimer.Elapsed += NonResponseTimer_Elapsed;

			_pollMainWindowTimer.Start();
		}

		#region overrides

		protected override void CreateThreadSeparatedElement()
		{
			base.CreateThreadSeparatedElement();

			_threadSeparatedDispatcher.BeginInvoke( DispatcherPriority.Render, new Action( () =>
			{
				_uiContent.Visibility = Visibility.Hidden;
			} ) );
		}

		protected override FrameworkElement CreateUiContent()
		{
			return new BusyIndicator
			{
				IsBusy = true,
				HorizontalAlignment = HorizontalAlignment.Center
			};
		}

		#endregion

		#region event handlers

		private void PollMainWindowTimerOnElapsed( object sender, ElapsedEventArgs elapsedEventArgs )
		{
			_pollMainWindowTimer.Stop();
			_nonResponseTimer.Start();
			if ( _mainWindowProcess.Responding )
			{
				_nonResponseTimer.Stop();
				if ( _isContentDisplaying )
				{
					_isContentDisplaying = false;

					_threadSeparatedDispatcher.BeginInvoke( DispatcherPriority.Render, new Action( () =>
					{
						_uiContent.Visibility = Visibility.Hidden;
						_pollMainWindowTimer.Start();
					} ) );
				}
				else
				{
					_pollMainWindowTimer.Start();
				}
			}
		}

		private void NonResponseTimer_Elapsed( object sender, ElapsedEventArgs e )
		{
			_pollMainWindowTimer.Stop();
			_nonResponseTimer.Stop();

			_isContentDisplaying = true;

			_threadSeparatedDispatcher.BeginInvoke( DispatcherPriority.Render, new Action( () =>
			{
				_uiContent.Visibility = Visibility.Visible;
			} ) );
		}

		#endregion
	}
}
