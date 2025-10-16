using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = @"SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, M.Descripcion as Marca, A.IdCategoria, C.Descripcion as Categoria, 
                                    A.Precio, 
                                    (SELECT TOP 1 ImagenUrl FROM IMAGENES I WHERE I.IdArticulo = A.Id) as ImagenUrl
                                    FROM ARTICULOS A 
                                    JOIN MARCAS M ON A.IdMarca = M.Id 
                                    JOIN CATEGORIAS C ON A.IdCategoria = C.Id";

                datos.setearConsulta(consulta);
                SqlDataReader lector = datos.ejecutarLectura();

                while (lector.Read())
                {
                    Articulo articulo = new Articulo();
                    articulo.Id = (int)lector["Id"];
                    articulo.Codigo = lector["Codigo"].ToString();
                    articulo.Nombre = lector["Nombre"].ToString();
                    articulo.Descripcion = lector["Descripcion"].ToString();
                    articulo.Precio = (decimal)lector["Precio"];
                    articulo.IdMarca = (int)lector["IdMarca"];
                    articulo.Marca = new Marca((int)lector["IdMarca"], lector["Marca"].ToString());
                    articulo.IdCategoria = (int)lector["IdCategoria"];
                    articulo.Categoria = new Categoria((int)lector["IdCategoria"], lector["Categoria"].ToString());
                    articulo.ImagenUrl = lector["ImagenUrl"] != DBNull.Value ? lector["ImagenUrl"].ToString() : null;

                    AccesoDatos datosImagenes = new AccesoDatos();
                    datosImagenes.setearConsulta("SELECT Id, ImagenUrl FROM IMAGENES WHERE IdArticulo = @IdArticulo");
                    datosImagenes.setearParametro("@IdArticulo", articulo.Id);
                    SqlDataReader lectorImg = datosImagenes.ejecutarLectura();

                    while (lectorImg.Read())
                    {
                        articulo.Imagenes.Add(new Imagen(
                            (int)lectorImg["Id"],
                            articulo.Id,
                            lectorImg["ImagenUrl"].ToString()
                        ));
                    }
                    datosImagenes.cerrarConexion();

                    lista.Add(articulo);
                }
                datos.cerrarConexion();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) VALUES (@Codigo, @Nombre, @Descripcion, @Precio, @IdMarca, @IdCategoria)");
                datos.setearParametro("@Codigo", nuevo.Codigo);
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Descripcion", nuevo.Descripcion);
                datos.setearParametro("@Precio", nuevo.Precio);

                if (nuevo.Marca == null)
                    throw new ArgumentNullException("Marca", "El objeto Marca no puede ser null.");
                datos.setearParametro("@IdMarca", nuevo.Marca.id);

                if (nuevo.Categoria == null)
                    throw new ArgumentNullException("Categoria", "El objeto Categoria no puede ser null.");
                datos.setearParametro("@IdCategoria", nuevo.Categoria.Id);

                datos.ejecutarAccion();

                int idArticulo = ObtenerIdPorCodigo(nuevo.Codigo);
                if (nuevo.Imagenes != null && nuevo.Imagenes.Count > 0)
                {
                    foreach (var img in nuevo.Imagenes)
                    {
                        AgregarImagen(idArticulo, img.ImagenURL);
                    }
                }

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

        public void eliminar (int Id)
        {
            try 
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM ARTICULOS WHERE Id = @Id");
                datos.setearParametro("@Id", Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void modificar (Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE ARTICULOS SET Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio, IdMarca = @IdMarca, IdCategoria = @IdCategoria WHERE Id = @Id");
                datos.setearParametro("@Codigo", articulo.Codigo);
                datos.setearParametro("@Nombre", articulo.Nombre);
                datos.setearParametro("@Descripcion", articulo.Descripcion);
                datos.setearParametro("@Precio", articulo.Precio);
                datos.setearParametro("@IdMarca", articulo.Marca.id);
                datos.setearParametro("@IdCategoria", articulo.Categoria.Id);
                datos.setearParametro("@Id", articulo.Id);
                datos.ejecutarAccion();

                //elimina imágenes existentes
                AccesoDatos datosImg = new AccesoDatos();
                datosImg.setearConsulta("DELETE FROM IMAGENES WHERE IdArticulo = @IdArticulo");
                datosImg.setearParametro("@IdArticulo", articulo.Id);
                datosImg.ejecutarAccion();
                datosImg.cerrarConexion();

                if (articulo.Imagenes != null && articulo.Imagenes.Count > 0)
                {
                    foreach (var img in articulo.Imagenes)
                    {
                        AgregarImagen(articulo.Id, img.ImagenURL);
                    }
                }
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
        public int ObtenerIdPorCodigo(string codigo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id FROM ARTICULOS WHERE Codigo = @Codigo");
                datos.setearParametro("@Codigo", codigo);
                SqlDataReader lector = datos.ejecutarLectura();
                if (lector.Read())
                {
                    return (int)lector["Id"];
                }
                return -1;
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
        public void AgregarImagen(int idArticulo, string imagenUrl)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@IdArticulo, @ImagenUrl)");
                datos.setearParametro("@IdArticulo", idArticulo);
                datos.setearParametro("@ImagenUrl", imagenUrl);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar la imagen: " + ex.Message);
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void ModificarImagen(int idArticulo, string nuevaImagenUrl)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Verifica si hay imagen
                datos.setearConsulta("SELECT TOP 1 Id FROM IMAGENES WHERE IdArticulo = @IdArticulo");
                datos.setearParametro("@IdArticulo", idArticulo);
                var lector = datos.ejecutarLectura();
                bool existeImagen = lector.Read();
                datos.cerrarConexion();

                if (existeImagen)
                {
            
                    datos = new AccesoDatos();
                    datos.setearConsulta("UPDATE IMAGENES SET ImagenUrl = @ImagenUrl WHERE Id = (SELECT TOP 1 Id FROM IMAGENES WHERE IdArticulo = @IdArticulo)");
                    datos.setearParametro("@ImagenUrl", nuevaImagenUrl);
                    datos.setearParametro("@IdArticulo", idArticulo);
                    datos.ejecutarAccion();
                }
                else
                {
          
                    datos = new AccesoDatos();
                    datos.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@IdArticulo, @ImagenUrl)");
                    datos.setearParametro("@IdArticulo", idArticulo);
                    datos.setearParametro("@ImagenUrl", nuevaImagenUrl);
                    datos.ejecutarAccion();
                }
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
    }
    
}
