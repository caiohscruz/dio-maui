using diomaui.Models;
using diomaui.Services;

namespace diomaui;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		var tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);

		var tarefa = new Tarefa
		{
			Titulo = "Nova tarefa",
			Descricao = "Descrição da tarefa",
			Status = Enums.Status.Backlog
		};

		await tarefasService.InsertAsync(tarefa);
		
		var count = await tarefasService.CountAllAsync();

		if (count == 1)
			CounterBtn.Text = $"{count} tarefa";
		else
			CounterBtn.Text = $"{count} tarefas";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

