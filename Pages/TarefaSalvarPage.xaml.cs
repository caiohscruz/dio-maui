using diomaui.Enums;
using diomaui.Models;
using diomaui.Services;

namespace diomaui.Pages;

public partial class TarefaSalvarPage : ContentPage
{
	DatabaseService<Tarefa> _tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);
	public Tarefa Tarefa { get; private set; }

	public string PageTitle => Tarefa == null ? "Nova Tarefa" : "Editar Tarefa";

	public TarefaSalvarPage(Tarefa tarefa)
	{
		InitializeComponent();
		Tarefa = tarefa;

		StatusPicker.ItemsSource = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
		UsuarioPicker.ItemsSource = UsuarioService.GetInstance().GetUsuarios();

		TituloEntry.Text = Tarefa.Titulo;
		DescricaoEditor.Text = Tarefa.Descricao;
		StatusPicker.SelectedItem = Tarefa.Status;
		UsuarioPicker.SelectedItem = Tarefa.Usuario;

		BindingContext = this;
	}

	public TarefaSalvarPage(Status status)
	{
		InitializeComponent();
		Tarefa = new Tarefa();

		StatusPicker.ItemsSource = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
		UsuarioPicker.ItemsSource = UsuarioService.GetInstance().GetUsuarios();

		StatusPicker.SelectedItem = status;

		BindingContext = this;
	}

	private async void OnSalvarClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(TituloEntry.Text))
		{
			await DisplayAlert("Erro", "Por favor, preencha o campo Título", "OK");
			TituloEntry.Focus();
			return;
		}

		if (string.IsNullOrEmpty(DescricaoEditor.Text))
		{
			await DisplayAlert("Erro", "Por favor, preencha o campo Descrição", "OK");
			DescricaoEditor.Focus();
			return;
		}

		if (StatusPicker.SelectedItem == null)
		{
			await DisplayAlert("Erro", "Por favor, selecione um Status", "OK");
			StatusPicker.Focus();
			return;
		}

		if (UsuarioPicker.SelectedItem == null)
		{
			await DisplayAlert("Erro", "Por favor, selecione um Usuário", "OK");
			UsuarioPicker.Focus();
			return;
		}

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
			Tarefa.DataAtualizacao = DateTime.Now;
			await _tarefasService.UpdateAsync(Tarefa);
		}
		await Navigation.PopAsync();
	}

	private async void OnCancelarClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}

