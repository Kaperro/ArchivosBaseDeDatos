using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArchivosBaseDeDatos.Models.Entities
{
    public partial class Documento
    {
        public Documento()
        {
            DocumentoRegistro = new HashSet<DocumentoRegistro>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Archivo64 { get; set; }
        public string ArchivoNombre { get; set; }
        public string Departamento { get; set; }
        [DisplayName("Archivo")]
        [NotMapped]
        public IFormFile ArchivoHelper { get; set; }
        public DateTime FechaCreado { get; set; }
        public DateTime FechaRevisado { get; set; }
        public string Usuario { get; set; }
        public string Destinatario { get; set; }

        public virtual ICollection<DocumentoRegistro> DocumentoRegistro { get; set; }
    }
}