<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Planner.aspx.cs" Inherits="goPlan.Planner" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>goPlanner</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>

    <script>
        $(function () {
            $("#datepicker").datepicker();
        });
        $(function () {
            $("#datepicker2").datepicker();
        });

    </script>


</head>
<body>
    <form id="goHotelForm" runat="server">
        <div id="goHeader" class="container-fluid" style="text-align: center;">
            <h1><span style="color: crimson; padding-right: 0; margin: 0;">go</span> Hotel</h1>
        </div>
        <div class="container">
            <div class="jumbotron">
                Explore Place :
                <span style="padding-right: 2em;"></span>
                <asp:DropDownList ID="DropDownList1" runat="server" Height="19px" Width="208px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="Select Place"></asp:ListItem>
                </asp:DropDownList>

                <asp:Button ID="find" runat="server" CssClass="btn btn-info" Text="Search" OnClick="find_Click1" />
            </div>
            <div style="background-color: lightgoldenrodyellow;">

                <div>

                    <span style="padding-left: 4em;"></span>
                    <asp:DropDownList ID="DDL_hotel_names" runat="server" Height="17px" Width="167px" OnSelectedIndexChanged="DDL_hotel_names_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="Select hotel"></asp:ListItem>
                    </asp:DropDownList>
                    <span style="padding-left: 4em;"></span>
                    <asp:Button ID="HotelInfo" runat="server" CssClass="btn btn-info" Text="INFO" OnClick="HotelInfo_Click" />
                </div>
                <div>
                    <span style="padding-left: 16em;"></span>
                    Facilities:
                <span style="padding-left: 16em;"></span>
                    Near by Places:
                </div>
                <div>
                    <span style="padding-left: 16em;"></span>
                    <textarea id="TextArea2" title="Facilities" runat="server" cols="20" rows="4"></textarea>
                    <span style="padding-left: 16em;"></span>
                    <textarea id="TextArea3" title="NearByPlaces" runat="server" cols="20" rows="4"></textarea>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
