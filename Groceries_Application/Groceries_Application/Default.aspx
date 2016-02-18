<%@ Page Title="India Bazaar" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Groceries_Application.Default" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br /><br />
    <div class="container">
        <div class="panel panel-default modal-dialog">
            <div class="panel-heading"> <h2 class="text-center">Sign In</h2></div>
            <div class="panel-body">
                 <div class="form-group">
                        <div class="col-md-12">
                            <label>Username</label>
                            <div class="input-group col-sm-10">
                                <span class="input-group-addon glyphicon  glyphicon-user"></span>
                                <input id="username" name="username" type="text" placeholder="Username" autofocus="autofocus"  class="form-control" runat="server"/>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <label>Password</label>
                            <div class="col-sm-10 input-group">
                                <span class="input-group-addon glyphicon  glyphicon-lock"></span>
                                <input id="password" name="password" type="password" placeholder="Password" class="form-control" runat="server"/>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-6">
                           <br/> <input type="submit" value="Sign In" class="btn btn-primary pull-left" runat="server" OnServerClick="btn_signIn_OnServerClick"/>
                        </div>
                    </div>
                <div>
                    <br/>
                    <asp:Label ID="Label1"  Font-Bold="True" ForeColor="Red" runat="server" Visible="True" ></asp:Label>
                </div>
            </div>
        </div>
    </div>
    
    
</asp:Content>
