using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivosBaseDeDatos.Utils
{
    public class SystemRoles
    {
        public const string Administrator = "Administrador";
        public const string Member = "Miembro";
        public const string AdministratorOrMember = Administrator + "," + Member;
    }
}
