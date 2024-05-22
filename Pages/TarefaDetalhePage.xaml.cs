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

	//TODO: Após editar, a tela de detalhes não está atualizando
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		Tarefa = await _tarefasService.GetByIdAsync(Tarefa.Id);
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
		bool confirm = await DisplayAlert("Deletar", "Deseja realmente deletar a tarefa?", "Sim", "Não");
		if (!confirm) return;
		
		await _tarefasService.DeleteAsync(Tarefa);
		await Navigation.PopAsync();
	}
}

