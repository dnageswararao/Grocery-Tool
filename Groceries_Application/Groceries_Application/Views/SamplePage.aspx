<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SamplePage.aspx.cs" MasterPageFile="~/Site.Master" Inherits="Groceries_Application.Views.SamplePage" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %> 

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <h3 class=" panel-heading text-center">Grocery Tool</h3>
    </div>
    <div class="progress">
            <div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 60%;">
                <span class="sr-only">60% Complete</span>
             </div>
    </div>
<div class="container">
  <div class="row">
    <div class="col-xs-12">
        <br/>
        <asp:RadioButton ID="RadioButton1" runat="server" GroupName="upload" Text="For Single File Upload" AutoPostBack="True"></asp:RadioButton>
        
        <asp:RadioButton ID="RadioButton2" runat="server" GroupName="upload" Text="For Multiple Files Upload" AutoPostBack="True"></asp:RadioButton>
       <%-- <input name="collapseGroup" type="radio" data-toggle="collapse" data-target="#collapseOne" value="Single" onclick="single();" />For Single File Upload &nbsp;&nbsp;&nbsp;&nbsp;

        <input name="collapseGroup" type="radio" data-toggle="collapse" data-target="#collapseTwo" value="Multiple"  onclick="multiples()"/>For Multiple Files Upload     --%>
        
      <div class="panel-group" id="accordion">
        <div class="panel panel-primary">
          <div id="collapseOne" >
            <div class="panel-body" >
              <div><br/>
                    <table id="table1" >
                        <tr>
                            <td><br/>
                                <label >Upload file of Vendor :</label>
                                <br/>
                            </td>
                            <td ><br/>
                                <asp:FileUpload ID="FileUpload1" runat="server" Text="Browse"  />
                            </td>        
                        </tr>
                        <tr>
                            <td style="float: right;">
                                <br/>
                                
                                <input type="submit" id="Submit1" value="Upload" runat="server" OnServerClick="Submit1_OnServerClick"/>
                            </td>
                            <td style="float: none">
                                <br/>
                                <asp:Label ID="lblMessage"  Font-Bold="True" ForeColor="Red" runat="server" Visible="True"></asp:Label>
                            </td>
                        </tr>
                   </table>
                    </div>
            </div>
          </div>
            
        </div>
      </div>
        
        <div class="panel-group" id="action">
        <div class="panel panel-info">
          <div id="collapseTwo" >
            <div class="panel-body">
              <div><br/>
                    <table id="table2">
                        <tr>
                            <td><br/>
                                <label >Upload multiple files of Vendor :</label>
                                <br/>
                            </td>
                            <td style="float: right"><br/>
                                <CuteWebUI:UploadAttachments InsertText="Upload Multiple files" runat="server" ID="FileUpload12"></CuteWebUI:UploadAttachments>
                                <button onclick="DoBrowseFolder();return false;" id='btnFolder'>
                                            Upload Folder</button>
                            </td>        
                        </tr>
                        <tr>
                            <td style="float: right;">
                                <br/>
                                <input type="submit" id="Submit2" value="Upload" runat="server"  OnServerClick="Submit2_OnServerClick"/>
                            </td>
                            <td style="float: none">
                                <br/>
                                <asp:Label ID="Label1"  Font-Bold="True" ForeColor="Red" runat="server" Visible="True" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
            </div>
          </div>

        </div>
      </div>

    </div>
  </div>
</div>
    <script>
        function multiples() {
            document.getElementById("table2").style.display = 'block';
            document.getElementById("table1").style.display = 'none';
            document.getElementById("accordion").style.display = 'none';
            document.getElementById("action").style.display = 'block';
        }
        function single() {
            document.getElementById("table1").style.display = 'block';
            document.getElementById("table2").style.display = 'none';
            document.getElementById("action").style.display = 'none';
            document.getElementById("accordion").style.display = 'block';
        }

        var checked1 = $('#ContentPlaceHolder1_RadioButton1')[0].checked;
        var checked2 = $('#ContentPlaceHolder1_RadioButton2')[0].checked;
        if (checked1) {
            $('.progress-bar').show();
            document.getElementById("table1").style.display = 'block';
            document.getElementById("accordion").style.display = 'block';
            document.getElementById("action").style.display = 'none';
        }
        if (checked2) {
            document.getElementById("table2").style.display = 'block';
            document.getElementById("accordion").style.display = 'none';
            document.getElementById("action").style.display = 'block';
        }
    </script>
    
    <style>
        #table1 {
            display: block;
            padding: 10px;
            background-color: whitesmoke;
            border: 1.5px solid black;
        }
        #table2 {
            display: block;
            padding: 10px;
            background-color: whitesmoke;
            border: 1.5px solid black;
        }
    </style>
</asp:Content>
