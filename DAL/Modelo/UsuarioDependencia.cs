using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modelo
{
    public class UsuarioDependencia:Generica
    {
        public int UsuarioID { get; set; }
        public int DependenciaID { get; set; }
        public int TipoUsuarioID { get; set; }
        public bool Activo { get; set; }
        public bool Bloqueado { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Dependencia Dependencia { get; set; }
        public virtual TipoUsuario TipoUsuario { get; set; }
    }
}
