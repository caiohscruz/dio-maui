using System.Windows.Input;
using diomaui.Models;
using diomaui.Services;

namespace diomaui.Pages;

public partial class MainPage : ContentPage
{

	DatabaseService<Tarefa> _tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);	

	public MainPage()
	{
		InitializeComponent();
		
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
	
	public ICommand NavigateToDetailCommand => new Command<Tarefa>(async (tarefa) =>
	{
		await Navigation.PushAsync(new TarefaDetalhePage(tarefa));
	});

	public ICommand TarefaDeleteCommand => new Command<Tarefa>(async (tarefa) =>
	{
		await _tarefasService.DeleteAsync(tarefa);
		CarregarTarefas();
	});
	private async void OnAddTasksBtnClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new TarefaSalvarPage());		
	}
}

