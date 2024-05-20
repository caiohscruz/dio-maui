using System.Windows.Input;
using diomaui.Models;
using diomaui.Services;

namespace diomaui.Pages;

public partial class TarefaDetalhePage : ContentPage
{
	public Tarefa Tarefa {get; private set;}

	private DatabaseService<Tarefa> _tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);
	public TarefaDetalhePage(Tarefa tarefa)
	{
		InitializeComponent();
		Tarefa = tarefa;
		BindingContext = this;
	}

	private async void OnVoltarClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

	private async void OnEditarClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new TarefaSalvarPage(Tarefa));
	}

	private async void OnDeletarClicked(object sender, EventArgs e)
	{
		await _tarefasService.DeleteAsync(Tarefa);
		await Navigation.PopAsync();
	}
}

