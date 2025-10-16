using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using dominio;
using Microsoft.Ajax.Utilities;
using negocio;

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
        public void Post([FromBody]string value)
        {
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
