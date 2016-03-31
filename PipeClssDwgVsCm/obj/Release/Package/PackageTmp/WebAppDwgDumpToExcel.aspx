<%@ Page AutoEventWireup="true" CodeBehind="WebAppDwgDumpToExcel.aspx.cs" Inherits="PipeClssDwgVsCm.WebFormDemo1" Language="C#" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Filter Drawing Data to Excel</title>
    <!-- 
    <link href="scripts/css/bootstrap.css" rel="stylesheet" />
       -->
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />

</head>
<body>

    <div class="blog-masthead">
        <div class="container">
            <nav class="blog-nav">
                <a class="blog-nav-item active" href="#">Home</a>
            </nav>
        </div>
    </div>



    <!-- was tag form  -->
    <div class="container">

        <div class="blog-header">
            <h1 class="blog-title">ADIM, Ca</h1>
            <p class="lead blog-description">Extract data from drawing title block to Excel <span class="glyphicon glyphicon-file"></span></p>
        </div>
      
        <form id="form1" runat="server">
            <div class="row" style="margin-top: 15px">
          
                    <div class="col-md-3 col-sm-3" style="background-color: #f3f3f3">
                        <asp:CheckBoxList ID="cblFieldSelect" runat="server"></asp:CheckBoxList>
                    </div>

                    <div class="col-md-3 col-sm-3" style="background-color: #f3f3f3">
                        <div class="row" style="margin-bottom: 5px; margin-top: 5px;">
                            <asp:DropDownList ID="ddlFileList" CssClass="panel-default" runat="server">
                            </asp:DropDownList>
                            <asp:Label ID="Label2" runat="server" CssClass="h4" Text="Choose file..."></asp:Label>
                        </div>
                        <div class="row">
                            <asp:Button ID="btnGetData" runat="server" class="btn btn-primary" Style="margin-top: 10px; margin-bottom: 5px;" Text="Get Data " OnClick="btnGetData_Click" />
                        </div>
                        <div class="row">
                            <asp:Button ID="btnExportToExcel" runat="server" class="btn btn-info" Style="margin-top: 5px; margin-bottom: 5px;" OnClick="btnExportToExcel_Click" Text="Export to Excel"/>
                        </div>
                        <div class="row">
                            <asp:Button ID="btnCheckAll" runat="server" class="btn btn-default" Style="margin-top: 5px; margin-bottom: 5px;" OnClick="btnCheckAll_Click" Text="Check All" />
                        </div>
                        <div class="row">
                            <asp:Button ID="btnUncheckAll" runat="server" class="btn btn-default" Style="margin-top: 5px; margin-bottom: 5px;" OnClick="btnUncheckAll_Click" Text="Uncheck All" />
                        </div>
                    </div>
                
                <div class="row">
                    <div class="col-md-6 col-sm-6">
                        <asp:Panel ID="Panel2" runat="server" class="panel-info" Style="border-style: solid; border-width: 1px; padding: 1px 4px; z-index: 1; left: 479px; top: 72px; font-size: large (16 pt); font-family: Arial; color: #8A6D3B; margin-top: 5px;" BorderColor="blue" CssClass="panel panel-info" Height="160px" Width="403px">

                            <div class="panel-heading">User Guide <span class="glyphicon glyphicon-info-sign"></span></div>
                            <div class="panel-body">
                                1. <strong>Choose file... </strong>
                                <br />
                                2. Review <i class="glyphicon glyphicon-ok"></i><strong> checkbox list</strong>, &#39;checked&#39; to retrieve data<br />
                                3. Click <strong>Get Data</strong> to preview<br />
                                4. Click <strong>Export to Excel </strong>to Open/Save in Excel
                            </div>

                        </asp:Panel>
                    </div>
                </div>

            </div>
            <!--/Row Checkbox, Buttons, note Panel-->

            <!--
            <div class="row" style="margin-top: 15px;">
                <asp:FileUpload ID="FileUpload1" runat="server" Style="margin-top: 5px; margin-bottom: 5px;" />
                <asp:TextBox ID="tbFileName" runat="server" EnableViewState="True" Visible="False" BorderStyle="Solid"></asp:TextBox>
            </div>

                -->

            <footer class="blog-footer" style="margin-top: 15px;">
                <p>
                    &copy;
                <asp:Label ID="CurrentTime" runat="server" /> - AdimRic841
                </p>
            </footer>
            <!-- /.footer //comment example -->

            <div class="row" style="margin-top: 15px;">
                <div class="col-md-12 col-sm-12">
                    <div class="table table-striped table-hover table-bordered table-condensed">
                        <asp:GridView ID="dataGridView1" runat="server" CssClass="table-striped">
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </form>
    </div>
    <!-- /container-->






    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <script src="scripts/ai.0.15.0-build58334.js"></script>

</body>
</html>

