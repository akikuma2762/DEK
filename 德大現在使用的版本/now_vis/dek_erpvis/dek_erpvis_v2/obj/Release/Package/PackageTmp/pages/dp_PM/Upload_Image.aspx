<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload_Image.aspx.cs" Inherits="dek_erpvis_v2.pages.dp_PM.Upload_Image" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
           <script type="text/javascript">

               //单击按钮，就像单击“浏览...”一样作用
               function BrowseFile() {
                   //document.getElementById("FileUpload1").style.display = "inline";
                   document.getElementById("FileUpload_image").click();
               }

               function photoTotal() {
                   document.getElementById("txtFileUrl").value = document.getElementById("FileUpload_image").value;
                   var size = document.getElementById("FileUpload_image").files.length;
                   document.getElementById("photoTal").innerText = "選擇了" + size + "個檔案";
               }


        </script>

        <div class="col-md-12 col-sm-12 col-xs-12">
            <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Label1" runat="server" Text="[產線名稱]:" Font-Size="50px"></asp:Label>
            <asp:Label ID="Label_name" runat="server" Text="" Font-Size="50px"></asp:Label><br />
            <br />

            <asp:Label ID="Label2" runat="server" Text="[排程編號]:" Font-Size="50px"></asp:Label>
            <asp:Label ID="Label_num" runat="server" Text="" Font-Size="50px"></asp:Label><br />
            <br />
            <asp:Label ID="Label3" runat="server" Text="[照片上傳]：" Font-Size="50px"></asp:Label><br />
            <div>
                <asp:FileUpload ID="FileUpload_image" runat="server" AllowMultiple="True" class="file" Width="203px" Height="23px" />
                <asp:TextBox ID="txtFileUrl" runat="server" Width="196px" Height="16px"></asp:TextBox>
            </div>
            <div>
                <asp:Image ID="Image3" runat="server" Style="width: 200px; height: 80px" ImageUrl="../../assets/images/setfile.JPG" onclick="BrowseFile()" />
                &nbsp&nbsp&nbsp&nbsp&nbsp
                <asp:Label ID="photoTal" runat="server" Text="未選擇任何檔案" ForeColor="#5C788F" Font-Names="微軟正黑體" Font-Size="70px" onclick="BrowseFile()"></asp:Label>
            </div>
            <br />
            <asp:Button ID="Button_Upload" runat="server" Text="上傳" BackColor="#26b99a" OnClick="Button_Upload_Click" Font-Size="50px" Height="110px" Width="417px" />
            <br />
            <asp:Label ID="Label4" runat="server" Font-Size="50px" style="display:none"></asp:Label>
        </div>

        <div class="gallery" id="gally"></div>
    </form>

    <script>
        $(function () {
            // Multiple images preview in browser
            var imagesPreview = function (input, placeToInsertImagePreview) {
                if (input.files) {
                    var filesAmount = input.files.length;
                    for (i = 0; i < filesAmount; i++) {
                        var reader = new FileReader();
                        reader.onload = function (event) {
                            $($.parseHTML('<img>')).attr('src', event.target.result).appendTo(placeToInsertImagePreview);
                        }
                        reader.readAsDataURL(input.files[i]);
                    }
                }
            };

            $('#FileUpload_image').on('change', function () {
                $("#gally").empty();
                imagesPreview(this, 'div.gallery');
            });
        });
    </script>
</body>
</html>
