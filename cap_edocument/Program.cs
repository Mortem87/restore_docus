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

    class Program
    {
        static void Main(string[] args)
        {
            List<DigitalDocument> documents = new List<DigitalDocument>();

            /*
            edocuments.Add(new Edocument("0072259", 3621, 400, "AA0641", "0192200HZ2VI8"));
            edocuments.Add(new Edocument("1015379", 3621, 400, "LS0242", "0192210KU1II8"));
            edocuments.Add(new Edocument("1015379", 3621, 400, "LS0242", "04282108D5281"));
            edocuments.Add(new Edocument("1016465", 3621, 402, "AA0156", "0192210KU9JQ3"));
            */
            /*
            documents.Add(new DigitalDocument("1006194", 3621, 400, "LSMAUN", "0438210JPC057", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1006195", 3621, 400, "LSMAUN", "0438210JPFJ83", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1012688", 3621, 400, "LSMAUN", "0438210JVMJ86", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1012690", 3621, 400, "LSMAUN", "0438210JVMXR2", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1014171", 3621, 400, "LSMAUN", "0438210JYFN48", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1014175", 3621, 400, "LSMAUN", "0438210JYF921", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1014449", 3621, 400, "LSMAUN", "0438210JZZUV4", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1014450", 3621, 400, "LSMAUN", "0438210JZZT27", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1014451", 3621, 400, "LSMAUN", "0438210JZZOV5", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1014471", 3621, 400, "LSMAUN", "0438210JZZRB7", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1014476", 3621, 400, "LSMAUN", "0438210JZZ755", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1016024", 3621, 400, "LSMAUN", "0438210K2XKU2", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1016027", 3621, 400, "LSMAUN", "0438210K2XFS1", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1017237", 3621, 400, "LSMAUN", "0438210KA2ZR3", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1017238", 3621, 400, "LSMAUN", "0438210KA2WT1", "PDF de eDocument"));
            documents.Add(new DigitalDocument("1004739", 3621, 400, "AA1127", "0436200BWQPK5", "PDF de eDocument"));
            */

            //
            //documents.Add(new DigitalDocument("0002051", 3621, 110, "LS0241", "2015971.PDF", "PDF de factura"));

            documents.Add(new DigitalDocument("1000443", 3621, 400, "AA0792", "ECOR-180451.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000488", 3621, 110, "LS0241", "21E0092.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000706", 3621, 110, "LS0241", "21E0102A.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1009940", 3621, 400, "AA1045", "FD21E3F1-D41F-4F61-90E2-5B9369C88DB1.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000802", 3621, 110, "LS0241", "21E0116.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1011068", 3621, 400, "AA0792", "21-13448-0.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000509", 3621, 110, "AA1339", "IN-2001398.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000762", 3621, 110, "AA1339", "IN-2001571.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1010749", 3621, 400, "AA1048", "efb54a10-0d88-41b9-839b-c7ae3b2703ee.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000826", 3621, 110, "LS0241", "21E0115.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1011350", 3621, 400, "AA0984", "387E6CF6-D28A-4B51-BAE3-7DE2DD9A9139.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1001438", 3621, 400, "AA0827", "IM-29492.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000758", 3621, 110, "LS0241", "21E0108.PDF", "PDF de factura"));
            //documents.Add(new DigitalDocument("1000803", 3621, 110, "LS0241", "21E0113.PDF", "PDF de factura"));//<---revisar este caso, si hay factura en ftp.
            documents.Add(new DigitalDocument("1000867", 3621, 110, "LS0241", "21E0123.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000864", 3621, 110, "LS0241", "21E0112.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000865", 3621, 110, "LS0241", "21E0117A.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02289-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02290-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02291-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02292-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02293-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02294-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02296-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02299-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02300-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02301-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02302-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02303-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02304-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02305-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02306-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02307-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1029085", 3621, 400, "AA1157", "V1E-PT-02308-CH.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1000635", 3621, 110, "LS0241", "21E0096.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1032626", 3621, 402, "AA0792", "22-15673-0FDX.PDF", "PDF de factura"));
            documents.Add(new DigitalDocument("1032718", 3621, 400, "AA0156", "SEI51114.PDF", "PDF de factura"));

            /*
                edocuments.Add(new Edocument("1006194",	3621,	400,	"LSMAUN",	"0438210JPC057"));
                edocuments.Add(new Edocument("1006195",	3621,	400,	"LSMAUN",	"0438210JPFJ83"));
                edocuments.Add(new Edocument("1012688",	3621,	400,	"LSMAUN",	"0438210JVMJ86"));
                edocuments.Add(new Edocument("1012690",	3621,	400,	"LSMAUN",	"0438210JVMXR2"));
                edocuments.Add(new Edocument("1014171",	3621,	400,	"LSMAUN",	"0438210JYFN48"));
                edocuments.Add(new Edocument("1014175",	3621,	400,	"LSMAUN",	"0438210JYF921"));
                edocuments.Add(new Edocument("1014449",	3621,	400,	"LSMAUN",	"0438210JZZUV4"));
                edocuments.Add(new Edocument("1014450",	3621,	400,	"LSMAUN",	"0438210JZZT27"));
                edocuments.Add(new Edocument("1014451",	3621,	400,	"LSMAUN",	"0438210JZZOV5"));
                edocuments.Add(new Edocument("1014471",	3621,	400,	"LSMAUN",	"0438210JZZRB7"));
                edocuments.Add(new Edocument("1014476",	3621,	400,	"LSMAUN",	"0438210JZZ755"));
                edocuments.Add(new Edocument("1016024",	3621,	400,	"LSMAUN",	"0438210K2XKU2"));
                edocuments.Add(new Edocument("1016027",	3621,	400,	"LSMAUN",	"0438210K2XFS1"));
                edocuments.Add(new Edocument("1017237",	3621,	400,	"LSMAUN",	"0438210KA2ZR3"));
                edocuments.Add(new Edocument("1017238",	3621,	400,	"LSMAUN",	"0438210KA2WT1"));
                edocuments.Add(new Edocument("1004739",	3621,	400,	"AA1127",	"0436200BWQPK5"));

             */

            foreach (DigitalDocument document in documents) 
            {
                Console.WriteLine(document.DocumentName);
                
                EdocumentBLL.RestoreDocument(document);

            }
            Console.ReadLine();
        }
    }
}
