using System.Windows.Input;
using diomaui.Models;
using diomaui.Services;

namespace diomaui.Pages;

public partial class TarefaDetalhePage : ContentPage
{
	public Tarefa Tarefa { get; private set; }

	private DatabaseService<Tarefa> _tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);
	private DatabaseService<Comentario> _comentariosService = new DatabaseService<Comentario>(Constants.Db.DB_PATH);

	private DatabaseService<Anexo> _anexoService = new DatabaseService<Anexo>(Constants.Db.DB_PATH);


	public TarefaDetalhePage(Tarefa tarefa)
	{
		InitializeComponent();
		Tarefa = tarefa;

		// ComentariosCollection.BindingContext = this;
		// FotosCollection.BindingContext = this;
		// LocalizacaoCollection.BindingContext = this;
		BindingContext = this;

		UsuarioPicker.ItemsSource = UsuarioService.GetInstance().GetUsuarios();
	}

	protected override void OnAppearing()
    {
        base.OnAppearing();
        
		LabelTitulo.Text = Tarefa.Titulo;
		LabelNomeUsuario.Text = Tarefa.Usuario.Nome;
		LabelDataCriacao.Text = Tarefa.DataCriacao.ToString();
		LabelDataAtualizacao.Text = Tarefa.DataAtualizacao.ToString();
		LabelStatus.Text = Tarefa.Status.ToString();
		LabelDescricao.Text = Tarefa.Descricao;
        UsuarioPicker.ItemsSource = UsuarioService.GetInstance().GetUsuarios();

		CarregarComentarios();
		CarregarImagens();
		CarregarLocalizacoes();
    }

	private async void CarregarComentarios()
	{
		ComentariosCollection.ItemsSource = await _comentariosService.GetQuery().Where(c => c.TarefaId == Tarefa.Id).ToListAsync();
	}

	private async void CarregarImagens()
	{
		var fotos = await _anexoService.GetQuery().Where(a => a.TarefaId == Tarefa.Id && string.IsNullOrEmpty(a.Arquivo)).ToListAsync();

		if (fotos.Count > 0)
		{
			FotosFrame.IsVisible = true;
			FotosCollection.ItemsSource = fotos;
		}
		else
		{
			FotosFrame.IsVisible = false;
		}

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

	private async Task<bool> CheckAndRequestCameraPermission()
	{
		var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
		if (status != PermissionStatus.Granted)
		{
			status = await Permissions.RequestAsync<Permissions.Camera>();
		}
		return status == PermissionStatus.Granted;
	}

	//TODO: Está retornando false, mesmo com a devida permissão no Manifest
	private async Task<bool> CheckAndRequestStoragePermission()
	{
		var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
		if (status != PermissionStatus.Granted)
		{
			status = await Permissions.RequestAsync<Permissions.StorageWrite>();
		}
		return status == PermissionStatus.Granted;
	}

	private async Task<bool> CheckAndRequestLocalizacaoPermission()
	{
		var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
		if (status != PermissionStatus.Granted)
		{
			status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
		}
		return status == PermissionStatus.Granted;
	}

	//TODO: Não está salvando a foto, não sei se é porque estou usando um emulador...
	private async void TirarFotoClicked(object sender, EventArgs e)
	{
		try
		{
			if (!MediaPicker.Default.IsCaptureSupported)
			{
				await DisplayAlert("Erro", "Captura de foto não suportada", "OK");
				return;
			}

			bool cameraPermissionGranted = await CheckAndRequestCameraPermission();
			bool storagePermissionGranted = await CheckAndRequestStoragePermission();

			if (!cameraPermissionGranted || !storagePermissionGranted)
			{
				await DisplayAlert("Permissões", "Permissões de câmera e armazenamento são necessárias.", "OK");
				return;
			}

			var photo = await MediaPicker.Default.CapturePhotoAsync();
			if (photo == null) return;

			// Cria um stream a partir do arquivo
			using Stream stream = await photo.OpenReadAsync();

			// Define o diretório onde a foto será salva
			var directory = FileSystem.AppDataDirectory;
			var fileName = Path.Combine(directory, $"{DateTime.Now.ToString("ddMMyyyy_hhmmss")}.jpg");

			// Salva a foto no diretório
			using FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
			await stream.CopyToAsync(fs);

			await _anexoService.InsertAsync(new Anexo
			{
				TarefaId = Tarefa.Id,
				Arquivo = fileName
			});
			CarregarImagens();
		}
		catch (FeatureNotSupportedException fnsEx)
		{
			await DisplayAlert("Erro", "A captura de fotos não é suportada nesse dispositivo. - " + fnsEx.Message, "OK");
		}
		catch (PermissionException pEx)
		{
			await DisplayAlert("Erro", "Permissão para acessar a câmera não concedida. - " + pEx.Message, "OK");
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
		}
	}

	public ICommand OpenLocationOnMapsCommand => new Command<Anexo>(async (location) =>
	{
		var url = string.Format("https://www.google.com/maps/search/?api=1&query={0},{1}", location.Latitude, location.Longitude);
		await Launcher.OpenAsync(new Uri(url));
	});

	private async void GPSClicked(object sender, EventArgs e)
	{
		var confirm = await DisplayAlert("GPS", "Confirma a captura da localização?", "Sim", "Não");

		if (!confirm) return;

		LocalizacaoButton.Text = "Carregando...";
		LocalizacaoButton.IsEnabled = false;

		try
		{

			bool locationPermissionGranted = await CheckAndRequestLocalizacaoPermission();
			if (!locationPermissionGranted)
			{
				await DisplayAlert("Permissões", "Permissões de localização são necessárias.", "OK");
				return;
			}

			var request = new GeolocationRequest(GeolocationAccuracy.Medium);
			var location = await Geolocation.GetLocationAsync(request);

			if (location == null)
			{
				await DisplayAlert("Erro", "Não foi possível capturar a localização", "OK");
				return;
			}

			await _anexoService.InsertAsync(new Anexo
			{
				TarefaId = Tarefa.Id,
				Latitude = location.Latitude,
				Longitude = location.Longitude
			});

			CarregarLocalizacoes();

			await DisplayAlert("Localização", $"Latitude: {location.Latitude}, Longitude: {location.Longitude}", "OK");
		}
		catch (FeatureNotSupportedException fnsEx)
		{
			await DisplayAlert("Erro", "GPS não é suportada nesse dispositivo. - " + fnsEx.Message, "OK");
		}
		catch (PermissionException pEx)
		{
			await DisplayAlert("Erro", "Permissão para acessar o GPS não concedida. - " + pEx.Message, "OK");
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
		}
		finally
		{
			LocalizacaoButton.Text = "Pegar as coordenadas do GPS";
			LocalizacaoButton.IsEnabled = true;
		}
	}

	private async void CarregarLocalizacoes()
	{
		var localizacoes = await _anexoService.GetQuery().Where(a => a.TarefaId == Tarefa.Id && a.Latitude != 0 && a.Longitude != 0).ToListAsync();

		if (localizacoes.Count > 0)
		{
			LocalizacaoCollection.ItemsSource = localizacoes;
			LocalizacaoFrame.IsVisible = true;
		}
		else
		{
			LocalizacaoFrame.IsVisible = false;
		}
	}
}

