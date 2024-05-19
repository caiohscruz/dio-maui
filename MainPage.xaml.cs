using diomaui.Models;
using diomaui.Services;

namespace diomaui;

public partial class MainPage : ContentPage
{

	DatabaseService<Tarefa> _tarefasService;
	public MainPage()
	{
		InitializeComponent();
		_tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);
		CarregarTarefas();
	}

	private async void CarregarTarefas()
	{
		var tarefas = await _tarefasService.GetAllAsync();
		TarefasCollection.ItemsSource = tarefas;
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{

		var random = new Random();
		var statusValues = Enum.GetValues(typeof(Enums.Status));
		var randomStatus = (Enums.Status)statusValues.GetValue(random.Next(statusValues.Length));

		var usuarios = UsuarioService.GetInstance().GetUsuarios();
		var randomUsuarioId = random.Next(usuarios.Count + 1);

		var tarefa = new Tarefa
		{
			Titulo = "Nova tarefa",
			Descricao = "Descrição da tarefa",
			Status = randomStatus,
			UsuarioId = randomUsuarioId == usuarios.Count ? 0 : usuarios[randomUsuarioId].Id
		};

		await _tarefasService.InsertAsync(tarefa);

		var count = await _tarefasService.CountAllAsync();

		if (count == 1)
			CounterBtn.Text = $"{count} tarefa";
		else
			CounterBtn.Text = $"{count} tarefas";

		CarregarTarefas();

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

