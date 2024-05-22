using System.Windows.Input;
using diomaui.Models;
using diomaui.Services;

namespace diomaui.Pages;

public partial class TarefaDetalhePage : ContentPage
{
	public Tarefa Tarefa { get; private set; }

	private DatabaseService<Tarefa> _tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);
	private DatabaseService<Comentario> _comentariosService = new DatabaseService<Comentario>(Constants.Db.DB_PATH);

	public TarefaDetalhePage(Tarefa tarefa)
	{
		InitializeComponent();
		Tarefa = tarefa;

		ComentariosCollection.BindingContext = this;
		BindingContext = this;

		UsuarioPicker.ItemsSource = UsuarioService.GetInstance().GetUsuarios();
	}

	//TODO: Após editar, a tela de detalhes não está atualizando
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		Tarefa = await _tarefasService.GetByIdAsync(Tarefa.Id);
		BindingContext = this;

		CarregarComentarios();
	}

	private async void CarregarComentarios()
	{
		ComentariosCollection.ItemsSource = (await _comentariosService.GetAllAsync()).Where(c => c.TarefaId == Tarefa.Id);
	}

	private async void OnVoltarClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}

	private async void OnAdicionarComentarioClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(ComentarioEntry.Text))
		{
			await DisplayAlert("Erro", "Por favor, preencha o campo Comentário", "OK");
			ComentarioEntry.Focus();
			return;
		}
		if (UsuarioPicker.SelectedItem == null)
		{
			await DisplayAlert("Erro", "Por favor, selecione um usuário", "OK");
			UsuarioPicker.Focus();
			return;
		}
		await _comentariosService.InsertAsync(new Comentario
		{
			TarefaId = Tarefa.Id,
			UsuarioId = ((Usuario)UsuarioPicker.SelectedItem).Id,
			Texto = ComentarioEntry.Text
		});
		UsuarioPicker.SelectedItem = null;
		ComentarioEntry.Text = string.Empty;

		CarregarComentarios();
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

