<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="比對加工欄位.aspx.cs" Inherits="dek_erpvis_v2.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label_NoUpdate" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
<script>
    var currentdate = new Date();
    var datetime = currentdate.getFullYear() + '/' + (currentdate.getMonth() + 1) + '/' + currentdate.getDate();
    alert(datetime)
</script>
</html>
