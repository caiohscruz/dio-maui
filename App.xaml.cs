namespace diomaui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell(); // muda toda a página toda
		//MainPage = new NavigationPage(new AppShell()); // permite ter um menu superior fixo para várias telas
		// repare na parte de cima durante a transição de páginas

	}
}
