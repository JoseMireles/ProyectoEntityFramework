using CursoEntityCore.Datos;
using CursoEntityCore.Models;
using CursoEntityCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CursoEntityCore.Controllers
{
    public class ArticulosController : Controller
    {
        public readonly ApplicationDbContext _contexto;

        public ArticulosController(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //Ópción 1 sin datos relacionados (solo trae el ID de la categoría)
            //List<Articulo> listaArticulos= _contexto.Articulo.ToList();

            //foreach (var articulo in listaArticulos)
            //{
            //    //Opción 2: carga manual se generan muchas consultas SQL, no es eficiente si necesitamos cargar
            //    //más propiedades
            //    //articulo.Categoria = _contexto.Categoria.FirstOrDefault(c => c.Categoria_Id == articulo.Categoria_Id);
            //    //Opción 3: Carga explícita (Explicit loading)
            //    _contexto.Entry(articulo).Reference(c => c.Categoria).Load();
            //}

            //Opción 4: CARGA DILIGENTE (Eager Loading)
            List<Articulo> listaArticulos = _contexto.Articulo.Include(c => c.Categoria).ToList();
            return View(listaArticulos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ArticuloCategoriaVM articuloCategorias = new ArticuloCategoriaVM();
            articuloCategorias.ListaCategorias = _contexto.Categoria.Select(i => new SelectListItem
            {
                Text = i.Nombre,
                Value = i.Categoria_Id.ToString()
            });


            return View(articuloCategorias);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                _contexto.Articulo.Add(articulo);
                _contexto.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            //Para que al retornar la vista por algun error, también retorne la lista de categorías
            ArticuloCategoriaVM articuloCategorias = new ArticuloCategoriaVM();
            articuloCategorias.ListaCategorias = _contexto.Categoria.Select(i => new SelectListItem
            {
                Text = i.Nombre,
                Value = i.Categoria_Id.ToString()
            });

            return View(articuloCategorias);
        }


        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)
            {
                return View();
            }

            //Para que al retornar la vista por algun error, también retorne la lista de categorías
            ArticuloCategoriaVM articuloCategorias = new ArticuloCategoriaVM();
            articuloCategorias.ListaCategorias = _contexto.Categoria.Select(i => new SelectListItem
            {
                Text = i.Nombre,
                Value = i.Categoria_Id.ToString()
            });

            articuloCategorias.Articulo = _contexto.Articulo.FirstOrDefault(c => c.Articulo_Id == id);
            if (articuloCategorias == null)
            {
                return NotFound();
            }

            return View(articuloCategorias);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(ArticuloCategoriaVM articuloVM)
        {
            if (articuloVM.Articulo.Articulo_Id == 0)
            {
                return View(articuloVM.Articulo);
            }
            else
            {
                _contexto.Articulo.Update(articuloVM.Articulo);
                _contexto.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Borrar(int? id)
        {
            var articulo = _contexto.Articulo.FirstOrDefault(c => c.Articulo_Id == id);
            _contexto.Articulo.Remove(articulo);
            _contexto.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AdministrarEtiquetas(int id)
        {
            ArticuloEtiquetaVM articuloEtiquetas = new ArticuloEtiquetaVM
            {
                ListaArticuloEtiquetas = _contexto.ArticuloEtiqueta.Include(e => e.Etiqueta).Include(a => a.Articulo)
                .Where(a => a.Articulo_Id == id),

                ArticuloEtiqueta = new ArticuloEtiqueta()
                {
                    Articulo_Id = id
                },
                Articulo = _contexto.Articulo.FirstOrDefault(a => a.Articulo_Id == id)
            };

            List<int> listaTemporalEtiquetasArticulo = articuloEtiquetas.ListaArticuloEtiquetas.Select(e => e.Etiqueta_Id).ToList();
            //Obtener todas las etiquetas cuyos id's no estén en la listaTemporalEtiquetasArticulo
            //Crear un NOT IN usando LINQ
            var listaTemporal = _contexto.Etiqueta.Where(e => !listaTemporalEtiquetasArticulo.Contains(e.Etiqueta_Id)).ToList();

            //Crear lista de etiquetas para el dropdown
            articuloEtiquetas.ListaEtiquetas = listaTemporal.Select(i => new SelectListItem
            {
                Text = i.Titulo,
                Value = i.Etiqueta_Id.ToString()
            });

            return View(articuloEtiquetas);
        }

        [HttpPost]
        public IActionResult AdministrarEtiquetas(ArticuloEtiquetaVM articuloEtiquetas)
        {
            if (articuloEtiquetas.ArticuloEtiqueta.Articulo_Id != 0 && articuloEtiquetas.ArticuloEtiqueta.Etiqueta_Id != 0)
            {
                _contexto.ArticuloEtiqueta.Add(articuloEtiquetas.ArticuloEtiqueta);
                _contexto.SaveChanges();
            }
            return RedirectToAction(nameof(AdministrarEtiquetas), new {@id = articuloEtiquetas .ArticuloEtiqueta.Articulo_Id});
        }

        [HttpPost]
        public IActionResult EliminarEtiquetas(int idEtiqueta, ArticuloEtiquetaVM articuloEtiquetas)
        {
            int idArticulo = articuloEtiquetas.Articulo.Articulo_Id;
            ArticuloEtiqueta articuloEtiqueta = _contexto.ArticuloEtiqueta.FirstOrDefault(
                    u => u.Etiqueta_Id == idEtiqueta && u.Articulo_Id == idArticulo
                );

                _contexto.ArticuloEtiqueta.Remove(articuloEtiqueta);
                _contexto.SaveChanges();
           
            return RedirectToAction(nameof(AdministrarEtiquetas), new { @id = idArticulo });
        }
    }
}
