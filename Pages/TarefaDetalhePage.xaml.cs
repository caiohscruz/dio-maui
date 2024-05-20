using System.Windows.Input;
using diomaui.Models;
using diomaui.Services;

namespace diomaui.Pages;

public partial class TarefaDetalhePage : ContentPage
{
	public Tarefa Tarefa {get; private set;}
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
}

