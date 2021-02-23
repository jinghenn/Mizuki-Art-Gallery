﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Mizuki.Master" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="Assignment.App_Pages.Order" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">

        
        <link href="https://fonts.googleapis.com/css?family=Raleway" rel="stylesheet">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.2/css/all.min.css" integrity="sha512-HK5fgLBL+xu6dm/Ii3z4xhlSUyZgTT9tuc/hSrtw6uzJOvgRr2a9jyxxT1ely+B+xFAmJKVSTbpM/CuL7qxO8w==" crossorigin="anonymous" />
        <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">

        <div class="container" id="product-section">
            <div class="row">
                <!-- Product picture -->
                <div class="col-md-6">
                    <div class="thumbnail">
                        <asp:Repeater ID="imageRepeater" runat="server">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <div class="gallery-item">
                                            <asp:ImageButton Width="100%" CssClass="gallery-image" ID="Image1" runat="server" ImageUrl='<%# Eval("URL") %>' Enabled="False" />
                                        </div>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>


                <div class="col-md-6">
                    <!--         <div class="row">
                        <div class="col-md-12">
                            <h4>Add to Wishlist <i class="far fa-star" style="size" id="btnWishlist"></i></h4>
                        </div>
                    </div>   -->

                    <div class="row">
                        <div class="col-md-12">
                            <asp:DetailsView ID="artworkName" runat="server" Height="50px" Width="388px" AutoGenerateRows="False" Font-Size="X-Large" GridLines="None">
                                <Fields>
                                    <asp:BoundField DataField="ArtworkName" SortExpression="ArtworkName" />
                                </Fields>
                            </asp:DetailsView>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:DetailsView ID="artworkDescription" runat="server" Height="50px" Width="220px" AutoGenerateRows="False" Font-Size="Medium" GridLines="None">
                                <Fields>
                                    <asp:BoundField DataField="Name" HeaderText="Artist Name :" SortExpression="Name" />
                                    <asp:BoundField DataField="StockQuantity" HeaderText="Quantity :" SortExpression="StockQuantity" />
                                    <asp:BoundField DataField="Price" DataFormatString="{0:C2}" HeaderText="Price :" SortExpression="Price" />
                                </Fields>
                            </asp:DetailsView>
                        </div>
                    </div>

                    <div class="row add-to-cart">
                        <div class="col-md-5 product-qty">
                            <br />
                            <br />
                            <br />
                            <asp:Button class="btn btn-default btn-lg btn-qty" Text="-" runat="server" ID="btnMinus" OnClick="btnMinus_Click" Style="width: 52px; height: 46px; border-radius: 0" PostBackUrl="~/App_Pages/Order.aspx" />
                            <asp:TextBox class="btn btn-default btn-lg btn-qty" ID="txtQuantity" runat="server" Style="width: 80px; height: 46px; border-radius: 0" Text="1" AutoPostBack="True"></asp:TextBox>
                            <asp:Button class="btn btn-default btn-lg btn-qty" Text="+" runat="server" ID="btnAdd" OnClick="btnAdd_Click" Style="width: 52px; height: 46px; border-radius: 0" PostBackUrl="~/App_Pages/Order.aspx" />
                            <br />
                            <br />
                            <asp:HiddenField ID="HiddenField1" runat="server" Visible="False" />
                            <br />
                            <br />
                            <br />

                        </div>
                    </div>
                    <!-- end row -->
                    <div class="row buttons">
                        <div class="col-md-6">
                            <asp:Button class="btn btn-warning btn-lg btn-brand btn-full-width" ID="btnAddToCart" Style="height: 50px" runat="server" Text="ADD TO CART" OnClick="btnAddToCart_Click" PostBackUrl="~/App_Pages/Order.aspx" />
                        </div>
                        <div class="col-md-6">
                            <asp:Button class="btn btn-primary btn-lg btn-brand btn-full-width" ID="btnBuyNow" Style="height: 50px" runat="server" Text="BUY NOW" PostBackUrl="~/App_Pages/BuyNow.aspx" OnClick="btnBuyNow_Click" />
                        </div>
                    </div>


                    <br />
                    <br />
                    <br />
                    <br />
                </div>
            </div>
        </div>


        <script
            src="https://code.jquery.com/jquery-2.2.2.min.js"
            integrity="sha256-36cp2Co+/62rEAAYHLmRCPIych47CvdM+uTBJwSzWjI="
            crossorigin="anonymous"></script>
        <script
            src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"
            integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS"
            crossorigin="anonymous">
        </script>

    </form>
</asp:Content>


