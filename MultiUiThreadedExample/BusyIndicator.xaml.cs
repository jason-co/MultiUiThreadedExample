using System;
using System.Windows;
using System.Windows.Media;

namespace MultiUiThreadedExample
{
	public partial class BusyIndicator
	{
		#region Fields

		#endregion

		#region Construction

		public BusyIndicator()
		{
			InitializeComponent();

			Hide();
		}

		#endregion

		#region Events

		#endregion

		#region Properties

		#region IsBusy (Dependency Property)

		public static readonly DependencyProperty IsBusyProperty =
			DependencyProperty.Register( "IsBusy", typeof( bool ), typeof( BusyIndicator ),
					new PropertyMetadata( false, new PropertyChangedCallback( OnIsBusyChanged ) ) );

		public bool IsBusy
		{
			get { return (bool)GetValue( IsBusyProperty ); }
			set { SetValue( IsBusyProperty, value ); }
		}

		private static void OnIsBusyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			( (BusyIndicator)d ).OnIsBusyChanged( e );
		}

		private void OnIsBusyChanged( DependencyPropertyChangedEventArgs e )
		{
			if ( IsBusy )
			{
				Show();
			}
			else
			{
				Hide();
			}
		}

		#endregion

		#region BusyMessage (Dependency Property)
		public static readonly DependencyProperty BusyMessageProperty =
		  DependencyProperty.Register( "BusyMessage", typeof( string ), typeof( BusyIndicator ),
			new PropertyMetadata( String.Empty ) );

		public string BusyMessage
		{
			get { return (string)GetValue( BusyMessageProperty ); }
			set { SetValue( BusyMessageProperty, value ); }
		}
		#endregion

		#region IsCurtainEnabled (Dependency Property)
		public static readonly DependencyProperty IsCurtainEnabledProperty =
		  DependencyProperty.Register( "IsCurtainEnabled", typeof( bool ), typeof( BusyIndicator ),
			new PropertyMetadata( true ) );

		public bool IsCurtainEnabled
		{
			get { return (bool)GetValue( IsCurtainEnabledProperty ); }
			set { SetValue( IsCurtainEnabledProperty, value ); }
		}
		#endregion

		#region Curtain (Dependency Property)
		public static readonly DependencyProperty CurtainProperty =
			DependencyProperty.Register( "Curtain", typeof( Brush ), typeof( BusyIndicator ),
										new PropertyMetadata(
											new SolidColorBrush( (Color)ColorConverter.ConvertFromString( "#66000000" ) ) ) );

		public Brush Curtain
		{
			get { return (Brush)GetValue( CurtainProperty ); }
			set { SetValue( CurtainProperty, value ); }
		}
		#endregion

		#endregion

		#region Public Methods

		public void Show()
		{
			if ( this.Visibility == Visibility.Collapsed )
			{
				this.Visibility = Visibility.Visible;
				VisualStateManager.GoToState( this, "Busy", false );
			}
		}

		public void Hide()
		{
			if ( this.Visibility == Visibility.Visible )
			{
				VisualStateManager.GoToState( this, "Idle", false );
				this.Visibility = Visibility.Collapsed;
			}
		}

		#endregion

		#region Event Handlers / Delegates

		#endregion

		#region Overrides

		#endregion

		#region Private Implementation

		#endregion
	}
}