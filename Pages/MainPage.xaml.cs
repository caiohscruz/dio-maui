using System.Windows.Input;
using diomaui.Enums;
using diomaui.Models;
using diomaui.Services;

namespace diomaui.Pages;

public partial class MainPage : ContentPage
{

	DatabaseService<Tarefa> _tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);	

	public MainPage()
	{
		InitializeComponent();
		
		TarefasEmBacklog.BindingContext = this;
		TarefasEmAnalise.BindingContext = this;
		TarefasParaFazer.BindingContext = this;
		TarefasEmDesenvolvimento.BindingContext = this;
		TarefasFeitas.BindingContext = this;

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

		// Se não tiver tarefas, insere algumas de exemplo
		if(tarefas == null || tarefas.Count == 0)
		{
			var usuarios = UsuarioService.GetInstance().GetUsuarios();
			var random = new Random();

			await _tarefasService.InsertAsync(new Tarefa { Titulo = "Tarefa 1", Status = Status.Backlog, UsuarioId = usuarios[random.Next(usuarios.Count)].Id, Descricao = "Descrição 1" });
			await _tarefasService.InsertAsync(new Tarefa { Titulo = "Tarefa 2", Status = Status.Analise, UsuarioId = usuarios[random.Next(usuarios.Count)].Id, Descricao = "Descrição 2" });
			await _tarefasService.InsertAsync(new Tarefa { Titulo = "Tarefa 3", Status = Status.ParaFazer, UsuarioId = usuarios[random.Next(usuarios.Count)].Id, Descricao = "Descrição 3" });
			await _tarefasService.InsertAsync(new Tarefa { Titulo = "Tarefa 4", Status = Status.Desenvolvimento, UsuarioId = usuarios[random.Next(usuarios.Count)].Id, Descricao = "Descrição 4" });
			await _tarefasService.InsertAsync(new Tarefa { Titulo = "Tarefa 5", Status = Status.Feito, UsuarioId = usuarios[random.Next(usuarios.Count)].Id, Descricao = "Descrição 5" });
			tarefas = await _tarefasService.GetAllAsync();
		}
		
		TarefasEmBacklog.ItemsSource = tarefas.Where(t => t.Status == Status.Backlog);
		TarefasEmAnalise.ItemsSource = tarefas.Where(t => t.Status == Status.Analise);
		TarefasParaFazer.ItemsSource = tarefas.Where(t => t.Status == Status.ParaFazer);
		TarefasEmDesenvolvimento.ItemsSource = tarefas.Where(t => t.Status == Status.Desenvolvimento);
		TarefasFeitas.ItemsSource = tarefas.Where(t => t.Status == Status.Feito);		
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
		var btn = (Button)sender;
		var status = (Status)btn.CommandParameter;
		await Navigation.PushAsync(new TarefaSalvarPage(status));		
	}
}

