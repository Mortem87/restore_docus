using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace cap_edocument
{
    class DigitalDocument
    {
        public DigitalDocument(
            string p_pedimento
            , int p_patente
            , int p_aduana
            , string p_numero
            , string p_document_name
            , string p_document_name_type
            )
            
        {
            DocumentName = p_document_name;
            Aduana = p_aduana;
            Pedimento = p_pedimento;
            Numero = p_numero;
            Patente = p_patente;
            DocumentNameType = p_document_name_type;
        }
        public DigitalDocument()
        {

        }
        public int Aduana { get; set; }// = 400;
        public string Pedimento { get; set; }// = "1015379";
        public string Numero { get; set; }// = "LS0242";
        public int Patente { get; set; }// = 3621;
        public string DocumentName { get; set; }
        public string DocumentNameType { get; set; }

    }
    class EdocumentBLL
    {
        public static void RestoreInvoicePDF(DigitalDocument edocument) 
        {
            byte[] image_crud = 
                GetInvoice
          (
               edocument.DocumentName// string DocumentName
                , edocument.Aduana// int Aduana// = 400;
                , edocument.Pedimento//string Pedimento// = "1015379";
                , edocument.Numero//string Numero// = "LS0242";
                , edocument.Patente//int Patente// = 3621;
           );
            if (image_crud == null)
            {
                Console.WriteLine(edocument.DocumentName + " No Obtenido.");
            }
            else 
            {
                File.WriteAllBytes(edocument.DocumentName, image_crud);
                Console.WriteLine(edocument.DocumentName + " Obtenido.");
                Console.ReadLine();
            }
        }
        public static void RestoreDocument(DigitalDocument document) 
        {
            switch (document.DocumentNameType) 
            {
                case "PDF de eDocument":
                    RestoreEdocument(document);
                    break;
                case "PDF de factura":
                    RestoreInvoicePDF(document);
                    break;
                default:
                    break;
            }
        }
        public static void RestoreEdocument(DigitalDocument edocument)
        {
            string message = "";

            string EdocumentId = edocument.DocumentName;

            string FullFilePath = GetEdocument
                (
                edocument.DocumentName
                , edocument.Aduana
                , edocument.Pedimento
                , edocument.Numero
                , edocument.Patente
                );
            if (FullFilePath != "")
            {
                string[] direcciones = FullFilePath.Split('\\');

                string rootpath = "\\" + "\\" +
                    direcciones[2] + "\\" +
                    direcciones[3] + "\\" +
                    direcciones[4] + "\\" +
                    direcciones[5] + "\\" +
                    direcciones[6] + "\\" +
                    direcciones[7] + "\\" +
                    direcciones[8] + "\\";

                string fullpedimento = direcciones[8];

                string patente = fullpedimento.Substring(0, 4);

                string aduana = fullpedimento.Substring(4, 3);

                string pedimento = fullpedimento.Substring(7, 7);

                string FullFilePathEdocument = rootpath + EdocumentId + ".PDF";

                string path_xml = direcciones[9];

                Log(rootpath);

                bool EdocumentExist = File.Exists(FullFilePathEdocument);

                if (EdocumentExist)
                {

                    message = EdocumentId + " exist.";

                    Log(message);

                }
                else
                {

                    message = EdocumentId + " not exist.";

                    Log(message);

                    bool Exist = File.Exists(FullFilePath);

                    if (Exist)
                    {

                        message = path_xml + " exist.";

                        Log(message);

                        string file = ReadXML(FullFilePath);

                        byte[] image_crud = Convert.FromBase64String(file);

                        string path = rootpath + EdocumentId + ".PDF";

                        File.WriteAllBytes(path, image_crud);

                        //Pedimento	Patente	Aduana

                        message = pedimento + "\t" + patente + "\t" + aduana;

                        Log(message);
                    }
                    else
                    {
                        message = path_xml + " Not Exist.";

                        Log(message);
                    }
                }
            }
            else 
            {
                message =  edocument.DocumentName + " FullFilePath is empty.";

                Log(message);
            }
            
        }
        public static string ReadXML(string filename)
        {
            string result = "";
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //return only when you have START tag  
                        switch (reader.Name.ToString())
                        {
                            case "dig:archivo":
                                result = reader.ReadString();
                                break;
                            case "Location":
                                Console.WriteLine("Your Location is : " + reader.ReadString());
                                break;
                        }
                    }
                    Console.WriteLine("");
                }
            }
            return result;
        }
        public static byte[] GetInvoice
            (
                /*string Pedimento// = "";//'0002051'
                , int Patente// = 0;//3621
                , int Aduana// = 0;//110
                , string Factura// = "";//2015990
                , string ClienteDarwin
                */
                string DocumentName
                , int Aduana// = 400;
                , string Pedimento// = "1015379";
                , string Numero// = "LS0242";
                , int Patente// = 3621;
            ) 
        {

            //AQUI QUITAR EXTENSION DE ARCHIVO.

            string Factura = DocumentName.Substring(0, DocumentName.Length - 4);

            byte[] image_crud = { 0, 1, 0, 1 };

            string message = "";

            string stringconnection = @"Data Source=DB-SERVER;Initial Catalog=EXPEDIENTES;Integrated Security=SSPI;";//User ID=UserName; Password = Password";

            using (SqlConnection conn = new SqlConnection(stringconnection))
            {
                conn.Open();
                

                    string sql =
        @"
--PDF VERSION--

DECLARE @Version AS int;
--SET @Version =
SELECT 
TOP 10
@Version = ed.PdfVer 
FROM EXPEDIENTES.dbo.ExpedienteDigital ed
WHERE ed.Patente = @Patente
AND ed.Aduana = @Aduana
AND ed.Pedimento = @Pedimento
AND ed.Factura = @Factura
AND ed.Referencia LIKE '%COVE%'
AND ed.ClienteDarwin = @ClienteDarwin
;
--SELECT @Version AS [Version];
DECLARE @CveArchivo AS int;

SELECT 
TOP 10
--*--ed.PdfVer 
@CveArchivo = ed.CveArchivo
FROM EXPEDIENTES.dbo.ExpedienteDigital ed
WHERE ed.Patente = @Patente
AND ed.Aduana = @Aduana
AND ed.Pedimento = @Pedimento
AND ed.Factura = @Factura
AND ed.Tipo = 'FACTURA'
AND ed.Version = @Version
AND ed.ClienteDarwin = @ClienteDarwin
AND ed.SubTipo = 'PDF'
;

--SELECT @CveArchivo AS CveArchivo;

SELECT TOP 1 ea.Archivo FROM EXPEDIENTES.dbo.ExpedienteArchivo ea WHERE ea.CveArchivo = @CveArchivo;
";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {

                    command.Parameters.AddWithValue("@Pedimento", Pedimento);
                    command.Parameters.AddWithValue("@Patente", Patente);
                    command.Parameters.AddWithValue("@Aduana", Aduana);
                    command.Parameters.AddWithValue("@Factura", Factura);
                    command.Parameters.AddWithValue("@ClienteDarwin", Numero);

                    try 
                    {
                        image_crud = (byte[])command.ExecuteScalar();
                    } 
                    catch (Exception ex) 
                    {

                        message = ex.Message;

                        Log(message);

                    }
                }
            }
            return image_crud;
        }
        public static string GetEdocument
            (
                string EdocumentId
                , int Aduana// = 400;
                , string Pedimento// = "1015379";
                , string Numero// = "LS0242";
                , int Patente// = 3621;
            )
        {
            EdocumentId = EdocumentId + ".XML";

            List<DigitalDocument> Edocuments = new List<DigitalDocument>();

            DataTable dtresult = new DataTable();

            string FullFilePath = "";

            string _STRING_CONNECTION = @"Data Source=DB-SERVER;Initial Catalog=EXPEDIENTES;Integrated Security=SSPI;";

            using (SqlConnection conn = new SqlConnection(_STRING_CONNECTION))
            {

                try
                {
                    conn.Open();

                    string sql =
@"
SELECT 
TOP 1000 
dd.DocumentName
, dd.FullFilePath
--dd.* 
FROM DigitalRecords.DBO.DigitalDocuments dd 
WHERE dd.DocumentName = @DocumentName--'0192210KU1II8.XML'
AND dd.DocumentsCollectionId = 
(SELECT 
TOP 1
vp.ID
FROM DARWINTIJ.dbo.VT_Pedimentos vp 
INNER JOIN DARWINTIJ.dbo.VT_Clientes vc ON (vc.ID = vp.Cliente)
WHERE vp.Patente = @Patente--3621
AND vp.Aduana = @Aduana--400
AND vp.Pedimento = @Pedimento--'1015379'
AND vc.Numero = @Numero--'LS0242'
)
;
";
                    

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@DocumentName", EdocumentId);
                    cmd.Parameters.AddWithValue("@Patente", Patente);
                    cmd.Parameters.AddWithValue("@Aduana", Aduana);
                    cmd.Parameters.AddWithValue("@Pedimento", Pedimento);
                    cmd.Parameters.AddWithValue("@Numero", Numero);

                    var adaptador = new SqlDataAdapter(cmd);

                    adaptador.Fill(dtresult);

                    foreach (DataRow row in dtresult.Rows)
                    {
                        //DigitalDocument edocument = new DigitalDocument();
                        if (dtresult.Rows.Count == 1)
                        {
                            FullFilePath = (string)row["FullFilePath"];
                        }
                        else 
                        {
                            FullFilePath = "";
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return FullFilePath;
        }
        public static void Log(string message)
        {//DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt")
            using (StreamWriter sw = new StreamWriter(@"C:\log_cap_edocument.txt", true))
            {
                string time_stamp = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                sw.WriteLine(time_stamp + " " + message);
            }
        }
    }
}
