using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoEntityCore.Models
{
    public class ArticuloEtiqueta
    {
        
        public int Articulo_Id { get; set; }      
        public int Etiqueta_Id { get; set; }

        public Articulo Articulo { get; set; }
        public Etiqueta Etiqueta { get; set; }
    }
}
