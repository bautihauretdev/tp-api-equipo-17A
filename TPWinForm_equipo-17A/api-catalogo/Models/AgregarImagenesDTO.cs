using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_catalogo.Models
{
    public class AgregarImagenesDTO
    {
        public int IdArticulo { get; set; }
        public List<string> Imagenes { get; set;  }
    }
}