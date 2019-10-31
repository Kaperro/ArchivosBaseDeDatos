using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivosBaseDeDatos.Models.Entities
{
    public class DocumentoRegistro
    {
        public long Id { get; set; }
        public long Documento { get; set; }
        public string Departamento { get; set; }
        public DateTime TiempoInicio { get; set; }
        public DateTime TiempoFin { get; set; }
        public string Usuario { get; set; }

        public virtual Documento DocumentoNavigation { get; set; }
    }
}
