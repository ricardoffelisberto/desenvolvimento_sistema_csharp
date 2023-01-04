using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CamadaDados
{
    public class DCategoria
    {
        private int _Idcategoria;
        private string _Nome;
        private string _Descricao;
        private string _TextoBuscar;

        public int Idcategoria { get => _Idcategoria; set => _Idcategoria = value; }
        public string Nome { get => _Nome; set => _Nome = value; }
        public string Descricao { get => _Descricao; set => _Descricao = value; }
        public string TextoBuscar { get => _TextoBuscar; set => _TextoBuscar = value; }

        //Construtor vazio
        public DCategoria() { 
        }

        //Construtor com parâmetros
        public DCategoria(int idcategoria, string nome, string descricao, string textobuscar) {
            this.Idcategoria = idcategoria;
            this.Nome = nome;
            this.Descricao = descricao;
            this.TextoBuscar = textobuscar;
        }

        //Metodo Inserir
        public string Inserir(DCategoria Categoria) {
            String resp = "";
            SqlConnection SqlCon = new SqlConnection();
            try {
                SqlCon.ConnectionString = Conexao.Cn;
                SqlCon.Open();

                SqlCommand SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlCon;
                SqlCmd.CommandText = "spinserir_categoria";
                SqlCmd.CommandType = CommandType.StoredProcedure;
                
                SqlParameter ParIdCategoria = new SqlParameter();
                ParIdCategoria.ParameterName = "@idcategoria";
                ParIdCategoria.SqlDbType = SqlDbType.Int;
                ParIdCategoria.Direction = ParameterDirection.Output;
                SqlCmd.Parameters.Add(ParIdCategoria);

                SqlParameter ParNome = new SqlParameter();
                ParNome.ParameterName = "@nome";
                ParNome.SqlDbType = SqlDbType.VarChar;
                ParNome.Size = 50;
                ParNome.Value = Categoria.Nome;
                SqlCmd.Parameters.Add(ParNome);

                SqlParameter ParDescricao = new SqlParameter();
                ParDescricao.ParameterName = "@descricao";
                ParDescricao.SqlDbType = SqlDbType.VarChar;
                ParDescricao.Size = 100;
                ParDescricao.Value = Categoria.Descricao;
                SqlCmd.Parameters.Add(ParDescricao);

                resp = SqlCmd.ExecuteNonQuery() == 1 ? "Ok" : "Esse registro não foi inserido.";

            } catch (Exception ex) {
                resp = ex.Message;

            } finally {
                if (SqlCon.State == ConnectionState.Open) {
                    SqlCon.Close();
                }
            }
            return resp;
        }

        //Metodo Editar
        public string Editar(DCategoria Categoria) {
            String resp = "";
            SqlConnection SqlCon = new SqlConnection();
            try {
                SqlCon.ConnectionString = Conexao.Cn;
                SqlCon.Open();

                SqlCommand SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlCon;
                SqlCmd.CommandText = "speditar_categoria";
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter ParIdCategoria = new SqlParameter();
                ParIdCategoria.ParameterName = "@idcategoria";
                ParIdCategoria.SqlDbType = SqlDbType.Int;
                ParIdCategoria.Value = Categoria.Idcategoria;
                SqlCmd.Parameters.Add(ParIdCategoria);

                SqlParameter ParNome = new SqlParameter();
                ParNome.ParameterName = "@nome";
                ParNome.SqlDbType = SqlDbType.VarChar;
                ParNome.Size = 50;
                ParNome.Value = Categoria.Nome;
                SqlCmd.Parameters.Add(ParNome);

                SqlParameter ParDescricao = new SqlParameter();
                ParDescricao.ParameterName = "@descricao";
                ParDescricao.SqlDbType = SqlDbType.VarChar;
                ParDescricao.Size = 100;
                ParDescricao.Value = Categoria.Descricao;
                SqlCmd.Parameters.Add(ParDescricao);

                resp = SqlCmd.ExecuteNonQuery() == 1 ? "Ok" : "Esse registro não foi editado.";

            } catch (Exception ex) {
                resp = ex.Message;

            } finally {
                if (SqlCon.State == ConnectionState.Open) {
                    SqlCon.Close();
                }
            }
            return resp;
        }

        //Metodo Excluir
        public string Excluir(DCategoria Categoria) {
            String resp = "";
            SqlConnection SqlCon = new SqlConnection();
            try {
                SqlCon.ConnectionString = Conexao.Cn;
                SqlCon.Open();

                SqlCommand SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlCon;
                SqlCmd.CommandText = "spdeletar_categoria";
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter ParIdCategoria = new SqlParameter();
                ParIdCategoria.ParameterName = "@idcategoria";
                ParIdCategoria.SqlDbType = SqlDbType.Int;
                ParIdCategoria.Value = Categoria.Idcategoria;
                SqlCmd.Parameters.Add(ParIdCategoria);

                resp = SqlCmd.ExecuteNonQuery() == 1 ? "Ok" : "Essa exclusão não foi feita.";

            } catch (Exception ex) {
                resp = ex.Message;

            } finally {
                if (SqlCon.State == ConnectionState.Open) {
                    SqlCon.Close();
                }
            }
            return resp;
        }

        //Metodo Mostrar
        public DataTable Mostrar() {
            DataTable DtResultado = new DataTable("categoria");
            SqlConnection SqlCon = new SqlConnection();
            try {
                SqlCon.ConnectionString = Conexao.Cn;
                SqlCommand SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlCon;
                SqlCmd.CommandText = "spmostrar_categoria";
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter SqlDat = new SqlDataAdapter(SqlCmd);
                SqlDat.Fill(DtResultado);

            } catch (Exception ex) {
                DtResultado = null;
            }
            return DtResultado;
        }

        //Metodo Buscar nome
        public DataTable BuscarNome(DCategoria Categoria) {
            DataTable DtResultado = new DataTable("categoria");
            SqlConnection SqlCon = new SqlConnection();
            try {
                SqlCon.ConnectionString = Conexao.Cn;
                SqlCommand SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlCon;
                SqlCmd.CommandText = "spbuscar_nome";
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter SqlDat = new SqlDataAdapter(SqlCmd);
                SqlDat.Fill(DtResultado);

                SqlParameter ParTextoBuscar = new SqlParameter();
                ParTextoBuscar.ParameterName = "@textobuscar";
                ParTextoBuscar.SqlDbType = SqlDbType.VarChar;
                ParTextoBuscar.Size = 50;
                ParTextoBuscar.Value = Categoria.Descricao;
                SqlCmd.Parameters.Add(ParTextoBuscar);

            } catch (Exception ex) {
                DtResultado = null;
            }
            return DtResultado;
        }
    }
}
