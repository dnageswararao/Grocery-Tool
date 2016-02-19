using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using DataLayer;
using Groceries_Application.Reports;
using Telerik.Reporting;
using Telerik.Web.UI;

namespace Groceries_Application.Views
{
    public partial class ReportFormat : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetVendor();
            }
            SearchButton1.Click += SearchButton_Click;
        }

        private void GetVendor()
        {
            var vendorsList = new List<Vendor> { new Vendor("DEEP"), 
                                     new Vendor("SWAD")};
            foreach (var w in vendorsList)
            {
                VendorNameComboBox.Items.Add(new RadComboBoxItem(w.Name));
            }
        }

        void SearchButton_Click(object sender, EventArgs e)
        {
            var vendortextbox = VendorNameComboBox.Text;
            var fromdate = FromDatePicker.DateInput.DisplayText;
            var todate = ToDatePicker.DateInput.DisplayText;
            var desctext = ItemsComboBox.Text;
            var top10Rate = Top10Rate.Checked;
            var top10RateDecrease = Top10RateDecrease.Checked;
            var last2Invoices = Last2Invoices.Checked;
            var allitems = AllItemsRadio.Checked;

            if (VendorNameComboBox.Text != "" && FromDatePicker.DateInput.DisplayText != "" &&
                ToDatePicker.DateInput.DisplayText != "" && ItemsComboBox.Text == "" && allitems)
            {
                ItemsCollection.GetVendorBasedResults(fromdate, todate, vendortextbox);
                var report1 = ((Report)new VendorReport2());
                ReportViewer1.ReportSource = new InstanceReportSource { ReportDocument = report1 };

                report1.ReportParameters[0].Value = fromdate;
                report1.ReportParameters[1].Value = todate;
                report1.ReportParameters[2].Value = vendortextbox;
                ReportViewer1.RefreshReport();
            }

            else if (VendorNameComboBox.Text != "" && FromDatePicker.DateInput.DisplayText != "" &&
                     ToDatePicker.DateInput.DisplayText != "" && ItemsComboBox.Text != "")
            {
                ItemsCollection.GetItemVendorBasedResults(fromdate, todate, vendortextbox, desctext);
                var report1 = ((Report)new ItemVendorBasedReport());
                ReportViewer1.ReportSource = new InstanceReportSource { ReportDocument = report1 };

                report1.ReportParameters[0].Value = fromdate;
                report1.ReportParameters[1].Value = todate;
                report1.ReportParameters[2].Value = vendortextbox;
                report1.ReportParameters[3].Value = desctext;
                ReportViewer1.RefreshReport();
            }

            else if (top10Rate && VendorNameComboBox.Text != "")
            {
                ItemsCollection.GetTop10Rate(vendortextbox);
                var report1 = ((Report)new Top10RateIncreaseReport());
                ReportViewer1.ReportSource = new InstanceReportSource { ReportDocument = report1 };

                report1.ReportParameters[0].Value = vendortextbox;

                ReportViewer1.RefreshReport();
            }

            else if (top10RateDecrease && VendorNameComboBox.Text != "")
            {
                ItemsCollection.GetTop10RateDecrease(vendortextbox);
                var report1 = ((Report)new Top10RateDecreaseReport());
                ReportViewer1.ReportSource = new InstanceReportSource { ReportDocument = report1 };

                report1.ReportParameters[0].Value = vendortextbox;

                ReportViewer1.RefreshReport();
            }

            else if (last2Invoices && VendorNameComboBox.Text != "")
            {
                ItemsCollection.GetLast2Invoices(vendortextbox);
                var report1 = ((Report)new Last2InvoicesReport());
                ReportViewer1.ReportSource = new InstanceReportSource { ReportDocument = report1 };

                report1.ReportParameters[0].Value = vendortextbox;

                ReportViewer1.RefreshReport();
            }

            //else if (allitems && VendorNameComboBox.Text != "")
            //{
            //    ItemsCollection.GetVendorAllItems(vendortextbox);
            //    var report1 = ((Report)new GetVendorItemsReport());
            //    ReportViewer1.ReportSource = new InstanceReportSource { ReportDocument = report1 };

            //    report1.ReportParameters[0].Value = vendortextbox;

            //    ReportViewer1.RefreshReport();
            //}
        }
        public IList<ItemsList> ItemsDataCollection { get; set; }
        protected void VendorNameComboBox_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var cnn = DbUtility.GetConnection();
            using (cnn = new SqlConnection(cnn.ConnectionString))
            {
                var query = "SELECT distinct(Item),Description FROM SavePDFTable where amount <>0 and VendorName='" + VendorNameComboBox.Text +"'";
                cnn.Open();

                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);
                //InvoicesComboBox.Items.Clear();
                ItemsComboBox.Items.Clear();
                //InvoicesComboBox.Text = "";
                ItemsComboBox.Text = "";
                //var l = (from DataRow dr in ds.Tables[0].Rows select dr[0].ToString()).Distinct().ToList();
                var l1 = (from DataRow dr in ds.Tables[0].Rows select dr[1].ToString()).ToList();
                //foreach (var s in l)
                //{
                //    InvoicesComboBox.Items.Add(new RadComboBoxItem(s));
                //}
                foreach (var s1 in l1)
                {
                    ItemsComboBox.Items.Add(new RadComboBoxItem(s1));
                }
            }
        }

        //protected void InvoicesComboBox_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    using (_cnn = new SqlConnection(ConnetionString))
        //    {
        //        var query = VendorNameComboBox.Text.Contains("DIP")
        //            ? "SELECT Item,Description FROM PDFFormatTable2 where InvoiceNo = '" + e.Text + "'" 
        //            : "SELECT Item,Description FROM SavePDFTable where InvoiceNo = '" + e.Text + "'";
        //        _cnn.Open();
        //        var cmd = new SqlCommand(query, _cnn);
        //        var sda = new SqlDataAdapter(cmd);
        //        var ds = new DataSet();
        //        sda.Fill(ds);
        //        ItemsComboBox.Items.Clear();
        //        ItemsComboBox.Text = "";
        //        var l = (from DataRow dr in ds.Tables[0].Rows select dr[1].ToString()).ToList();
        //        foreach (var s in l)
        //        {
        //            ItemsComboBox.Items.Add(new RadComboBoxItem(s));
        //        }
        //    }
        //}


        protected void Top10Rate_OnCheckedChanged(object sender, EventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ItemsComboBox.Text = "";
            ToDatePicker.SelectedDate = null;
            ReportViewer1.ReportSource = null;
        }

        protected void Top10RateDecrease_OnCheckedChanged(object sender, EventArgs e)
        {
            FromDatePicker.SelectedDate = null;
            ItemsComboBox.Text = "";
            ToDatePicker.SelectedDate = null;
            ReportViewer1.ReportSource = null;
        }

        protected void AllItemsRadio_OnCheckedChanged(object sender, EventArgs e)
        {
            //FromDatePicker.SelectedDate = null;
            ItemsComboBox.Text = "";
            //ToDatePicker.SelectedDate = null;
            ReportViewer1.ReportSource = null;
        }

        protected void ResetButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("ReportFormat.aspx");
        }

        protected void ItemsComboBox_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Top10RateDecrease.Checked = false;
            Top10Rate.Checked = false;
            Last2Invoices.Checked = false;
            AllItemsRadio.Checked = false;
            ReportViewer1.ReportSource = null;
        }
    }
    public class Vendor
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public Vendor(string name)
        {
            Name = name;
        }
    }

    [DataObject]
    public class ItemsList
    {
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public string Shipped { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
    }
}