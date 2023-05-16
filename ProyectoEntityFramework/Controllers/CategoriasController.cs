using CursoEntityCore.Datos;
using CursoEntityCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CursoEntityCore.Controllers
{
    public class CategoriasController : Controller
    {
        public readonly ApplicationDbContext _contexto;

        public CategoriasController(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //Consulta inicial con todos los datos
            //List<Categoria> listaCategorias = _contexto.Categoria.ToList();

            //Consulta filtrando por fecha
            //DateTime fechaComparacion = new DateTime(2021, 11, 05);
            //List<Categoria> listaCategorias = _contexto.Categoria.Where(f => f.FechaCreacion >= fechaComparacion).OrderByDescending(f => f.FechaCreacion).ToList();
            //return View(listaCategorias);

            //Seleccionar columnas espefificas
            //var categorias = _contexto.Categoria.Where(n => n.Nombre == "Test 5").Select(n => n).ToList();
            //List<Categoria> listaCategorias = _contexto.Categoria.ToList();

            //Agrupar
            //var listaCategorias = _contexto.Categoria
            //    .GroupBy(c => new { c.Activo })
            //    .Select(c => new { c.Key, Count = c.Count() }).ToList();

            //take y skip
            //List<Categoria> listaCategorias = _contexto.Categoria.Skip(3).Take(2).ToList();

            ////Consultas sql convencioinales
            //var listaCategorias = _contexto.Categoria.FromSqlRaw("select * from categoria where nombre like 'categoría%' and Activo = 1").ToList();

            //Consultas sql convencioinales
            //var listaCategorias = _contexto.Categoria.FromSqlRaw("select * from categoria where nombre like 'categoría%' and Activo = 1").ToList();

            //Interpolacion de string (string interpolation)
            //var id = 31;
            //var categoria = _contexto.Categoria.FromSqlRaw($"select * from categoria where categoria_id = {id}").ToList();
            List<Categoria> listaCategorias = _contexto.Categoria.ToList();
            return View(listaCategorias);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contexto.Categoria.Add(categoria);
                _contexto.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult CrearMultipleOpcion2()
        {
            List<Categoria> categorias = new List<Categoria>();
            for (int i = 0; i < 2; i++)
            {
                categorias.Add(new Categoria { Nombre = Guid.NewGuid().ToString() });
                //_contexto.Categoria.Add(new Categoria { Nombre = Guid.NewGuid().ToString() });
            }
            _contexto.Categoria.AddRange(categorias);
            _contexto.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CrearMultipleOpcion5()
        {
            List<Categoria> categorias = new List<Categoria>();
            for (int i = 0; i < 5; i++)
            {
                categorias.Add(new Categoria { Nombre = Guid.NewGuid().ToString() });
                //_contexto.Categoria.Add(new Categoria { Nombre = Guid.NewGuid().ToString() });
            }
            _contexto.Categoria.AddRange(categorias);
            _contexto.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult VistaCrearMultipleOpcionFormulario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearMultipleOpcionFormulario()
        {
            string categoriasForm = Request.Form["Nombre"];
            var listaCategorias = from val in categoriasForm.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries) select (val);

            List<Categoria> categorias = new List<Categoria>();

            foreach (var categoria in listaCategorias)
            {
                categorias.Add(new Categoria
                {
                    Nombre = categoria
                });
            }
            _contexto.Categoria.AddRange(categorias);
            _contexto.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var categoria = _contexto.Categoria.FirstOrDefault(c => c.Categoria_Id == id);
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contexto.Categoria.Update(categoria);
                _contexto.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [HttpGet]
        public IActionResult Borrar(int? id)
        {
            var categoria = _contexto.Categoria.FirstOrDefault(c => c.Categoria_Id == id);
            _contexto.Categoria.Remove(categoria);
            _contexto.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult BorrarMultiple2()
        {
            IEnumerable<Categoria> categorias = _contexto.Categoria.OrderByDescending(c => c.Categoria_Id).Take(2);
            _contexto.Categoria.RemoveRange(categorias);
            _contexto.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult BorrarMultiple5()
        {
            IEnumerable<Categoria> categorias = _contexto.Categoria.OrderByDescending(c => c.Categoria_Id).Take(5);
            _contexto.Categoria.RemoveRange(categorias);
            _contexto.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
