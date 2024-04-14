using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace bussines
{
    public class ItemCategory
    {
        public List<Category> categoryList()
        {
            List<Category> list = new List<Category>();
            DataAccess data = new DataAccess();

            try
            {
                data.setQuery("select Id, Descripcion from CATEGORIAS");
                data.executeRead();

                while (data.Reader.Read())
                {
                    Category aux = new Category();
                    aux.Id = (int)data.Reader["Id"];
                    aux.Descripcion = (string)data.Reader["Descripcion"];

                    list.Add(aux);
                }
                return list;

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
    }
}
