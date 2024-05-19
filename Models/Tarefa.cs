using diomaui.Enums;
using SQLite;

namespace diomaui.Models
{
    public class Tarefa
    {

        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int UsuarioId { get; set; }
        public Status Status { get; set; }

        public Tarefa()
        {
            DataCriacao = DateTime.Now;
            DataAtualizacao = DateTime.Now;
        }
    }
}