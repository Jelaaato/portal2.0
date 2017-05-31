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
        public void InitializePDF(Document doc, MemoryStream mst, Rectangle pagesize)
        {
            PdfWriter writer = PdfWriter.GetInstance(doc, mst);
            writer.CloseStream = false;

            doc.SetPageSize(pagesize);

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

        public Font SetFont(string family, int size, int style)
        {
            var customFont = FontFactory.GetFont(family, size, style);
            return customFont;
        }
    }
}