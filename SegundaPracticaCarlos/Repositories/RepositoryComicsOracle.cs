using Oracle.ManagedDataAccess.Client;
using SegundaPracticaCarlos.Models;
using System.Data;

namespace SegundaPracticaCarlos.Repositories
{
    #region PROCEDURES ORACLE
    /*
    ////////////////////////////////////PROCEDURE INSERTAR COMIC////////////////////////////////////
    CREATE OR REPLACE PROCEDURE SP_INSERT_COMIC
    (
    P_IDCOMIC COMICS.IDCOMIC%TYPE,
    P_NOMBRE COMICS.NOMBRE%TYPE,
    P_IMAGEN COMICS.IMAGEN%TYPE,
    P_DIRECCION COMICS.DESCRIPCION%TYPE
    )
    AS
    BEGIN
      INSERT INTO COMICS VALUES
             (P_IDCOMIC, P_NOMBRE, P_IMAGEN, P_DIRECCION);
      COMMIT;
    END;
    */
    #endregion
    public class RepositoryComicsOracle: IRepositoryComics
    {
        private OracleConnection cn;
        private OracleCommand com;
        private OracleDataAdapter adapter;
        private DataTable tablaComics;

        public RepositoryComicsOracle()
        {
            string connectionSql = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;User Id=SYSTEM;Password=oracle";
            string sql = "SELECT * FROM COMICS";
            this.adapter = new OracleDataAdapter(sql, connectionSql);
            this.cn = new OracleConnection(connectionSql);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            this.tablaComics = new DataTable();
            this.adapter.Fill(this.tablaComics);
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();

            foreach (var row in consulta)
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
            OracleParameter pamIdComic = new OracleParameter("P_IDCOMIC", maximo);
            this.com.Parameters.Add(pamIdComic);

            OracleParameter pamNombre = new OracleParameter("P_NOMBRE", nombre);
            this.com.Parameters.Add(pamNombre);

            OracleParameter pamImagen = new OracleParameter("P_IMAGEN", imagen);
            this.com.Parameters.Add(pamImagen);

            OracleParameter pamDescripcion = new OracleParameter("P_DESCRIPCION", descripcion);
            this.com.Parameters.Add(pamDescripcion);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC";
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
