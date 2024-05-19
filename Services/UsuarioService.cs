using diomaui.Models;

namespace diomaui.Services
{
    public class UsuarioService
    {
        private static UsuarioService _usuarioService = new UsuarioService();
        private List<Usuario> _usuarios;
        private UsuarioService()
        {
            _usuarios = [
                new Usuario { Id = 1, Nome = "João" },
                new Usuario { Id = 2, Nome = "Maria" },
                new Usuario { Id = 3, Nome = "José" },
            ];
        }

        public static UsuarioService GetInstance()
        {
            return _usuarioService;
        }

        public List<Usuario> GetUsuarios()
        {            
            return _usuarios;
        }
    }
}