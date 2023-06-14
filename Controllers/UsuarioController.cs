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
                             where usu.Correo == modelo.Correo && usu.Contrasena == modelo.Contrasena && usu.IdTipoUsuario == 1
                             select usu.IdUsuario).SingleOrDefault();

            if (idusuario > 0)
            {
                return RedirectToAction(nameof(Menu), new { idusuario = idusuario });
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

        public IActionResult DetalleProducto(int idusuario, int idpelicula)
        {


            TempData["idusuario"] = idusuario;
            PeliculaCarrito peliculaCarrito = new PeliculaCarrito()
            {
                pelicula = _context.Peliculas.Include(p => p.IdCategoriaNavigation).FirstOrDefault(p => p.IdPelicula == idpelicula),
                carrito = new Carrito(),
                usuario = new Usuario()
                {
                    IdUsuario = idusuario
                },
                carritodetalle = new CarritoDetalle()

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
            TempData["idusuario"] = modelo.usuario.IdUsuario;

            int? stockdisponible = (from peli in _context.Peliculas
                                    where peli.IdPelicula == modelo.pelicula.IdPelicula
                                    select peli.Stock).SingleOrDefault();

            if (stockdisponible > modelo.carritodetalle.Stock)
            {
                int idcarrito = 0;
                try
                {
                    idcarrito = (from carritoo in _context.Carritos
                                 where carritoo.IdUsuario == modelo.usuario.IdUsuario && carritoo.IdEstatus==1
                                 select carritoo.IdCarrito).SingleOrDefault();
                }
                catch
                {
                    idcarrito = 0;
                }

                if (idcarrito > 0)
                {
                    // ya existe un carrito para este cliente

                    CarritoDetalle carritoDetalle = new CarritoDetalle()
                    {
                        IdCarrito = idcarrito,
                        IdPelicula = modelo.pelicula.IdPelicula,
                        Stock = modelo.carritodetalle.Stock
                        
                    };

                    _context.CarritoDetalles.Add(carritoDetalle);
                    _context.SaveChanges();



                    var stockactual = (from peli in _context.Peliculas
                                       where peli.IdPelicula == modelo.pelicula.IdPelicula
                                       select peli.Stock).SingleOrDefault();

                    Pelicula pelicula = new Pelicula()
                    {
                        IdPelicula = modelo.pelicula.IdPelicula,
                        Stock = stockactual - modelo.carritodetalle.Stock
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
                        IdUsuario = modelo.usuario.IdUsuario,
                        IdEstatus=1


                    };

                    _context.Carritos.Add(carrito);
                    _context.SaveChanges();

                    idcarrito = (from carritoo in _context.Carritos
                                 where carritoo.IdUsuario == modelo.usuario.IdUsuario && carritoo.IdEstatus == 1
                                 select carritoo.IdCarrito).SingleOrDefault();

                    CarritoDetalle carritoDetalle = new CarritoDetalle()
                    {
                        IdCarrito = idcarrito,
                        IdPelicula = modelo.pelicula.IdPelicula,
                        Stock = modelo.carritodetalle.Stock
                    };

                    _context.CarritoDetalles.Add(carritoDetalle);
                    _context.SaveChanges();

                   


                    var stockactual = (from peli in _context.Peliculas
                                       where peli.IdPelicula == modelo.pelicula.IdPelicula
                                       select peli.Stock).SingleOrDefault();

                    Pelicula pelicula = new Pelicula()
                    {
                        IdPelicula = modelo.pelicula.IdPelicula,
                        Stock = stockactual - modelo.carritodetalle.Stock
                    };


                    _context.Entry(pelicula).Property(x => x.Stock).IsModified = true;
                    _context.SaveChanges();



                }
            }
            else
            {
                var errorMessage = "Contenido no Disponible";

                var model = new ErrorViewModel
                {
                    ErrorMessage = errorMessage,
                    asp_action = "Menu",
                    asp_controller = "Usuario",
                    parametro2 = modelo.usuario.IdUsuario

                };

                return View("Error", model);
            }



            return RedirectToAction(nameof(Menu), new { idusuario = modelo.usuario.IdUsuario });

        }


        public IActionResult Carrito(int idusuario)
        {

            TempData["idusuario"] = idusuario;


            int idcarrito = (from carrito in _context.Carritos
                             where carrito.IdUsuario == idusuario && carrito.IdEstatus==1
                             select carrito.IdCarrito).SingleOrDefault();

            TempData["idcarrito"] = idcarrito;



            var carritodetalle = _context.CarritoDetalles
            .Include(m => m.IdCarritoNavigation).Include(p => p.IdPeliculaNavigation)
            .Where(a => a.IdCarritoNavigation.IdUsuario == idusuario  && a.IdCarritoNavigation.IdEstatus==1)
            .ToList()
            .Select((carritodetalle, indice) => new { CarritoDetalle = carritodetalle, Indice = indice })
            .GroupBy(x => x.Indice / 3)
            .Select(g => g.Select(x => x.CarritoDetalle).ToList()) // Convertir IEnumerable en List
            .ToList();



            return View(carritodetalle);

        }

        public IActionResult Pedidos(int idusuario)
        {


            TempData["idusuario"] = idusuario;




        



            var carritodetalle = _context.CarritoDetalles
            .Include(m => m.IdCarritoNavigation).Include(p => p.IdPeliculaNavigation)
            .Where(a => a.IdCarritoNavigation.IdUsuario == idusuario && a.IdCarritoNavigation.IdEstatus == 2)
            .ToList()
            .Select((carritodetalle, indice) => new { CarritoDetalle = carritodetalle, Indice = indice })
            .GroupBy(x => x.Indice / 2)
            .Select(g => g.Select(x => x.CarritoDetalle).ToList()) // Convertir IEnumerable en List
            .ToList();



            return View(carritodetalle);

        }


        public IActionResult QuitarPelicula(int idcarritodetalle, int idusuario)
        {
            CarritoDetalle carritoDetalle = new CarritoDetalle()
            {
                IdCarritoDetalle = idcarritodetalle
            };

            int stockdevolver = (from carritodetalle in _context.CarritoDetalles
                                 where carritodetalle.IdCarritoDetalle == idcarritodetalle
                                 select carritodetalle.Stock).SingleOrDefault();

            int idpelicula = (from carritodetalle in _context.CarritoDetalles
                              where carritodetalle.IdCarritoDetalle == idcarritodetalle
                              select carritodetalle.IdPelicula).SingleOrDefault();

            int? stockactual = (from peli in _context.Peliculas
                                where peli.IdPelicula == idpelicula
                                select peli.Stock).SingleOrDefault();







            _context.CarritoDetalles.Remove(carritoDetalle);
            _context.SaveChanges();




            Pelicula pelicula = new Pelicula()
            {
                IdPelicula = idpelicula,
                Stock = stockdevolver + stockactual
            };


            _context.Entry(pelicula).Property(x => x.Stock).IsModified = true;
            _context.SaveChanges();



            return RedirectToAction(nameof(Carrito), new { idusuario = idusuario });
        }


        public IActionResult Pagar(int idusuario, int idcarrito)
        {

          

            List<CarritoDetalle> carritoDetalles = _context.CarritoDetalles.Where(p => p.IdCarrito == idcarrito).ToList();


            


            foreach (var person in carritoDetalles)
            {
                // Hacer algo con cada persona, por ejemplo, agregarla a una lista de resultados
                // o mostrar sus propiedades en la vista

                Almacen al = new Almacen()
                {
                    IdPelicula = person.IdPelicula,
                    IdTipoEntrada = 2,
                    Cantidad = person.Stock,
                    FechaRegistro = DateTime.Now
                };

                _context.Almacens.Add(al);
                _context.SaveChanges();



            }

            Carrito carrito = new Carrito()
            {
                IdCarrito=idcarrito,
                IdEstatus=2,
                FechaPedido=DateTime.Now
            };

            _context.Entry(carrito).Property(x => x.IdEstatus).IsModified = true;
            _context.Entry(carrito).Property(x => x.FechaPedido).IsModified = true;
            _context.SaveChanges();


            return RedirectToAction(nameof(Menu), new { idusuario = idusuario });
        }
    }
}
