using System.Windows;

namespace MultiUiThreadedExample
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override void OnStartup( StartupEventArgs e )
		{
			var window = new MainWindow
			{
				DataContext = new MainWindowViewModel()
			};

			window.Show();
		}
	}
}
