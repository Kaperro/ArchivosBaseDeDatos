using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivosBaseDeDatos.Models
{
    public class RoleModel
    {
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Usuario")]
        public string UserName { get; set; }
    }
}
