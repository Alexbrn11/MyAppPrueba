using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Entities.Models;

namespace MyApp.Entities.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        public int ArticuloId { get; set; }
        public Articulo Articulo { get; set; }

        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }

        public DateTime FechaEntrega { get; set; }
        public DateTime FechaDevolucion { get; set; }

        public string Estado { get; set; } // Pendiente, Aprobado, Devuelto, Rechazado
    }

}
