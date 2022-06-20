<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="好看的TAB標籤.aspx.cs" Inherits="dek_erpvis_v2.WebForm5" %>

<!DOCTYPE html>
<link rel="stylesheet" href="https://e6t7a8v2.stackpathcdn.com/tutorial/css/fontawesome-all.min.css">
<link rel="stylesheet" href="https://e6t7a8v2.stackpathcdn.com/tutorial/css/bootstrap.min.css">
<link href="https://fonts.googleapis.com/css2?family=Nunito:wght@200;300;400;600;700&display=swap" rel="stylesheet">
<style>
    a:hover,
    a:focus {
        text-decoration: none;
        outline: none;
    }

    .tab {
        font-family: 'Nunito', sans-serif;
    }

        .tab .nav-tabs {
            background-color: #fff;
            padding: 0 0 1px;
            margin: 0 0 10px;
            border: none;
            border-radius: 30px;
            box-shadow: 3px 3px 15px rgba(0, 0, 0, 0.15);
        }

            .tab .nav-tabs li a {
                color: #555;
                background: #fff;
                font-size: 17px;
                font-weight: 700;
                text-transform: uppercase;
                padding: 8px 20px 6px;
                margin: 0 5px 0 0;
                border: none;
                border-radius: 30px;
                overflow: hidden;
                position: relative;
                z-index: 1;
                transition: all 0.3s ease 0.3s;
            }

                .tab .nav-tabs li.active a,
                .tab .nav-tabs li a:hover,
                .tab .nav-tabs li.active a:hover {
                    color: #fff;
                    background: #fff;
                    border: none;
                }

                .tab .nav-tabs li a:before,
                .tab .nav-tabs li a:after {
                    content: "";
                    background-color: #1890E0;
                    width: 100%;
                    height: 100%;
                    border-radius: 30px;
                    opacity: 0.5;
                    transform: scaleX(0);
                    position: absolute;
                    top: 0;
                    left: 0;
                    z-index: -1;
                    transition: all 0.4s ease 0s;
                }

                .tab .nav-tabs li a:after {
                    background-color: #7A10EB;
                    transform: scaleX(0);
                    transition: all 0.4s ease 0.2s;
                }

                .tab .nav-tabs li.active a:before,
                .tab .nav-tabs li a:hover:before {
                    opacity: 0;
                    transform: scaleX(1);
                }

                .tab .nav-tabs li.active a:after,
                .tab .nav-tabs li a:hover:after {
                    opacity: 1;
                    transform: scaleX(1);
                    background: linear-gradient(to right, #1890E0, #7A10EB);
                }

        .tab .tab-content {
            color: #999;
            background-color: #fff;
            font-size: 17px;
            letter-spacing: 1px;
            line-height: 30px;
            padding: 20px;
            box-shadow: 3px 3px 15px rgba(0, 0, 0, 0.15);
            border-radius: 20px;
            position: relative;
        }

    @media only screen and (max-width: 479px) {
        .tab .nav-tabs {
            padding: 0;
            border-radius: 20px;
        }

            .tab .nav-tabs li {
                width: 100%;
                text-align: center;
                margin: 0 0 5px;
            }

                .tab .nav-tabs li:last-child {
                    margin-bottom: 0;
                }

                .tab .nav-tabs li a {
                    margin: 0;
                }
    }
</style>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <div class="tab" role="tabpanel">
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li role="presentation" class="active"><a href="#Section1" aria-controls="home" role="tab" data-toggle="tab">Section 1</a></li>
                            <li role="presentation"><a href="#Section2" aria-controls="profile" role="tab" data-toggle="tab">Section 2</a></li>
                            <li role="presentation"><a href="#Section3" aria-controls="messages" role="tab" data-toggle="tab">Section 3</a></li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content tabs">
                            <div role="tabpanel" class="tab-pane fade in active" id="Section1">
                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce semper, magna a ultricies volutpat, mi eros viverra massa, vitae consequat nisi justo in tortor. Proin accumsan felis ac felis dapibus, non iaculis mi varius.</p>
                            </div>
                            <div role="tabpanel" class="tab-pane fade" id="Section2">
                                <h3>SecKtion 2</h3>
                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce semper, magna a ultricies volutpat, mi eros viverra massa, vitae consequat nisi justo in tortor. Proin accumsan felis ac felis dapibus, non iaculis mi varius.</p>
                            </div>
                            <div role="tabpanel" class="tab-pane fade" id="Section3">
                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce semper, magna a ultricies volutpat, mi eros viverra massa, vitae consequat nisi justo in tortor. Proin accumsan felis ac felis dapibus, non iaculis mi varius.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
<script type="text/javascript" src="https://code.jquery.com/jquery-1.12.4.min.js"></script>

<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS" crossorigin="anonymous"></script>
</html>
