<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="從組裝MS寫至組裝MYSQL.aspx.cs" Inherits="dek_erpvis_v2.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        </div>

        <asp:Label ID="Label1" runat="server" Text="驗證用"></asp:Label>
        <p>
            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
        </p>

        <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="true" OnTextChanged="TextBox2_TextChanged"></asp:TextBox>


        <div>
            <div>
                <canvas height="300" name="SketchPad" style="border: 2px solid gray;" width="900"></canvas>
            </div>
            <input type="button" id="SaveImage" value="簽名" />
        </div>




    </form>
</body>
<script>
    //取得canvas
    var $canvas = $("[name = 'SketchPad']");
    var ctx = $canvas[0].getContext("2d");
    ctx.lineCap = "round";
    ctx.fillStyle = "white"; //整個canvas塗上白色背景避免PNG的透明底色效果
    ctx.fillRect(0, 0, $canvas.width(), $canvas.height());
    var drawMode = false;
    debugger;
    //canvas點選、移動、放開按鍵事件時進行繪圖動作
    $canvas.mousedown(function (e) {
        ctx.beginPath();
        ctx.strokeStyle = "black";//顏色
        ctx.lineWidth = "4";//粗度
        ctx.moveTo(e.pageX - $canvas.position().left, e.pageY - $canvas.position().top);
        drawMode = true;
    })
        .mousemove(function (e) {
            if (drawMode) {
                ctx.lineTo(e.pageX - $canvas.position().left, e.pageY - $canvas.position().top);
                ctx.stroke();
            }
        })
        .mouseup(function (e) {
            drawMode = false;
        });
    //利用.toDataqURL()將繪圖結果轉成圖檔
    $("#SaveImage").click(function () {
        var buf = toBinary($canvas);
        // Blob
        var blob = new Blob([buf.buffer], {
            type: 'image/png'
        });


        var fd = new FormData();
        fd.append("UploadedImage", blob);//圖檔
        fd.append("fileName", "Test");//檔案名稱，可以透過Url帶入。

        var ajaxRequest = $.ajax({
            type: "POST",
            url: "/api/FileUpload/UploadFile",
            contentType: false,
            processData: false,
            data: fd,
            success: function (msg) {
                debugger;
                alert('簽名 完成!!');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });

    });

    //壓縮圖檔(Binary)
    function toBinary(canvas) {
        var base64 = canvas[0].toDataURL('image/png'),
            bin = atob(base64.replace(/^.*,/, '')),
            buffer = new Uint8Array(bin.length);
        for (var i = 0; i < bin.length; i++) {
            buffer[i] = bin.charCodeAt(i);
        }
        return buffer;
    }
</script>
</html>
