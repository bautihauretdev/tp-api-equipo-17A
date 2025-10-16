using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id, Descripcion FROM CATEGORIAS");
                SqlDataReader lector = datos.ejecutarLectura();

                while (lector.Read())
                {
                    Categoria cat = new Categoria();
                    cat.Id = (int)lector["Id"];
                    cat.Descripcion = lector["Descripcion"].ToString();
                    lista.Add(cat);
                }

                return lista;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Agregar(Categoria nueva)
        {
            if (ExisteCategoria(nueva.Descripcion))
            {
                throw new Exception("La categoría ya existe.");
            }

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta($"INSERT INTO CATEGORIAS (Descripcion) VALUES ('{nueva.Descripcion}')");
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Eliminar(int Id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM CATEGORIAS WHERE Id = @Id");
                datos.setearParametro("@Id", Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(Categoria cat)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE CATEGORIAS SET Descripcion = @Descripcion WHERE Id = @Id");
                datos.setearParametro("@Id", cat.Id);
                datos.setearParametro("@Descripcion", cat.Descripcion);

                datos.ejecutarAccion();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public bool ExisteCategoria(string descripcion)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM CATEGORIAS WHERE TRIM(UPPER(Descripcion)) = @descripcion");
                datos.setearParametro("@descripcion", descripcion.Trim().ToUpper());
                var lector = datos.ejecutarLectura();
                if (lector.Read())
                {
                    int cantidad = (int)lector[0];
                    return cantidad > 0;
                }
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
   
