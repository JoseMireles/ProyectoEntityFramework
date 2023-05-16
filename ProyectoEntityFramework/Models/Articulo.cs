using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoEntityCore.Models
{
   
    public class Articulo
    {
       
        public int Articulo_Id { get; set; }       
        public string TituloArticulo { get; set; }       
        public string Descripcion { get; set; }
        [Range(0.1, 5.0)]        
        public double Calificacion { get; set; }       
        public DateTime Fecha { get; set; }
        
        public int Categoria_Id { get; set; }
        public Categoria Categoria { get; set; }
        //Para relacion muchos a muchos
        public ICollection<ArticuloEtiqueta> ArticuloEtiqueta { get; set; }
    }
}
