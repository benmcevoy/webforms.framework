<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Webforms.Framework.Web.Default" %>
<%@ Import Namespace="Webforms.Framework.Web.Model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js" type="text/javascript"></script>
    <style>
    .error { border: 1px solid red; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <div>
            <asp:ValidationSummary runat="server" EnableClientScript="true" />
        </div>
        <div>
            <asp:Repeater runat="server" ID="ProductRepeater">
                <ItemTemplate>
                    <div>
                        <label>Name</label>
                        <asp:TextBox runat="server"  MaxLength="10" ID="Name" Text="<%# As<Product>(Container).Name %>" />
                        <asp:DataAnnotationValidator ID="dav1" runat="server" Display="Dynamic" ControlToValidate="Name" Property="Name" Type="Webforms.Framework.Web.Model.Product, Webforms.Framework.Web" />
                    </div>

                        <div>
                        <label>Quantity</label>
                        <asp:TextBox runat="server"  MaxLength="10" ID="QuantityTextBox" Text="<%# As<Product>(Container).Quantity %>" />
                        <asp:DataAnnotationValidator ID="DataAnnotationValidator1" runat="server" Display="Dynamic" ControlToValidate="QuantityTextBox" Property="Quantity" Type="Webforms.Framework.Web.Model.Product, Webforms.Framework.Web" />
                    </div>

                    <div>
                        <label>Name</label>
                        <asp:TextBox runat="server" MaxLength="10" ID="TextBox1" Text="<%# As<Product>(Container).Name %>" />
                        <asp:RequiredFieldValidator ID="reqVal1" runat="server" ControlToValidate="TextBox1" ErrorMessage="requied"/>
                        <asp:RangeValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" ErrorMessage="range error" MinimumValue="10" MaximumValue="20"/>
                    </div>

       <%--             <div>
                        <label>Quantity</label>
                        <asp:TextBox runat="server" ID="Quantity" Text="<%# AsProduct(Container).Quantity %>" />
                        <asp:DataAnnotationValidator ID="DataAnnotationValidator1" runat="server" ControlToValidate="Quantity" Property="Quantity" Type="Webforms.Framework.Web.Model.Product, Webforms.Framework.Web" />
                    </div>--%>
                   

                </ItemTemplate>
            </asp:Repeater>
            <asp:Button runat="server" Text="Submit" OnClick="Button_Click" CausesValidation="true"/>
        </div>
    </form>

    <script src="Script/webforms.validate.proxy.js" type="text/javascript"></script>
</body>
</html>
