<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ReportFormat.aspx.cs" Inherits="Groceries_Application.Views.ReportFormat" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=8.0.14.225, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2014.1.225.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="modal fade" id="loader" tabindex=" -1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                             <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                <h4 class="modal-title" id="myModalLabel">Please wait......</h4>
                                    </div>
                                    <div class="modal-body">
                                      <div class="progress">
                                          <div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 80%;">
                                    </div>
                                   </div>
                             </div>
                         </div>
                    </div>
                </div>

<div class="panel panel-primary pnl">
    <div class="panel-heading">
        <h4 class=" panel-title text-center">Vendor Report </h4>
    </div>
    <div id="dialog"><table id="edit"></table></div>
    <div class="panel-body">
        <div class="" id="content-wrap">
            <div class="container" style="width: 1000px">
                <div>
                    <table id="vendorReport">
                        <thead>
                            <tr>
                                <td style="width: 100px;"><span><b>Vendor <span style="color: red">*</span></b></span></td>
                                <td id="vendorcomboBox">
                                    <telerik:RadComboBox ID="VendorNameComboBox" EmptyMessage="Please Select a Vendor" runat="server" AllowCustomText="true" Filter="Contains" 
                                        autopostback="True" OnSelectedIndexChanged="VendorNameComboBox_OnSelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </td>
                                 <%--<td style="width: 100px;"><span><b>Invoice No</b></span></td>
                                <td>
                                    <telerik:RadComboBox ID="InvoicesComboBox" EmptyMessage="Please Select a InvoiceNo" runat="server" AllowCustomText="true" Filter="Contains" 
                                        autopostback="True" OnSelectedIndexChanged="InvoicesComboBox_OnSelectedIndexChanged">
                                    </telerik:RadComboBox><br/>
                                </td>--%>
                            
                                <td><span style="float: left"><b>Items</b></span></td>
                                <td>
                                    <telerik:RadComboBox ID="ItemsComboBox" EmptyMessage="Please Select a Item" runat="server" AllowCustomText="true" Filter="Contains" 
                                        AutoPostBack="True" OnSelectedIndexChanged="ItemsComboBox_OnSelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    <%--<telerik:RadTextBox runat="server" ID="itemtextbox" Visible="False"></telerik:RadTextBox>--%>
                                </td>
                                <td style="float: left">
                                    <telerik:RadButton OnCheckedChanged="AllItemsRadio_OnCheckedChanged" ID="AllItemsRadio" runat="server" ToggleType="Radio" ButtonType="ToggleButton" GroupName="StandardButton" Text="All Items" AutoPostBack="True">
                                    </telerik:RadButton>
                                </td>
                                <td>
                                    <telerik:RadButton OnCheckedChanged="Top10Rate_OnCheckedChanged" ID="Last2Invoices" runat="server" ToggleType="Radio" ButtonType="ToggleButton" GroupName="StandardButton" Text="Last2 Invoices" AutoPostBack="True">
                                    </telerik:RadButton>
                                </td>
                                </tr>
                            <tr>
                                <td><span style="width: 100px;float: right;"><b>From Date</b></span></td>
                                <td>
                                    <telerik:RadDatePicker ID="FromDatePicker" runat="server" Culture="en-US" Skin="Default"><DateInput DateFormat="MM/dd/yyyy" 
                                        DisplayDateFormat="MM/dd/yyyy" runat="server" ></DateInput></telerik:RadDatePicker>
                                </td>
                                <td><span ><b>To Date</b></span></td>
                                <td>
                                    <telerik:RadDatePicker ID="ToDatePicker" runat="server" Culture="en-US" Skin="Default"><DateInput DateFormat="MM/dd/yyyy" 
                                        DisplayDateFormat="MM/dd/yyyy" runat="server"></DateInput></telerik:RadDatePicker>
                                </td>
                                <td style="float: left">
                                    <telerik:RadButton OnCheckedChanged="Top10Rate_OnCheckedChanged" ID="Top10Rate" runat="server" ToggleType="Radio" ButtonType="ToggleButton" GroupName="StandardButton" Text="Top10 RateIncrease" AutoPostBack="True">
                                    </telerik:RadButton>
                                </td>
                                <td>
                                    <telerik:RadButton OnCheckedChanged="Top10RateDecrease_OnCheckedChanged" ID="Top10RateDecrease" runat="server" ToggleType="Radio" ButtonType="ToggleButton" GroupName="StandardButton" Text="Top10 RateDecrease" AutoPostBack="True">
                                    </telerik:RadButton>
                                </td>
                                
                                <td id="search_btn">
                                    <telerik:RadButton ID="SearchButton1" runat="server" Text="Search" AutoPostBack="true"></telerik:RadButton>
                                </td>
                                <td id="reset_btn" >
                                    <telerik:RadButton ID="ResetButton" runat="server" Text="Reset" AutoPostBack="true" OnClick="ResetButton_OnClick"></telerik:RadButton>
                                </td>
                            </tr>
                        </thead>
                        <tbody style="width: 100px">
                            <tr></tr>
                            <tr style="float: inherit">
                                <td id ="report" colspan="7"><br/>
                                    <div style="width: 100px">
                                        <telerik:ReportViewer ID="ReportViewer1" Height="762px" Width="980px" Skin="Office2007" runat="server" BorderStyle="Double" 
                                            ViewMode="PrintPreview"   ShowPrintPreviewButton="False"></telerik:ReportViewer>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
    
<script>
    $('#search_btn').on('click', function () {
        var vendorcombo = window.$find('<%=VendorNameComboBox.ClientID %>');
        var itemscombo = window.$find('<%=ItemsComboBox.ClientID %>');
        var fromdate = window.$find('<%=FromDatePicker.ClientID %>');
        var todate = window.$find('<%=ToDatePicker.ClientID %>');
        var top10Radio = window.$find('<%=Top10Rate.ClientID %>');
        var top10RadioDec = window.$find('<%=Top10RateDecrease.ClientID %>');
        var last2Invoices = window.$find('<%=Last2Invoices.ClientID %>');
        var allitems = window.$find('<%=AllItemsRadio.ClientID %>');
        if (vendorcombo._text == "" ) {
            alert('Please select any Vendor..');
        }
        else if (fromdate._dateInput._displayText != "" && todate._dateInput._displayText != "" && itemscombo._text != "") {
            top10Radio._checked = false;
            last2Invoices._checked = false;
        }
        else if (vendorcombo._text != "" && itemscombo._text != "" && (fromdate._dateInput._displayText == "" || todate._dateInput._displayText != "")) {
            alert('Please select From Date');
        }
        else if (vendorcombo._text != "" && itemscombo._text != "" && (fromdate._dateInput._displayText != "" || todate._dateInput._displayText == "")) {
            alert('Please select To Date');
        }
        else if (vendorcombo._text == "" && allitems._checked == true || (fromdate._dateInput._displayText == "" || todate._dateInput._displayText == "") && top10Radio._checked == false && top10RadioDec._checked == false && last2Invoices._checked == false) {
            alert('Please select From Date and To Date');
        }
        else if (vendorcombo._text != "" && allitems._checked == false && (fromdate._dateInput._displayText != "" || todate._dateInput._displayText != "")) {
            alert('Please select all items');
        }
        $('#loader').modal('show');
    });
</script>
</asp:Content>
