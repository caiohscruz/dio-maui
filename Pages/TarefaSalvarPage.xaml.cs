using diomaui.Enums;
using diomaui.Models;
using diomaui.Services;

namespace diomaui.Pages;

public partial class TarefaSalvarPage : ContentPage
{
	DatabaseService<Tarefa> _tarefasService;
	public Tarefa Tarefa { get; private set; }

	public string PageTitle => Tarefa == null ? "Nova Tarefa" : "Editar Tarefa";
	public TarefaSalvarPage(Tarefa tarefa = null)
	{
		_tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);
		InitializeComponent();
		Tarefa = tarefa;

		if (Tarefa != null)
		{
			TituloEntry.Text = Tarefa.Titulo;
			DescricaoEditor.Text = Tarefa.Descricao;
			StatusPicker.SelectedItem = Tarefa.Status;
			UsuarioPicker.SelectedItem = UsuarioService.GetInstance().GetUsuarios().Find(u => u.Id == Tarefa.UsuarioId);
		}

		StatusPicker.ItemsSource = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
		UsuarioPicker.ItemsSource = UsuarioService.GetInstance().GetUsuarios();
		
		BindingContext = this;
	}

	private async void OnSalvarClicked(object sender, EventArgs e)
	{
		if (Tarefa == null) Tarefa = new Tarefa();

		Tarefa.Titulo = TituloEntry.Text;
		Tarefa.Descricao = DescricaoEditor.Text;
		Tarefa.Status = (Status)StatusPicker.SelectedItem;
		Tarefa.UsuarioId = ((Usuario)UsuarioPicker.SelectedItem).Id;

		if (Tarefa.Id == 0)
		{
			await _tarefasService.InsertAsync(Tarefa);
		}
		else
		{
			await _tarefasService.UpdateAsync(Tarefa);
		}
		await Navigation.PopAsync();
	}

	private async void OnCancelarClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}

