using diomaui.Enums;
using diomaui.Services;
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

        [Ignore]
        public Usuario Usuario { get {
            return UsuarioService.GetInstance().GetUsuarios().Find(u => u.Id == this.UsuarioId);            
        } }

        public Status Status { get; set; }

        public Tarefa()
        {
            DataCriacao = DateTime.Now;
            DataAtualizacao = DateTime.Now;
        }
    }
}