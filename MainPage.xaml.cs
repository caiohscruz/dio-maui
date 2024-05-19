﻿using System.Windows.Input;
using diomaui.Models;
using diomaui.Services;

namespace diomaui;

public partial class MainPage : ContentPage
{

	DatabaseService<Tarefa> _tarefasService;

	public ICommand VerTarefaCommand { get; private set; }

	public MainPage()
	{
		InitializeComponent();
		_tarefasService = new DatabaseService<Tarefa>(Constants.Db.DB_PATH);

		VerTarefaCommand = new Command<Tarefa>((tarefa) =>
		{
			DisplayAlert("Alerta", $"Tarefa: {tarefa.Titulo}", "OK");
		});
		TarefasCollectionTable.BindingContext = this;

		CarregarTarefas();
	}

	private async void CarregarTarefas()
	{
		var tarefas = await _tarefasService.GetAllAsync();
		TarefasCollectionTable.ItemsSource = tarefas;
	}

	private async void OnAddTasksBtnClicked(object sender, EventArgs e)
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

		CarregarTarefas();
	}
}

