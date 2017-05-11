using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Portal.Models.BusinessLogic
{
    public class CreatePDF
    {
        public void InitializePDF(Document doc, MemoryStream mst)
        {
            PdfWriter writer = PdfWriter.GetInstance(doc, mst);
            writer.CloseStream = false;

            doc.SetPageSize(PageSize.A4);

            doc.Open();
        }

        public PdfPTable CreateTable(int column_size)
        {
            PdfPTable table = new PdfPTable(column_size);
            table.WidthPercentage = 100;
            table.DefaultCell.Padding = 8;

            return table;
        }

        public PdfPTable ImageHeader()
        {
            Image logo = Image.GetInstance(@"C:\pdf_logo.png");
            //Image logo = Image.GetInstance(@"C:\inetpub\wwwroot\pdf_logo.png");

            PdfPTable headerImg = new PdfPTable(1);
            headerImg.WidthPercentage = 30;
            headerImg.DefaultCell.Border = Rectangle.NO_BORDER;
            headerImg.AddCell(logo);

            return headerImg;
        }
    }
}