using diomaui.Services;
using SQLite;

namespace diomaui.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public DateTime Data { get; set; }
        public int TarefaId { get; set; }

        public int UsuarioId { get; set; }

        [Ignore]
        public Usuario Usuario { get {
            return UsuarioService.GetInstance().GetUsuarios().Find(u => u.Id == this.UsuarioId);            
        } }

        public Comentario()
        {
            Data = DateTime.Now;
        }
    }
}