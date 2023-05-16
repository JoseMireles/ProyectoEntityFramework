using CursoEntityCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CursoEntityCore.ViewModels
{
    public class ArticuloCategoriaVM
    {
        public Articulo Articulo { get; set; }
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }
    }
}
