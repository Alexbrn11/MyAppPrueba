using MyApp.DataAccess.Repositories;
using MyApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace MyApp.Business.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repo;

        public UsuarioService(IGenericRepository<Usuario> repo) => _repo = repo;

        public async Task<Usuario?> Autenticar(string email, string password)
        {
            var usuario = (await _repo.GetAll()).FirstOrDefault(u => u.Email == email);
            if (usuario != null && BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                return usuario;
            return null;
        }

        public async Task CrearUsuario(Usuario usuario, string password)
        {
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            await _repo.Add(usuario);
            await _repo.Save();
        }

        public async Task CambiarRol(int usuarioId, string nuevoRol)
        {
            var usuario = await _repo.GetById(usuarioId);
            if (usuario != null)
            {
                usuario.Rol = nuevoRol;
                _repo.Update(usuario);
                await _repo.Save();
            }
        }
    }

}
