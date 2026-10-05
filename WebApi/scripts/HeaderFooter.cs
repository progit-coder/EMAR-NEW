using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTCPro.WebApi.scripts
{
    
    /** Inner class to add a header and a footer. */
    internal class HeaderFooter : PdfPageEventHelper
    {
        /** Alternating phrase for the header. */
        Phrase [] header = new Phrase[2];
    /** Current page number (will be reset for every chapter). */
    int pagenumber;

    /**
     * Initialize one of the headers.
     * @see com.itextpdf.text.pdf.PdfPageEventHelper#onOpenDocument(
     *      com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
     */
    public void onOpenDocument(PdfWriter writer, Document document)
    {
        header[0] = new Phrase("Movie history");
    }

    /**
     * Initialize one of the headers, based on the chapter title;
     * reset the page number.
     * @see com.itextpdf.text.pdf.PdfPageEventHelper#onChapter(
     *      com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document, float,
     *      com.itextpdf.text.Paragraph)
     */
    public void onChapter(PdfWriter writer, Document document,
            float paragraphPosition, Paragraph title)
    {
        header[1] = new Phrase(title.Content);
        pagenumber = 1;
    }

    /**
     * Increase the page number.
     * @see com.itextpdf.text.pdf.PdfPageEventHelper#onStartPage(
     *      com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
     */
    public void onStartPage(PdfWriter writer, Document document)
    {
        pagenumber++;
    }

    /**
     * Adds the header and the footer.
     * @see com.itextpdf.text.pdf.PdfPageEventHelper#onEndPage(
     *      com.itextpdf.text.pdf.PdfWriter, com.itextpdf.text.Document)
     */
    public void onEndPage(PdfWriter writer, Document document)
    {
        Rectangle rect = writer.GetBoxSize("art");
        switch (writer.PageNumber % 2)
        {
            case 0:
                ColumnText.ShowTextAligned(writer.DirectContent,
                        Element.ALIGN_RIGHT, header[0],
                        rect.Right, rect.Top, 0);
                break;
            case 1:
                ColumnText.ShowTextAligned(writer.DirectContent,
                        Element.ALIGN_LEFT, header[1],
                        rect.Left, rect.Top, 0);
                break;
        }
        ColumnText.ShowTextAligned(writer.DirectContent,
                Element.ALIGN_CENTER, new Phrase(String.Format("page %d", pagenumber)),
                (rect.Left + rect.Right) / 2, rect.Bottom - 18, 0);

       //     ColumnText.ShowTextAligned(writer.DirectContent,
       //Element.ALIGN_RIGHT, new Phrase("My static header text"),
       //rect.Right, rect.Top, 0);
       //     ColumnText.ShowTextAligned(writer.DirectContent,
       //             Element.ALIGN_CENTER, new Phrase(String.Format("page %d", pagenumber)),
       //             (rect.Left + rect.Right) / 2, rect.Bottom - 18, 0);


        }
}
}