<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" MasterPageFile="~/Site.Master" Inherits="Groceries_Application.Views.MainPage" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %> 

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="panel-heading">
        <h3 class=" panel-heading text-center">Grocery Tool</h3>
    </div>
    <div id="dialog"><table id="edit"></table></div>
    <div class="panel-body" >
        <div class="" id="content-wrap">
            <div class="container">
                <div>
                   <asp:RadioButton ID="RadioButton1" runat="server" GroupName="gender" Text="For Single File Upload" AutoPostBack="True" Checked="True"></asp:RadioButton>&nbsp;&nbsp;&nbsp;&nbsp;
                   <asp:RadioButton ID="RadioButton2" runat="server" GroupName="gender" Text="For Multiple Files Upload" AutoPostBack="True"></asp:RadioButton>
                   <%-- <input type="radio" id="radio1" name="action" value="Single" onclick="single();" />For Single File Upload &nbsp;&nbsp;&nbsp;&nbsp;
                    <input type="radio" id="radio2" name="action" value="Multiple"  onclick="multiples()"/>For Multiple Files Upload     --%>
                </div>
                <div class="panel-group" id="accordion" >
                    <div class="panel panel-info">
                          <div id="collapseOne" >
                            <div class="panel-body modal-content" >
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
                                
                            </td>
                            <td>
                                <asp:Label ID="lblMessage"  Font-Bold="True" ForeColor="Red" runat="server" Visible="True"></asp:Label>
                            </td>
                        </tr>
                   </table>
                                </div>
                            </div>
                         </div>
                    </div>
                </div>
                
                <div class="progress-bar panel-primary" runat="server">Loading.....</div>

                <div class="panel-collapse" id="action">
                    <div class="panel panel-info">
                         <div id="collapseTwo" >
                            <div class="panel-body modal-content">
                                <div><br/>
                                    <%--<div><input type="radio" id="radio2" name="action" value="Multiple"  onclick="multiples()"/>For Multiple Files Upload </div>--%>
                      <table id="table2">
                        <tr>
                            <td><br/>
                                <label >Upload multiple files of Vendor :</label>
                                <br/>
                            </td>
                            <td style="float: right"><br/>

                                <CuteWebUI:UploadAttachments InsertText="Upload Multiple files" runat="server" ID="FileUpload12"></CuteWebUI:UploadAttachments>
                                <button onclick="DoBrowseFolder();return false;" id='btnFolder'>Upload Folder</button>
                            </td>        
                        </tr>
                        <tr>
                            <td style="float: right;">
                                <br/>
                                <input type="submit" id="Submit2" value="Upload" runat="server" OnServerClick="Submit2_OnServerClick" />
                            </td>
                            <td style="float: none">
                                <br/>
                                
                            </td>
                            <td style="float: left">
                                <asp:Label ID="Label1"  Font-Bold="True" ForeColor="Red" runat="server" Visible="True" ></asp:Label>
                                <asp:Label ID="Label2"  Font-Bold="True" ForeColor="Red" runat="server" Visible="True" ></asp:Label>
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
    
    <style>
        #table1 {
            display: block;
            padding: 10px;
            background-color: whitesmoke;
            border: 1px solid black;
        }
        #table2 {
            display: block;
            padding: 10px;
            background-color: whitesmoke;
            border: 1px solid black;
        }
    </style>
    <script>

        var checked1 = $('#ContentPlaceHolder1_RadioButton1')[0].checked;
        var checked2 = $('#ContentPlaceHolder1_RadioButton2')[0].checked;
        var lblmsg = $('#ContentPlaceHolder1_lblMessage');
        var lbl1 = $('#ContentPlaceHolder1_Label1');
        if (checked1) {
            document.getElementById("table1").style.display = 'block';
            document.getElementById("accordion").style.display = 'block';
            document.getElementById("action").style.display = 'none';
            $('#ContentPlaceHolder1_Label1')[0].textContent = "";
        }
        if (checked2) {
            document.getElementById("table2").style.display = 'block';
            document.getElementById("accordion").style.display = 'none';
            document.getElementById("action").style.display = 'block';
            $('#ContentPlaceHolder1_lblMessage')[0].textContent = "";
        }

/* For multiple files Upload Folder*/
        var uploader;
        var inputfile;
        var btnFolder;
        function CuteWebUI_AjaxUploader_OnInitialize() {
            uploader = this;
            var scope = uploader.internalobject;
            if (scope.addontype != "HTML5") return;
            inputfile = scope.addonobject;
            if (!/Chrome/.test(navigator.userAgent)) return;
            btnFolder = document.getElementById("btnFolder");
            btnFolder.style.display = "";
        }
        function DoBrowseFolder() {
            if (!inputfile) return;
            inputfile.setAttribute("webkitdirectory", "");
            inputfile.setAttribute("directory", "");
            uploader.startbrowse();
            inputfile.removeAttribute("webkitdirectory");
            inputfile.removeAttribute("directory");
        }
/* For Upload Folder Code*/

    </script>
</asp:Content>