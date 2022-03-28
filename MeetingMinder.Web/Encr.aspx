<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Encr.aspx.cs" Inherits="MeetingMinder.Web.Encr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

       <style type="text/css">
        /*.agenda h3
        {
            color: #666666 !important;
        }*/

        .agenda h3 a {
            color: Blue;
            margin-left: 10px;
        }

        .agenda a {
            color: Blue;
        }

        #accordion h2 {
            margin-left: -20px;
        }

        #accordion li {
            margin-left: 20px;
            list-style: none;
        }

        .ddrag > li {
            font-size: 20px;
        }

        h3 {
            font-size: 20px;
        }


        /*.inddrag {
        list-style:outside none lower-alpha !important;
        }*/
        @font-face {
            font-family: Rupee;
            src: url("fonts/Rupee_Foradian.eot") /*  IE 8 */;
        }

        @font-face {
            font-family: Rupee;
            src: url("fonts/Rupee_Foradian.ttf") /* CSS3 supported browsers */;
        }

        table {
            border: none !important;
        }

            table tr td, table tr th {
                border: none !important;
            }

        .tab tr td {
            border: 1px solid black !important;
        }

        .tab tr:last-child > td {
            border: 1px solid black !important;
        }

        .divPrint{
            color: black !important;
    font-family: times new roman;
    font-size: 14pt;
        }

         .divPrint table{
            color: black !important;
    font-family: times new roman;
    font-size: 14pt;
        }

            .subAgenda td{
            color: black !important;
    font-family: times new roman;
    font-size: 11pt !important;
        }

         b {
             font-weight:bold !important;
         }

         
.divPrint b {
    font-size: 14pt !important;
}
    </style>
 
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
     <table><tr><td>Text</td><td>
         <input type='text'  style='width:30px;visibility:hidden;' />
         <asp:TextBox ID="txtDomain" runat="server"></asp:TextBox></td></tr>
     <tr><td>  </td><td> </td></tr>
    </table>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
    </div>

                    <asp:Label CssClass="agenda" Font-Names="Rupee" ID="lblList" runat="server"></asp:Label>
                     
            <asp:Button ID="btnExportd" runat="server" Text="Export To Doc" OnClick="btnExportd_Click" />
                             
    </form>
</body>
</html>
