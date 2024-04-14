using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace bussines
{
    public class BussinesItem
    {
        public List<Item> Listart()
        {
            List<Item> list = new List<Item>();
            SqlConnection connection = new SqlConnection();
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            try
            {
                connection.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, M.Descripcion Marca, C.Descripcion Categoria, A.IdMarca,A.IdCategoria,A.Id FROM ARTICULOS A JOIN MARCAS M ON A.IdMarca = M.Id JOIN CATEGORIAS C ON A.IdCategoria = C.Id";
                command.Connection = connection;

                connection.Open();
                reader = command.ExecuteReader();

                while(reader.Read()) 
                {
                    Item aux = new Item();
                    aux.Id = (int)reader["Id"];
                    aux.Code = (string)reader["Codigo"];
                    aux.Name = (string)reader["Nombre"];
                    aux.Description = (string)reader["Descripcion"];
                    aux.Brand = new Brand();
                    aux.Brand.Id = (int)reader["IdMarca"];
                    aux.Brand.Descripcion = (string)reader["Marca"];
                    aux.Category = new Category();
                    aux.Category.Id = (int)reader["IdCategoria"];
                    aux.Category.Descripcion = (string)reader["Categoria"];  
                    if (!(reader["ImagenUrl"] is DBNull))
                        aux.UrlImage = (string)reader["ImagenUrl"];
                    aux.Price = (decimal)reader["Precio"];

                    list.Add(aux);
                }

                connection.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void addItem(Item newItem) 
        {
            DataAccess data = new DataAccess();

            try
            {
                data.setQuery("Insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca,IdCategoria, ImagenUrl,Precio)values('" + newItem.Code +"', '" + newItem.Name+ "', '"+ newItem.Description + "', @IdMarca,@IdCategoria,@ImagenUrl,@Precio)");
                data.setParameter("@IdMarca",newItem.Brand.Id);
                data.setParameter("@IdCategoria", newItem.Category.Id);
                data.setParameter("@ImagenUrl", newItem.UrlImage);
                data.setParameter("@Precio",newItem.Price);
                data.executeAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.closeConnection();
            }
        }
        public void modifyItem (Item modifyItem) 
        {
            DataAccess data = new DataAccess();
            try
            {
                data.setQuery("update ARTICULOS set Codigo = @code, nombre = @name, Descripcion = @description, IdMarca = @idBrand,IdCategoria = @IdCategory, ImagenUrl = @Image, precio = @price where Id = @id");
                data.setParameter("@code",modifyItem.Code);
                data.setParameter("@name", modifyItem.Name);
                data.setParameter("@description", modifyItem.Description);
                data.setParameter("@idBrand", modifyItem.Brand.Id);
                data.setParameter("@IdCategory", modifyItem.Category.Id);
                data.setParameter("@Image", modifyItem.UrlImage);
                data.setParameter("@price", modifyItem.Price);
                data.setParameter("@id", modifyItem.Id);

                data.executeAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.closeConnection();
            }
        }
        public void delete(int id)
        {
            try
            {
                DataAccess data = new DataAccess();
                data.setQuery("delete from ARTICULOS where id = @id");
                data.setParameter("@id", id);
                data.executeAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Item> filterBy(string filter, string criterio, string explore)
        {
            List<Item> list = new List<Item>();
            DataAccess data = new DataAccess();
            try
            {
                string query = "SELECT Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, M.Descripcion Marca, C.Descripcion Categoria, A.IdMarca,A.IdCategoria,A.Id FROM ARTICULOS A JOIN MARCAS M ON A.IdMarca = M.Id JOIN CATEGORIAS C ON A.IdCategoria = C.Id And ";

                if(filter == "Name")
                {
                    switch (criterio)
                    {
                        case "Starts With":
                            query += "Nombre like '" + explore + "%' ";
                            break;
                        case "Ends With":
                            query += "Nombre like '%" + explore + "'";
                            break;
                        default :
                            query += "Nombre like '%" + explore + "%'";
                            break;
                    }
                }
                else if (filter == "Description")
                {
                    switch (criterio)
                    {
                        case "Starts With":
                            query += "A.Descripcion like '" + explore + "%' ";
                            break;
                        case "Ends With":
                            query += "A.Descripcion like '%" + explore + "'";
                            break;
                        default:
                            query += "A.Descripcion like '%" + explore + "%'";
                            break;
                    }
                }
                else
                {
                    switch(criterio) 
                    {
                        case "Bigger than":
                            query += "Precio > " + explore;
                            break;
                        case "Less than":
                            query += "Precio < " + explore;
                            break;
                        case "Equals":
                            query += "Precio = " + explore;
                            break;
                    }
                }

                data.setQuery(query);
                data.executeRead();

                while (data.Reader.Read())
                {
                    Item aux = new Item();
                    aux.Id = (int)data.Reader["Id"];
                    aux.Code = (string)data.Reader["Codigo"];
                    aux.Name = (string)data.Reader["Nombre"];
                    aux.Description = (string)data.Reader["Descripcion"];
                    aux.Brand = new Brand();
                    aux.Brand.Id = (int)data.Reader["IdMarca"];
                    aux.Brand.Descripcion = (string)data.Reader["Marca"];
                    aux.Category = new Category();
                    aux.Category.Id = (int)data.Reader["IdCategoria"];
                    aux.Category.Descripcion = (string)data.Reader["Categoria"];
                    if (!(data.Reader["ImagenUrl"] is DBNull))
                        aux.UrlImage = (string)data.Reader["ImagenUrl"];
                    aux.Price = (decimal)data.Reader["Precio"];

                    list.Add(aux);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }        
        }
    }  
}
