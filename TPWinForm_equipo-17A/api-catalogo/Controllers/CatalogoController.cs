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

        // GET: api/Catalogo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Catalogo
        public IHttpActionResult Post([FromBody] AltaArticuloDTO Articulo)
        {
        

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

        // PUT: api/Catalogo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Catalogo/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                // Verificamos que sea un id válido
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
