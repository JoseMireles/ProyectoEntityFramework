using System.ComponentModel.DataAnnotations;

namespace CursoEntityCore.Models
{
    public class Etiqueta
    {
       
        public int Etiqueta_Id { get; set; }
        public string Titulo { get; set; }       
        public DateTime Fecha { get; set; }

        public ICollection<ArticuloEtiqueta> ArticuloEtiqueta { get; set; }
    }
}
