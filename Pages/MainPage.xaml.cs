using System.Windows.Input;
using diomaui.Models;
using diomaui.Services;

namespace diomaui.Pages;

public partial class MainPage : ContentPage
{

	DatabaseService<Tarefa> _tarefasService;

	public ICommand NavigateToDetailCommand { get; private set; }

	public MainPage()
	{
		InitializeComponent();
		_tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);

		NavigateToDetailCommand = new Command<Tarefa>(async (tarefa) =>
		{
			await Navigation.PushAsync(new TarefaDetalhePage(tarefa));
		});
		TarefasCollectionTable.BindingContext = this;

		CarregarTarefas();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		CarregarTarefas();
	}

	private async void CarregarTarefas()
	{
		var tarefas = await _tarefasService.GetAllAsync();
		TarefasCollectionTable.ItemsSource = tarefas;
	}

	private async void OnAddTasksBtnClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new TarefaSalvarPage());		
	}
}

