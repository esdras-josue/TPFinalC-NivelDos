using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace bussines
{
    public class ItemBrand
    {
        public List<Brand> brandList()
        {
			List<Brand>list = new List<Brand>();
			DataAccess data = new DataAccess();
			try
			{
				data.setQuery("select Id, Descripcion from Marcas");
				data.executeRead();

				while (data.Reader.Read())
				{
					Brand aux = new Brand();
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
