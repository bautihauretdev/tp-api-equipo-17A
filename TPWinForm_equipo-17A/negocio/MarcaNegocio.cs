using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class MarcaNegocio
    {
        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT Id, Descripcion FROM MARCAS";
                datos.setearConsulta(consulta);
                SqlDataReader lector = datos.ejecutarLectura();


                while (lector.Read())
                {
                    int id = (int)lector["Id"];
                    string descripcion = lector["Descripcion"].ToString();
                    lista.Add(new Marca(id, descripcion));
                }
                datos.cerrarConexion();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Agregar(Marca nueva)
        {
            if (ExisteMarca(nueva.descripcion))
            {
                throw new Exception("La marca ya existe en la base de datos.");
            }  
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta($"INSERT INTO MARCAS (Descripcion) VALUES ('{nueva.descripcion}')");
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
        public void Modificar(Marca marca)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = $"UPDATE MARCAS SET Descripcion = '{marca.descripcion}' WHERE Id = {marca.id}";
                datos.setearConsulta(consulta);
                SqlDataReader lector = datos.ejecutarLectura();

                datos.cerrarConexion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "DELETE FROM MARCAS WHERE Id = @Id";
                datos.setearConsulta(consulta);
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();

                datos.cerrarConexion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ExisteMarca(string descripcion)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT COUNT(*) FROM MARCAS WHERE TRIM(UPPER(Descripcion)) = @descripcion");
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