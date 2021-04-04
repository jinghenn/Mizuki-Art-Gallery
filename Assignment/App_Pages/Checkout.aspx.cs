﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace Assignment.App_Pages
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            lblSubtotal.Text = String.Format("{0:0.00}", Convert.ToDouble(Session["TotalPrice"].ToString()));
            lblTax.Text = (Convert.ToDouble(lblSubtotal.Text) * 0.06).ToString();
            lblTotal.Text = String.Format("{0:C2}",(Convert.ToDouble(lblTax.Text) + Convert.ToDouble(lblSubtotal.Text)));
        }

        

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/App_Pages/MainPage.aspx");
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {

            Double total = Convert.ToDouble(Convert.ToDouble(lblTax.Text) + Convert.ToDouble(lblSubtotal.Text));
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

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
            cmdAddOrder.Parameters.AddWithValue("@ContactNumber", txtContactNo.Text);
            cmdAddOrder.ExecuteNonQuery();
            con.Close();

            con.Open();
            SqlCommand cmdOrderID = new SqlCommand("SELECT OrderID FROM [dbo].[Order] ORDER BY OrderID DESC;", con);
            int orderID = Convert.ToInt32(cmdOrderID.ExecuteScalar());
            con.Close();

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
            con.Close();

            

            //rmb add delete query
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        //kyao de function
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string num = args.Value;

            if (num.Length != 14)
            {
                CustomValidator1.ErrorMessage = "Card Number should be 14 digits";
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

            if (rblPayment.Text == "Master")
            {
                if (num.First() != '5')
                {
                    CustomValidator1.ErrorMessage = "Master Card start with '5'";
                    args.IsValid = false;

                }
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}