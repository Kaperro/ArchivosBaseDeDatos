using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivosBaseDeDatos.Models.Entities
{
    public class DocumentoTransferViewModel
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Departamento { get; set; }
    }
}
