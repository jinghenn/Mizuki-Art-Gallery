﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment.App_Pages
{
    public partial class Checkout2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                String strOrderCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection orderCon = new SqlConnection(strOrderCon);

                orderCon.Open();
                String strSelectItem = "SELECT Cart.ArtworkID, Art.ArtworkName, Art.Price, Art.URL, Cart.Quantity, Cart.Quantity * Art.Price AS TotalPrice FROM Artwork Art, CartDetails Cart WHERE Art.ArtworkID = Cart.ArtworkID AND Cart.Username= @Username;";
                SqlCommand cmdSelectItem = new SqlCommand(strSelectItem, orderCon);
                cmdSelectItem.Parameters.AddWithValue("@Username", Session["Username"].ToString());
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmdSelectItem;
                DataTable dt = new DataTable();
                da.Fill(dt);
                Repeater1.DataSource = cmdSelectItem.ExecuteReader();
                Repeater1.DataBind();
                orderCon.Close();

                orderCon.Open();
                String strCartTotal = "Select Cart.Quantity * Art.Price AS TotalPrice from Artwork Art, CartDetails Cart, [User] u Where Cart.Username = u.Username and Cart.Username = @username and Art.ArtworkID = Cart.ArtworkID; ";
                SqlCommand cmdCartTotal = new SqlCommand(strCartTotal, orderCon);
                cmdCartTotal.Parameters.AddWithValue("@username", Session["username"].ToString());
                SqlDataReader dr = cmdCartTotal.ExecuteReader();
                decimal Total = Convert.ToDecimal(0.0);
                while (dr.Read())
                {
                    Total = Total + Convert.ToDecimal(dr["TotalPrice"].ToString());
                }
                orderCon.Close();

                lblSubtotal.Text = String.Format("{0:0.00}", Total);
                lblTax.Text = String.Format("{0:0.00}", (Convert.ToDouble(lblSubtotal.Text) * 0.06));
                lblTotal.Text = String.Format("RM {0:0.00}", (Convert.ToDouble(lblTax.Text) + Convert.ToDouble(lblSubtotal.Text)));
            }
            else
            {
                Response.Redirect("~/App_Pages/Login.aspx");
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string num = args.Value;

            if (txtCardNumber.Text.Length != 16)
            {
               CustomValidator1.ErrorMessage = "Card Number should be 16 digits";
                args.IsValid = false;
            }

            if (rblPayment.Text == "Visa")
            {
                if (num.First() != '4')
                {
                    CustomValidator1.ErrorMessage = "Visa Card start with '4'";
                    args.IsValid = false;
                }
            }
            else if (rblPayment.Text == "Master")
            {
                if (num.First() != '5')
                {
                    CustomValidator1.ErrorMessage = "Master Card start with '5'";
                    args.IsValid = false;

                }
            }
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int month = Convert.ToInt32(args.Value);

            if (Convert.ToInt32(ddlYear.Text) == DateTime.Now.Year)
            {
                if (month < DateTime.Now.Month)
                {
                    args.IsValid = false;
                    CustomValidator3.ErrorMessage = "Expiration Date has passed";
                }
            }
            else if (Convert.ToInt32(ddlYear.Text) < DateTime.Now.Year)
            {
                args.IsValid = false;
                CustomValidator3.ErrorMessage = "Expiration Date has passed";
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/App_Pages/MainPage.aspx");
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Double total = Convert.ToDouble(Convert.ToDouble(lblTax.Text) + Convert.ToDouble(lblSubtotal.Text));
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

                //Insert into Order table
                con.Open();
                SqlCommand cmdAddOrder = new SqlCommand("INSERT INTO [dbo].[Order] VALUES (@Username, @Date, @Total, @PaymentType, @CardNumber, @DeliveryAddress, @RecipientName, @EmailAddress, @ContactNumber);", con);
                cmdAddOrder.Parameters.AddWithValue("@Username", Session["Username"].ToString());
                cmdAddOrder.Parameters.AddWithValue("@Date", DateTime.Now.ToString("dd-MM-yyyy"));
                cmdAddOrder.Parameters.AddWithValue("@Total", total);
                cmdAddOrder.Parameters.AddWithValue("@PaymentType", rblPayment.Text);
                cmdAddOrder.Parameters.AddWithValue("@CardNumber", txtCardNumber.Text);
                cmdAddOrder.Parameters.AddWithValue("@DeliveryAddress", txtDeliveryAddress.Text + " " + txtZipCode.Text + " " + txtCity.Text + " " + ddlState.Text);
                cmdAddOrder.Parameters.AddWithValue("@RecipientName", txtName.Text);
                cmdAddOrder.Parameters.AddWithValue("@EmailAddress", txtEmail.Text);
                cmdAddOrder.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text);
                cmdAddOrder.ExecuteNonQuery();
                con.Close();

                //get latest OrderID from Order table
                con.Open();
                SqlCommand cmdgetOrderID = new SqlCommand("SELECT OrderID FROM [dbo].[Order] ORDER BY OrderID DESC;", con);
                int orderID = Convert.ToInt32(cmdgetOrderID.ExecuteScalar());
                con.Close();

                //Insert into OrderDetails table
                con.Open();
                SqlCommand cmdCartDetails = new SqlCommand("SELECT CartDetails.ArtworkID, CartDetails.Quantity FROM CartDetails WHERE CartDetails.Username= @Username;", con);
                cmdCartDetails.Parameters.AddWithValue("@Username", Session["Username"].ToString());
                SqlDataReader cart = cmdCartDetails.ExecuteReader();
                while (cart.Read())
                {
                    SqlCommand cmdAddOrderDetails = new SqlCommand("INSERT INTO OrderDetails VALUES (@OrderID, @ArtworkID, @Quantity);", con);
                    cmdAddOrderDetails.Parameters.AddWithValue("@OrderID", orderID);
                    cmdAddOrderDetails.Parameters.AddWithValue("@ArtworkID", cart["ArtworkID"].ToString());
                    cmdAddOrderDetails.Parameters.AddWithValue("@Quantity", cart["Quantity"].ToString());
                    cmdAddOrderDetails.ExecuteNonQuery();
                }
                cart.Close();
                con.Close();

                //Reduce Stock
                con.Open();
                SqlCommand cmdGetArtworkCount = new SqlCommand("SELECT * FROM OrderDetails WHERE OrderID = @OrderID", con);
                cmdGetArtworkCount.Parameters.AddWithValue("@OrderID", orderID);
                SqlDataReader rows = cmdCartDetails.ExecuteReader();
                while (rows.Read())
                {
                    SqlCommand cmdReduceStock = new SqlCommand("UPDATE Artwork SET StockQuantity = StockQuantity - (SELECT Quantity FROM OrderDetails WHERE OrderID = @OrderID AND ArtworkID = @ArtworkID) WHERE ArtworkID = @ArtworkID", con);
                    cmdReduceStock.Parameters.AddWithValue("@OrderID", orderID);
                    cmdReduceStock.Parameters.AddWithValue("@ArtworkID", rows["ArtworkID"].ToString());
                    cmdReduceStock.ExecuteNonQuery();
                }
                rows.Close();
                con.Close();

                //clear cart
                con.Open();
                SqlCommand cmdClearCart = new SqlCommand("DELETE FROM CartDetails WHERE Username = @Username", con);
                cmdClearCart.Parameters.AddWithValue("@Username", Session["Username"].ToString());
                cmdClearCart.ExecuteNonQuery();
                con.Close();

                //update all artwork quantity in the cart if exceed maximum amount
                con.Open();
                SqlCommand cmdgetArtworkInOrder = new SqlCommand("SELECT ArtworkID FROM OrderDetails WHERE OrderID = @OrderID", con);
                cmdgetArtworkInOrder.Parameters.AddWithValue("@OrderID", orderID);
                SqlDataReader artworkInOrder = cmdgetArtworkInOrder.ExecuteReader();
                int stockQuantity;
                while (artworkInOrder.Read())
                {
                    //get the stock quantity for a specific artwork
                    SqlCommand cmdgetStockQuantity = new SqlCommand("SELECT StockQuantity FROM Artwork WHERE ArtworkID = @ArtworkID", con);
                    cmdgetStockQuantity.Parameters.AddWithValue("@ArtworkID", artworkInOrder["ArtworkID"].ToString());
                    stockQuantity = Convert.ToInt32(cmdgetStockQuantity.ExecuteScalar());

                    if (stockQuantity == 0)
                    {
                        //remove the specific artwork from all carts where stock quantity = 0
                        SqlCommand cmdRemoveFromAllCart = new SqlCommand("DELETE FROM CartDetails WHERE ArtworkID=@ArtworkID", con);
                        cmdRemoveFromAllCart.Parameters.AddWithValue("@ArtworkID", artworkInOrder["ArtworkID"].ToString());
                        cmdRemoveFromAllCart.ExecuteNonQuery();
                    }
                    else
                    {
                        //update the cart quantity of the specific artwork if > stock quantity
                        SqlCommand cmdUpdateAllCart = new SqlCommand("UPDATE CartDetails SET Quantity = @StockQuantity WHERE ArtworkID=@ArtworkID AND Quantity > @StockQuantity", con);
                        cmdUpdateAllCart.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                        cmdUpdateAllCart.Parameters.AddWithValue("@ArtworkID", artworkInOrder["ArtworkID"].ToString());
                        cmdUpdateAllCart.ExecuteNonQuery();
                    }
                }
                artworkInOrder.Close();
                con.Close();

                string queryString = "~/App_Pages/DigitalReceipt.aspx?OrderID=" + orderID;
                Response.Redirect(queryString);
            }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        
    }
}