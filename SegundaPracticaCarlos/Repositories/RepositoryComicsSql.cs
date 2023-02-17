using SegundaPracticaCarlos.Models;
using System.Data;
using System.Data.SqlClient;

namespace SegundaPracticaCarlos.Repositories
{
    #region PROCEDURES SQL
    /*
    ////////////////////////////////////PROCEDURE INSERTAR COMIC////////////////////////////////////
    CREATE PROCEDURE SP_INSERTCOMIC
    (@IDCOMIC INT,
    @NOMBRE NVARCHAR(50),
    @IMAGEN NVARCHAR(50),
    @DESCRIPCION NVARCHAR(50))
    AS
	    INSERT INTO COMICS 
		VALUES(@IDCOMIC, @NOMBRE, @IMAGEN, @DESCRIPCION)
    GO
    */
    #endregion
    public class RepositoryComicsSql : IRepositoryComics
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataAdapter adapter;
        private DataTable tablaComics;

        public RepositoryComicsSql()
        {
            string connectionSql = @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            string sql = "SELECT * FROM COMICS";
            this.adapter = new SqlDataAdapter(sql, connectionSql);
            this.cn = new SqlConnection(connectionSql);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tablaComics = new DataTable();
            this.adapter.Fill(this.tablaComics);
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();

            foreach(var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }

        private int GetMaximoIdComic()
        {
            var maximo = (from datos in this.tablaComics.AsEnumerable()
                          select datos).Max(x => x.Field<int>("IDCOMIC")) + 1;
            return maximo;
        }

        public void InsertComic(string nombre, string imagen, string descripcion)
        {
            int maximo = this.GetMaximoIdComic();
            SqlParameter pamIdComic = new SqlParameter("@IDCOMIC", maximo);
            this.com.Parameters.Add(pamIdComic);

            SqlParameter pamNombre = new SqlParameter("@NOMBRE", nombre);
            this.com.Parameters.Add(pamNombre);

            SqlParameter pamImagen = new SqlParameter("@IMAGEN", imagen);
            this.com.Parameters.Add(pamImagen);

            SqlParameter pamDescripcion = new SqlParameter("@DESCRIPCION", descripcion);
            this.com.Parameters.Add(pamDescripcion);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERTCOMIC";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
