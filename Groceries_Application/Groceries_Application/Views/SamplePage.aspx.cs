using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using DataLayer;
using System.IO;
using Path = System.IO.Path;


namespace Groceries_Application.Views
{
    public partial class SamplePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string FileName { get; set; }
        protected void Submit1_OnServerClick(object sender, EventArgs e)
        {

            if (!FileUpload1.HasFile)
            {
                lblMessage.Text = "Please select Pdf files";
            }
            else
            {
                var hfc = Request.Files;

                var hpf = hfc[0];
                var sFileExt = System.IO.Path.GetExtension(hpf.FileName);

                if (hpf.ContentLength > 0 && (sFileExt == ".pdf" || sFileExt == ".PDF"))
                {
                    if (File.Exists(Server.MapPath("CopyFiles\\") + System.IO.Path.GetFileName(hpf.FileName)))
                    {
                        FileName = hpf.FileName;
                        lblMessage.Text = "Already file has been uploaded";
                    }
                    else
                    {
                        hpf.SaveAs(Server.MapPath("CopyFiles\\") + Path.GetFileName(hpf.FileName));
                        var b =
                            ExtractTextFromPdfPage(Server.MapPath("CopyFiles\\") + Path.GetFileName(hpf.FileName));
                        if (b) lblMessage.Text = "Successfully Uploaded.. " + Path.GetFileName(hpf.FileName);
                        else
                        {
                            File.Delete(Server.MapPath("CopyFiles\\") + Path.GetFileName(hpf.FileName));

                            lblMessage.Text = "Not valid.. ( " + Path.GetFileName(hpf.FileName) +
                                              ") Please select appropriate vendor files ";
                        }
                    }
                }
                else
                {
                    lblMessage.Text = "Please select Pdf files";
                }

            }
        }

        protected void Submit2_OnServerClick(object sender, EventArgs e)
        {
            if (FileUpload12.Items.Count == 0)
            {
                Label1.Text = "Please select Pdf files";
            }
            else
            {
                var hfc = FileUpload12.Items;

                if (hfc.Count > 10) Label1.Text = "Files Not more than 10";
                for (var i = 0; i <= hfc.Count - 1; i++)
                {
                    var hpf = hfc[i];
                    var sFileExt = System.IO.Path.GetExtension(hpf.FileName);

                    if ((sFileExt == ".pdf" || sFileExt == ".PDF" || sFileExt == ""))
                    {
                        if (File.Exists(Server.MapPath("CopyFiles\\") + System.IO.Path.GetFileName(hpf.FileName)))
                        {
                            FileName = hpf.FileName;
                            Label1.Text = "Already file has been uploaded";
                        }
                        else
                        {
                            hpf.CopyTo(Server.MapPath("CopyFiles\\") + Path.GetFileName(hpf.FileName));
                            var b =
                                ExtractTextFromPdfPage(Server.MapPath("CopyFiles\\") + Path.GetFileName(hpf.FileName));
                            if (b)
                            {
                                Label1.Text = "Successfully Uploaded..";
                            }
                            else
                            {
                                File.Delete(Server.MapPath("CopyFiles\\") + Path.GetFileName(hpf.FileName));

                                Label1.Text = "Not valid.. Please select appropriate vendor files ";
                            }
                        }
                    }
                    else
                    {
                        Label1.Text = "Please select Pdf files";
                    }
                }
                FileUpload12.DeleteAllAttachments();
            }
        }

        private static bool ExtractTextFromPdfPage(string p)
        {
            var cnn = DbUtility.GetConnection();
            var reader = new PdfReader(p);
            var text = string.Empty;
            for (var page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();
            var editedText = text.Split('\n');

            if (editedText.First().Contains("Page: 1") && editedText.Last().Contains("TOTAL QUANTITY:"))
            {

                using (cnn = new SqlConnection(cnn.ConnectionString))
                {
                    cnn.Open();
                    //var netinvoice = "";
                    var str = "";
                    var tableFormat1 = new PdfTableFormat1();

                    foreach (var s in editedText)
                    {
                        using (
                            var cmd =
                                new SqlCommand(
                                    "insert into SavePDFTable " +
                                    "(FileName,InvoiceNo,InvoiceDate,OrderNo,CustomerNo,Item,Description,Unit,Shipped,Price,Amount)" +
                                    "values(@filename,@invoiceno,@invoicedate,@orderno,@customername,convert(VARCHAR(max), @item),@desc,@unit,@shipped,@price,@amount)",
                                    cnn))
                        {
                            if (!(s.Split().Contains("UNIT") && s.Split().Contains("#")))
                            {
                                if (s.Split().Contains("BAG") || s.Split().Contains("CASE") || s.Split().Contains("EACH") ||
                                s.Split().Contains("BAGS") || s.Split().Contains("UNIT"))
                                {
                                    if (s.Split().Contains("BAG"))
                                    {
                                        str = "BAG";
                                    }
                                    else if (s.Split().Contains("CASE"))
                                    {
                                        str = "CASE";
                                    }
                                    else if (s.Split().Contains("EACH"))
                                    {
                                        str = "EACH";
                                    }
                                    else if (s.Split().Contains("BAGS"))
                                    {
                                        str = "BAGS";
                                    }
                                    else if (s.Split().Contains("UNIT"))
                                    {
                                        str = "UNIT";
                                    }
                                    var code1 = s.Split(new[] { str }, StringSplitOptions.None)[0];
                                    var code2 = s.Split(new[] { str }, StringSplitOptions.None)[1];
                                    tableFormat1.FileName = p.Split('\\').Last();
                                    cmd.Parameters.AddWithValue("@item", s.Split()[0]);
                                    cmd.Parameters.AddWithValue("@filename", tableFormat1.FileName);
                                    cmd.Parameters.AddWithValue("@invoiceno", tableFormat1.InvoiceNumber);
                                    cmd.Parameters.AddWithValue("@invoicedate", tableFormat1.InvoiceDate);
                                    cmd.Parameters.AddWithValue("@orderno", tableFormat1.OrderNumber);
                                    cmd.Parameters.AddWithValue("@customername", tableFormat1.CustomerNumber);
                                    if (code1.Split().Length == 2)
                                    {
                                        cmd.Parameters.AddWithValue("@desc",
                                            (code1.Split()[1]).Replace(
                                                "'", " "));
                                    }
                                    if (code1.Split().Length == 3)
                                    {
                                        cmd.Parameters.AddWithValue("@desc",
                                            (code1.Split()[1] + " " + code1.Split()[2]).Replace(
                                                "'", " "));
                                    }
                                    if (code1.Split().Length == 4)
                                    {
                                        cmd.Parameters.AddWithValue("@desc",
                                            (code1.Split()[1] + " " + code1.Split()[2] + " " + code1.Split()[3]).Replace(
                                                "'", " "));
                                    }
                                    if (code1.Split().Length == 5)
                                    {
                                        cmd.Parameters.AddWithValue("@desc",
                                            (code1.Split()[1] + " " + code1.Split()[2] + " " + code1.Split()[3] + " " +
                                             code1.Split()[4]).Replace("'", " "));
                                    }
                                    if (code1.Split().Length == 6)
                                    {
                                        cmd.Parameters.AddWithValue("@desc",
                                            (code1.Split()[1] + " " + code1.Split()[2] + " " + code1.Split()[3] + " " +
                                             code1.Split()[4] + " " + code1.Split()[5]).Replace("'", " "));
                                    }
                                    if (code1.Split().Length == 7)
                                    {
                                        cmd.Parameters.AddWithValue("@desc",
                                            (code1.Split()[1] + " " + code1.Split()[2] + " " + code1.Split()[3] + " " +
                                             code1.Split()[4] + " " + code1.Split()[5] + " " + code1.Split()[6]).Replace(
                                                 "'", " "));
                                    }
                                    if (code1.Split().Length == 8)
                                    {
                                        cmd.Parameters.AddWithValue("@desc",
                                            (code1.Split()[1] + " " + code1.Split()[2] + " " + code1.Split()[3] + " " +
                                             code1.Split()[4] + " " + code1.Split()[5] + " " + code1.Split()[6] + " " +
                                             code1.Split()[7]).Replace("'", " "));
                                    }
                                    if (code1.Split().Length >= 9)
                                    {
                                        cmd.Parameters.AddWithValue("@desc",
                                            (code1.Split()[1] + " " + code1.Split()[2] + " " + code1.Split()[3] + " " +
                                             code1.Split()[4] + " " + code1.Split()[5] + " " + code1.Split()[6] + " " +
                                             code1.Split()[7] + " " + code1.Split()[8]).Replace("'", " "));
                                    }
                                    cmd.Parameters.AddWithValue("@unit", str);
                                    var shipped = Double.Parse(code2.Split()[2]) / Double.Parse(code2.Split()[1]);
                                    cmd.Parameters.AddWithValue("@shipped", Double.IsNaN(shipped) ? 0.00 : shipped);
                                    cmd.Parameters.AddWithValue("@amount", Double.Parse(code2.Split()[2]));
                                    cmd.Parameters.AddWithValue("@price", code2.Split()[1]);
                                    //if (double.IsNaN(shipped))
                                    //{
                                    //    cmd.Parameters.AddWithValue("@pdfprice", 0.00);
                                    //    cmd.Parameters.AddWithValue("@price", code2.Split()[1]);
                                    //}
                                    //else
                                    //{
                                    //    cmd.Parameters.AddWithValue("@pdfprice", code2.Split()[1]);
                                    //    cmd.Parameters.AddWithValue("@price", Double.Parse(code2.Split()[1]) / shipped);
                                    //}
                                    cmd.ExecuteNonQuery();
                                }
                                else if (s.Split().Contains("Date:") || (s.Split().Contains("Invoice") && s.Split().Contains("Number:")) ||
                                (s.Split().Contains("Order") && s.Split().Contains("Number:")) || (s.Split().Contains("Customer") && s.Split().Contains("Number:")) ||
                                (s.Split().Contains("Net") && s.Split().Contains("Invoice:")))
                                {
                                    if (s.Split().Contains("Date:"))
                                    {
                                        str = "Date:";
                                        var code2 = s.Split(new[] { str }, StringSplitOptions.None)[1];
                                        tableFormat1.InvoiceDate = code2.Split()[1];
                                    }
                                    else if (s.Split().Contains("Invoice") && s.Split().Contains("Number:"))
                                    {
                                        str = "Invoice";
                                        var code2 = s.Split(new[] { str }, StringSplitOptions.None)[1];
                                        tableFormat1.InvoiceNumber = code2.Split()[2];
                                    }
                                    else if (s.Split().Contains("Order") && s.Split().Contains("Number:"))
                                    {
                                        str = "Order";
                                        var code2 = s.Split(new[] { str }, StringSplitOptions.None)[1];
                                        tableFormat1.OrderNumber = code2.Split()[2];
                                    }
                                    else if (s.Split().Contains("Customer") && s.Split().Contains("Number:"))
                                    {
                                        str = "Customer";
                                        var code2 = s.Split(new[] { str }, StringSplitOptions.None)[1];
                                        tableFormat1.CustomerNumber = code2.Split()[2];
                                    }
                                    else if (s.Split().Contains("Net") && s.Split().Contains("Invoice:"))
                                    {
                                        str = "Net";
                                        //var code2 = s.Split(new[] { str }, StringSplitOptions.None)[1];
                                        //netinvoice = code2.Split()[2];
                                    }
                                }
                            }



                            else
                            {
                                cmd.Parameters.AddWithValue("@invoiceno", "");
                                cmd.Parameters.AddWithValue("@invoicedate", "");
                                cmd.Parameters.AddWithValue("@orderno", "");
                                cmd.Parameters.AddWithValue("@customername", "");
                                cmd.Parameters.AddWithValue("@item", "");
                                cmd.Parameters.AddWithValue("@desc", "");
                                cmd.Parameters.AddWithValue("@unit", "");
                                cmd.Parameters.AddWithValue("@shipped", "");
                                cmd.Parameters.AddWithValue("@price", "");
                                cmd.Parameters.AddWithValue("@amount", "");
                                cmd.Parameters.AddWithValue("@pdfprice", "");
                            }
                        }
                    }
                    cnn.Close();
                    return true;
                }
            }

            else if (editedText.First().Contains("***") || editedText.Last().Contains("Please remember to write Invoice"))
            {
                using (cnn = new SqlConnection(cnn.ConnectionString))
                {
                    cnn.Open();
                    var str1 = ""; var tableFormat2 = new PdfTableFormat2();

                    foreach (var f in editedText)
                    {
                        if (editedText.Last().Contains("Please remember to write Invoice"))
                        {
                            tableFormat2.FileName = p.Split('\\').Last();
                            tableFormat2.InvoiceNumber = editedText.Last().Split()[6];
                        }
                        if (f.Split().Contains("Driver"))
                        {
                            tableFormat2.InvoiceDate = f.Split()[0];
                        }
                        using (var cmdfmt =
                            new SqlCommand(
                                "insert into PDFFormatTable2 " +
                                "(FileName,InvoiceNo,InvoiceDate,Shipped,Unit,Item,Description,Price,Amount)" +
                                "values(@filename,@invoiceno,@invoicedate,@shipped,@unit,convert(VARCHAR(max), @item),@desc,@price,@amount)",
                                cnn))
                        {

                            var cas = f.Split().Contains("CAS") && !f.Split().Contains("*") && !f.Split()[0].Contains("0") && f.Contains("(");
                            var pk = f.Split().Contains("PK") && !f.Split().Contains("*") && !f.Split()[0].Contains("0") && f.Contains("(");
                            var mb = f.Split().Contains("MB") && !f.Split().Contains("*") && !f.Split()[0].Contains("0") && f.Contains("(");
                            var pkt = f.Split().Contains("PKT") && !f.Split().Contains("*") && !f.Split()[0].Contains("0") && f.Contains("(");
                            var mba = f.Split().Contains("MBA") && !f.Split().Contains("*") && !f.Split()[0].Contains("0") && f.Contains("(");
                            var bag = f.Split().Contains("BAG") && !f.Split().Contains("*") && !f.Split()[0].Contains("0") && f.Contains("(");
                            var tryy = f.Split().Contains("TRY") && !f.Split().Contains("*") && !f.Split()[0].Contains("0") && f.Contains("(");
                            var pak = f.Split().Contains("PAK") && !f.Split().Contains("*") && !f.Split()[0].Contains("0") && f.Contains("(");
                            var mc = f.Split().Contains("MC") && !f.Split().Contains("*") && !f.Split()[0].Contains("0") && f.Contains("(");

                            if (f.Split().Contains("***") && f.Split('*')[3].StartsWith(" QUOTE"))
                            {
                                //tableFormat2.InvoiceDate = f.Split()[2];
                                //tableFormat2.InvoiceNumber = f.Split('*')[7];
                            }

                            else if (cas || pk || mb || pkt || mba || bag || tryy || pak || mc)
                            {

                                if (f.Split().Contains("CAS"))
                                {
                                    str1 = "CAS";
                                }
                                else if (f.Split().Contains("PK"))
                                {
                                    str1 = "PK";
                                }
                                else if (f.Split().Contains("MB"))
                                {
                                    str1 = "MB";
                                }
                                else if (f.Split().Contains("MBA"))
                                {
                                    str1 = "MBA";
                                }
                                else if (f.Split().Contains("BAG"))
                                {
                                    str1 = "BAG";
                                }
                                else if (f.Split().Contains("TRY"))
                                {
                                    str1 = "TRY";
                                }
                                else if (f.Split().Contains("PAK"))
                                {
                                    str1 = "PAK";
                                }
                                else if (f.Split().Contains("MC"))
                                {
                                    str1 = "MC";
                                }
                                else if (f.Split().Contains("PKT"))
                                {
                                    str1 = "PKT";
                                }
                                var code1 = f.Split(new[] { str1 }, StringSplitOptions.None)[1];
                                var code1112 = code1.Split(new[] { "(" }, StringSplitOptions.None)[0];
                                string code11;
                                if ((f.Split('(').Length - 1) >= 2)
                                {
                                    var splits = f.LastIndexOf('(');
                                    code11 = f.Substring(splits + 1);
                                }
                                else if (!f.Contains("("))
                                {
                                    code11 = code1.Substring(26, 35);
                                }
                                else
                                {
                                    code11 = f.Split(new[] { "(" }, StringSplitOptions.None)[1];
                                }

                                //var code2 = f.Split(new[] { str1 }, StringSplitOptions.None)[1];
                                tableFormat2.Shipped = f.Split()[0];
                                var list = code1112.Split().Distinct().ToList();
                                var list1 = code11.Split().Distinct().ToList();

                                cmdfmt.Parameters.AddWithValue("@filename", tableFormat2.FileName);
                                cmdfmt.Parameters.AddWithValue("@invoiceno", tableFormat2.InvoiceNumber);
                                cmdfmt.Parameters.AddWithValue("@invoicedate", tableFormat2.InvoiceDate);
                                cmdfmt.Parameters.AddWithValue("@shipped", tableFormat2.Shipped);
                                cmdfmt.Parameters.AddWithValue("@unit", str1);
                                cmdfmt.Parameters.AddWithValue("@item", list[1]);
                                switch (list.Count)
                                {
                                    case 2:
                                        cmdfmt.Parameters.AddWithValue("@desc", "");
                                        cmdfmt.Parameters.AddWithValue("@price", "");
                                        cmdfmt.Parameters.AddWithValue("@amount", "");
                                        cmdfmt.Parameters.AddWithValue("@pdfprice", "");
                                        break;
                                    case 4:
                                        if (Convert.ToInt16(tableFormat2.Shipped) < 2 && list1.Count == 3)
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + "(" + list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[2]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        else
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + "(" + list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[3]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        break;

                                    case 5:
                                        if (Convert.ToInt16(tableFormat2.Shipped) < 2 && list1.Count == 3)
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " (" + list1[0]).Replace(
                                                    "'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[2]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        else
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " (" + list1[0]).Replace(
                                                    "'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[3]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        break;

                                    case 6:
                                        if (Convert.ToInt16(tableFormat2.Shipped) < 2 && list1.Count == 3)
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + "(" +
                                                 list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[2]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        else
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + "(" +
                                                 list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[3]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        break;
                                    case 7:
                                        if (Convert.ToInt16(tableFormat2.Shipped) < 2 && list1.Count == 3)
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + " " + list[6] +
                                                 "(" + list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[2]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        else
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + " " + list[6] +
                                                 "(" + list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[3]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        break;

                                    case 8:
                                        if (Convert.ToInt16(tableFormat2.Shipped) < 2 && list1.Count == 3)
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + " " + list[6] +
                                                 " " + list[7] + "(" + list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[2]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        else
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + " " + list[6] +
                                                 " " + list[7] + "(" + list1[0]).Replace("'", " "));
                                            //if (list1[2].Contains("")) { cmdfmt.Parameters.AddWithValue("@price", list1[3]); }
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[3]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        break;
                                    case 9:
                                        if (Convert.ToInt16(tableFormat2.Shipped) < 2 && list1.Count == 3)
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + " " + list[6] +
                                                 " " + list[7] + " " + list[8] + "(" + list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[2]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        else
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + " " + list[6] +
                                                 " " + list[7] + " " + list[8] + "(" + list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[3]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        break;
                                    case 10:
                                        if (Convert.ToInt16(tableFormat2.Shipped) < 2 && list1.Count == 3)
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + " " + list[6] +
                                                " " + list[7] + " " + list[8] + " " + list[9] + " " + list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[2]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        else
                                        {
                                            cmdfmt.Parameters.AddWithValue("@desc",
                                                (list[2] + " " + list[3] + " " + list[4] + " " + list[5] + " " + list[6] +
                                                " " + list[7] + " " + list[8] + " " + list[9] + "(" + list1[0]).Replace("'", " "));
                                            cmdfmt.Parameters.AddWithValue("@price", list1[2]);
                                            tableFormat2.Amount = Double.Parse(list1[3]);
                                            cmdfmt.Parameters.AddWithValue("@amount", tableFormat2.Amount);
                                        }
                                        break;
                                }

                                cmdfmt.ExecuteNonQuery();
                            }
                        }
                    }
                    cnn.Close();
                }
                return true;
            }
            return false;
        }

        protected void Radio1_submit_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}