using System;
using System.Windows;
using System.Windows.Media;

namespace MultiUiThreadedExample.CustomVisuals
{
	public class VisualTargetPresentationSource : PresentationSource, IDisposable
	{
		private readonly VisualTarget _visualTarget;

		public VisualTargetPresentationSource( HostVisual hostVisual )
		{
			_visualTarget = new VisualTarget( hostVisual );
			AddSource();
		}

		public override Visual RootVisual
		{
			get
			{
				try
				{
					return _visualTarget.RootVisual;
				}
				catch ( Exception )
				{
					return null;
				}
			}

			set
			{
				Visual oldRoot = _visualTarget.RootVisual;

				// Set the root visual of the VisualTarget.  This visual will
				// now be used to visually compose the scene.
				_visualTarget.RootVisual = value;

				// Tell the PresentationSource that the root visual has
				// changed.  This kicks off a bunch of stuff like the
				// Loaded event.
				RootChanged( oldRoot, value );

				// Kickoff layout...
				UIElement rootElement = value as UIElement;
				if ( rootElement != null )
				{
					rootElement.Measure( new Size( Double.PositiveInfinity, Double.PositiveInfinity ) );
					rootElement.Arrange( new Rect( rootElement.DesiredSize ) );
				}
			}
		}

		protected override CompositionTarget GetCompositionTargetCore()
		{
			return _visualTarget;
		}


		private bool _isDisposed;
		public override bool IsDisposed
		{
			get { return _isDisposed; }
		}

		public void Dispose()
		{
			RemoveSource();
			_isDisposed = true;
		}
	}
}
