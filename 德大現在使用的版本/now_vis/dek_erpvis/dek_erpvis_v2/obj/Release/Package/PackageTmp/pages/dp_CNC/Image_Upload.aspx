<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Image_Upload.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_CNC.Image_Upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <h1>機台名稱：<%=Machine %></h1>
            <asp:FileUpload ID="FileUpload_Image" runat="server" Font-Size="40pt" /><br>
            <asp:Button ID="Button_Upload" runat="server" Text="上傳" OnClick="Button_Upload_Click" Font-Size="20" />
        </div>
    </form>
</body>
</html>
