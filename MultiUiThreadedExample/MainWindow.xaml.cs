using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace MultiUiThreadedExample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ButtonLockScreen_OnClick( object sender, RoutedEventArgs e )
		{
			Thread.Sleep( 5000 );
		}

		private void MainWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			var k = test;
		}
	}
}
