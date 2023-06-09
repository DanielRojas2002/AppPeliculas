using AppPeliculas.Models;
using AppPeliculas.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppPeliculas.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly DbpeliculasContext _context;

        public UsuarioController(DbpeliculasContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(Usuario modelo)
        {
           

            int idusuario = (from usu in _context.Usuarios
                               where usu.Correo == modelo.Correo  && usu.Contrasena == modelo.Contrasena && usu.IdTipoUsuario == 1
                             select usu.IdUsuario).SingleOrDefault();

            if (idusuario>0)
            {
                return RedirectToAction(nameof(Menu),new {idusuario=idusuario});
            }
            else
            {
                var errorMessage = "Credenciales Incorrectas";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    asp_action = "Index",
                    asp_controller = "Usuario"

                };

                return View("Error", model);
            }
            
        }

        public IActionResult Menu(int idusuario)
        {
            TempData["idusuario"] = idusuario;
            var peliculas = _context.Peliculas
            .Include(m => m.IdCategoriaNavigation)
            .Where(a => a.Stock > 0 && a.IdEstatusPelicula == 1)
            .ToList()
            .Select((pelicula, indice) => new { Pelicula = pelicula, Indice = indice })
            .GroupBy(x => x.Indice / 3)
            .Select(g => g.Select(x => x.Pelicula).ToList()) // Convertir IEnumerable en List
            .ToList();

            return View(peliculas);
        }

        public IActionResult DetalleProducto(int idusuario,int idpelicula)
        {
          
            

            PeliculaCarrito peliculaCarrito = new PeliculaCarrito()
            {
                pelicula = _context.Peliculas.Include(p => p.IdCategoriaNavigation).FirstOrDefault(p => p.IdPelicula == idpelicula),
                carrito = new Carrito(),
                usuario = new Usuario()
                {
                    IdUsuario=idusuario
                },
                carritodetalle =new CarritoDetalle()

            };

            peliculaCarrito.pelicula = _context.Peliculas.Find(idpelicula);



            TempData["rutaimagen"] = peliculaCarrito.pelicula.Imagen;


            return View(peliculaCarrito);
        }

        [HttpPost]
        public IActionResult DetalleProducto(PeliculaCarrito modelo)
        {
            // agregar al carrito

            // creo un carrito
            // verifico si ya existe un carrito para ese cliente

            int idcarrito = 0;
            try
            {
                idcarrito = (from carritoo in _context.Carritos
                                 where carritoo.IdUsuario == modelo.usuario.IdUsuario
                                 select carritoo.IdCarrito).SingleOrDefault();
            }
            catch
            {
                idcarrito = 0;
            }

            if (idcarrito>0)
            {
                // ya existe un carrito para este cliente

                CarritoDetalle carritoDetalle = new CarritoDetalle()
                {
                    IdCarrito = idcarrito,
                    IdPelicula = modelo.pelicula.IdPelicula,
                    Stock = modelo.carritodetalle.Stock
                };


                Almacen al = new Almacen()
                {
                    IdPelicula = modelo.pelicula.IdPelicula,
                    IdTipoEntrada = 2,
                    Cantidad = modelo.carritodetalle.Stock,
                    FechaRegistro = DateTime.Now
                };

                var stockactual = (from peli in _context.Peliculas
                                   where peli.IdPelicula == modelo.pelicula.IdPelicula
                                   select peli.Stock).SingleOrDefault();

                Pelicula pelicula = new Pelicula()
                {
                    IdPelicula = modelo.pelicula.IdPelicula,
                    Stock = modelo.carritodetalle.Stock - stockactual
                };


                _context.Entry(pelicula).Property(x => x.Stock).IsModified = true;

                _context.SaveChanges();

            }
            else
            {
                // no existe un carrito para este cliente
                Carrito carrito = new Carrito()
                {
                    FechaRegistro = DateTime.Now,
                    IdUsuario = modelo.usuario.IdUsuario

                    //HoraPedido= DateTime.Now, acepte nulos

                    // falta en el sql horapedido acepte nulos
                };

                CarritoDetalle carritoDetalle = new CarritoDetalle()
                {
                    IdCarrito = idcarrito,
                    IdPelicula = modelo.pelicula.IdPelicula,
                    Stock = modelo.carritodetalle.Stock
                };



                Almacen al = new Almacen()
                {
                    IdPelicula = modelo.pelicula.IdPelicula,
                    IdTipoEntrada = 2,
                    Cantidad = modelo.carritodetalle.Stock,
                    FechaRegistro = DateTime.Now
                };


                var stockactual = (from peli in _context.Peliculas
                                   where peli.IdPelicula == modelo.pelicula.IdPelicula
                                   select peli.Stock).SingleOrDefault();

                Pelicula pelicula = new Pelicula()
                {
                    IdPelicula = modelo.pelicula.IdPelicula,
                    Stock = modelo.carritodetalle.Stock - stockactual
                };


                _context.Entry(pelicula).Property(x => x.Stock).IsModified = true;

                _context.SaveChanges();

                

            }

   
            return RedirectToAction("Index");
            
        }
    }
}
