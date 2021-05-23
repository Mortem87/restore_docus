using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace sln_cove_pdf
{
    class Program
    {
        private static string _STRING_CONNECTION;
        static void Main(string[] args)
        {
            /*
            const string conn_digital_records_prod = "Server=DB-SERVER;Database=EXPEDIENTES;Integrated Security=True;";

            _STRING_CONNECTION = conn_digital_records_prod;

            byte[] byteArrayIn = ObtenerCoves();
            Image img = byteArrayToImage(byteArrayIn);
            img.Save("myfile.png");
            */

            //byte[] image_crud = ObtenerArchivo();

            int CveArchivo = 37581004;

            byte[] image_crud =
                ObtenerArchivo(CveArchivo);
            File.WriteAllBytes("20E0381.PDF", image_crud);
            Console.WriteLine("PDF Obtenido.");
            Console.ReadLine();

            /*
             * 
             * 26181881
26171053
             var sb = new StringBuilder();

for (int i = 0; i < data[0] - 1; i++)
{
    for (int j = 0; j < 12; j++)
    {
        sb.Append(data[i * 12 + j] + "\t");
    }
    sb.AppendLine();
}
File.WriteAllText("filename.ext", sb.ToString());
             */
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        private static byte[] ObtenerArchivo(int CveArchivo)
        {
            byte[] image_crud = { 0, 1, 0, 1 };

            string stringconnection = @"Data Source=DB-SERVER;Initial Catalog=EXPEDIENTES;Integrated Security=SSPI;";//User ID=UserName; Password = Password";

            using (SqlConnection conn = new SqlConnection(stringconnection))
            {
                conn.Open();
                try
                {

                    string sql =
@"
--SELECT TOP 1 ea.Archivo FROM EXPEDIENTES.dbo.ExpedienteArchivo ea WHERE ea.CveArchivo = @CveArchivo;
 SELECT pdf FROM EFACTURAS.dbo.pdf pdf
  WHERE Factura = '20E0381'
  ;
";

                    
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {

                        //command.Parameters.AddWithValue("@CveArchivo", CveArchivo);
                        image_crud = (byte[])command.ExecuteScalar();
                    }
                }
                catch
                {
                    Console.WriteLine("Count not insert.");
                }
            }
            return image_crud;
        }

        private static void ObtenerArchivoRegla8va()
        {
            byte[] image_crud = { 0, 1, 0, 1 };

            string stringconnection = @"Data Source=AAM-BACKUP-SRVR;Initial Catalog=MainSilaHistorico2016;Integrated Security=SSPI;";//User ID=UserName; Password = Password";

            using (SqlConnection conn = new SqlConnection(stringconnection))
            {
                conn.Open();
                try
                {

                    string sql =
@"

use MainSilaHistorico2018

DECLARE @FolioCliente nvarchar(6)
DECLARE @idCuenta int
set @FolioCliente='JD0036'

set @idCuenta = (select idCuentaMemoOperativo from Empresa.VwCuentaNombreEmpresa where CuentaMemoOperativoFolio =@FolioCliente)


select --TOP 1 
--*
IdDocumentoDigital
, Documento 
from 
Empresa.DocumentoDigital  where idDocumentoDigital in (
SELECT idDocumentoDigital from Empresa.MemoOperativoTipoDocumento 
where idCuentaMemoOperativo=@idCuenta)
and idTipoDocumento=51 and VigenciaInicio>='2018-01-01'
";
                    DataTable reglas8vas = new DataTable();

                    using (SqlCommand command = new SqlCommand(sql, conn)) 
                    {
                        var adaptador = new SqlDataAdapter(command);
                        adaptador.Fill(reglas8vas);
                    }

                    int count = 0;
                    foreach (DataRow regla8va in reglas8vas.Rows) 
                    {
                        count++;
                        image_crud = (byte[])regla8va["Documento"];
                        string file_name = regla8va["IdDocumentoDigital"].ToString();
                        file_name = file_name + "_R8va.pdf";
                        File.WriteAllBytes(file_name, image_crud);
                        Console.WriteLine("PDf Obtenido: " + file_name + " count: " + count);
                    }
                }
                catch
                {
                    Console.WriteLine("Count not insert.");
                }
            }
            //return image_crud;
        }
        public static byte[] ObtenerCoves()
        {
            byte[] result = { 0, 1, 0, 1 };
            string sql = @"SELECT TOP 1  ea.Archivo FROM EXPEDIENTES.dbo.ExpedienteArchivo ea WHERE ea.CveArchivo = 35433493;";
            using (SqlConnection conn = new SqlConnection(_STRING_CONNECTION))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    /*
                    cmd.Parameters.Add("@PedimentoId", SqlDbType.Int);
                    cmd.Parameters["@PedimentoId"].Value = CustomId;


                    cmd.Parameters.Add("@InvoiceId", SqlDbType.Int);
                    cmd.Parameters["@InvoiceId"].Value = InvoiceId;

                    */
                    result = (byte[])cmd.ExecuteScalar();

                }
                catch (Exception e)
                {
                    
                    Console.WriteLine(e.Message);
                }
            }
            return result;
        }
    }
}
