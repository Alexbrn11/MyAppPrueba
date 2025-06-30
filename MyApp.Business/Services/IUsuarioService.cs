using MyApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Business.Services
{
    public interface IUsuarioService
    {
        Task<Usuario?> Autenticar(string email, string password);
        Task CrearUsuario(Usuario usuario, string password);
        Task CambiarRol(int usuarioId, string nuevoRol);
    }

}
