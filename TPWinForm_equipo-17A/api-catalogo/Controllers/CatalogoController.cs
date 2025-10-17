using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dominio;
using Microsoft.Ajax.Utilities;
using negocio;
using api_catalogo.Models;

namespace api_catalogo.Controllers
{
    public class CatalogoController : ApiController
    {
        // GET: api/Catalogo
        public IEnumerable<Articulo> Get()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            return negocio.Listar();
        }

        // GET: api/Catalogo/numId
        public IHttpActionResult Get(int id)
        {
            try
            {
                // Verificamos que sea un id válido (mayor a 0)
                if (id <= 0)
                    return BadRequest("ID inválido.");

                ArticuloNegocio negocio = new ArticuloNegocio();

                // Verificar si existe el id
                Articulo articulo = negocio.Listar().Find(a => a.Id == id);
                if (articulo == null)
                    return NotFound();

                // Si todo está correcto, se muestra
                return Ok(articulo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Catalogo
        public IHttpActionResult Post([FromBody] AltaArticuloDTO Articulo)
        {
            //validaciones
            if (Articulo == null)
                return BadRequest("No se recibio informacion del articulo");

            if (string.IsNullOrWhiteSpace(Articulo.Codigo))
                return BadRequest("El campo 'Codigo' es obligatorio.");

            if (string.IsNullOrWhiteSpace(Articulo.Nombre))
                return BadRequest("El campo 'Nombre' es obligatorio.");

            if (string.IsNullOrWhiteSpace(Articulo.Descripcion))
                return BadRequest("El campo 'Descripcion' es obligatorio.");

            if (Articulo.IdMarca <= 0)
                return BadRequest("El campo 'IdMarca' debe ser un número válido.");

            if (Articulo.IdCategoria <= 0)
                return BadRequest("El campo 'IdCategoria' debe ser un número válido.");

            if (Articulo.Precio <= 0)
                return BadRequest("El campo 'Precio' debe ser mayor a cero.");

            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                Articulo nuevo = new Articulo();
                nuevo.Codigo = Articulo.Codigo; 
                nuevo.Nombre = Articulo.Nombre; 
                nuevo.Descripcion = Articulo.Descripcion;   
                nuevo.Marca = new Marca { id = Articulo.IdMarca };  
                nuevo.Categoria = new Categoria { Id = Articulo.IdCategoria };
                nuevo.Precio = Articulo.Precio; 

                negocio.Agregar(nuevo);
                
                return Ok("Articulo agregado correctamente.");  
            }
            catch (Exception ex)
            {
               return InternalServerError(ex);
            }
        }


            // POST: api/Catalogo/AgregarImagenes
            [HttpPost]
            [Route("api/Catalogo/AgregarImagenes")]
            public IHttpActionResult AgregarImagenes([FromBody] AgregarImagenesDTO dto)
            {
                try
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();

                    //Verifica que el articulo existe
                    var articulo = negocio.Listar().Find(a => a.Id == dto.IdArticulo);
                    if (articulo == null)
                        return NotFound();

                    //si todo ok , agrega las imagenes
                    foreach (var url in dto.Imagenes)
                    {
                    if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                        return BadRequest($"La URL '{url}' no es válida.");
                    negocio.AgregarImagen(dto.IdArticulo, url);
                    }

                    return Ok(new { mensaje = "Imágenes agregadas correctamente." });
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }



        // PUT: api/Catalogo/5
        public IHttpActionResult Put(int id, [FromBody] AltaArticuloDTO Articulo)
        {
            //validaciones
            if (Articulo == null)
                return BadRequest("No se recibio informacion del articulo");

            if (string.IsNullOrWhiteSpace(Articulo.Codigo))
                return BadRequest("El campo 'Codigo' es obligatorio.");

            if (string.IsNullOrWhiteSpace(Articulo.Nombre))
                return BadRequest("El campo 'Nombre' es obligatorio.");

            if (string.IsNullOrWhiteSpace(Articulo.Descripcion))
                return BadRequest("El campo 'Descripcion' es obligatorio.");

            if (Articulo.IdMarca <= 0)
                return BadRequest("El campo 'IdMarca' debe ser un número válido.");

            if (Articulo.IdCategoria <= 0)
                return BadRequest("El campo 'IdCategoria' debe ser un número válido.");

            if (Articulo.Precio <= 0)
                return BadRequest("El campo 'Precio' debe ser mayor a cero.");

            try
            {
                ArticuloNegocio negocio = new ArticuloNegocio();
                Articulo nuevo = new Articulo();
                nuevo.Id = id;
                nuevo.Codigo = Articulo.Codigo;
                nuevo.Nombre = Articulo.Nombre;
                nuevo.Descripcion = Articulo.Descripcion;
                nuevo.Marca = new Marca { id = Articulo.IdMarca };
                nuevo.Categoria = new Categoria { Id = Articulo.IdCategoria };
                nuevo.Precio = Articulo.Precio;

                negocio.modificar(nuevo);

                return Ok("Articulo modificado exitosamente.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/Catalogo/numId
        public IHttpActionResult Delete(int id)
        {
            try
            {
                // Verificamos que sea un id válido (mayor a 0)
                if (id <= 0)
                    return BadRequest("ID inválido.");

                ArticuloNegocio negocio = new ArticuloNegocio();

                // Verificar si existe el id
                Articulo articulo = negocio.Listar().Find(a => a.Id == id);
                if (articulo == null)
                    return NotFound();

                // Si todo está correcto, se elimina
                negocio.eliminar(id);
                return Ok($"Producto con ID {id} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
