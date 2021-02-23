﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment.App_Pages
{
    public partial class Order : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String strOrderCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection orderCon = new SqlConnection(strOrderCon);

            orderCon.Open();
            String strSelectItem = "SELECT Artwork.ArtworkName FROM Artwork WHERE (Artwork.ArtworkID = @ArtworkID);";
            SqlCommand cmdSelectItem = new SqlCommand(strSelectItem, orderCon);
            cmdSelectItem.Parameters.AddWithValue("@ArtworkID", Session["artworkID"].ToString());
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmdSelectItem;
            DataTable dt = new DataTable();
            da.Fill(dt);
            artworkName.DataSource = cmdSelectItem.ExecuteReader();
            artworkName.DataBind();
            orderCon.Close();

            orderCon.Open();
            strSelectItem = "SELECT [User].Name, Artwork.Price, Artwork.StockQuantity FROM Artwork INNER JOIN [User] ON Artwork.Username = [User].Username WHERE (Artwork.ArtworkID = @ArtworkID);";
            cmdSelectItem = new SqlCommand(strSelectItem, orderCon);
            cmdSelectItem.Parameters.AddWithValue("@ArtworkID", Session["artworkID"].ToString());
            da = new SqlDataAdapter();
            da.SelectCommand = cmdSelectItem;
            dt = new DataTable();
            da.Fill(dt);
            artworkDescription.DataSource = cmdSelectItem.ExecuteReader();
            artworkDescription.DataBind();
            orderCon.Close();
            
            orderCon.Open();
            strSelectItem = "SELECT URL FROM Artwork WHERE (Artwork.ArtworkID = @ArtworkID);";
            cmdSelectItem = new SqlCommand(strSelectItem, orderCon);
            cmdSelectItem.Parameters.AddWithValue("@ArtworkID", Session["artworkID"].ToString());
            da = new SqlDataAdapter();
            da.SelectCommand = cmdSelectItem;
            dt = new DataTable();
            da.Fill(dt);
            imageRepeater.DataSource = cmdSelectItem.ExecuteReader();
            imageRepeater.DataBind();
            orderCon.Close();

            orderCon.Open();
            strSelectItem = "SELECT StockQuantity FROM Artwork WHERE (Artwork.ArtworkID = @ArtworkID);";
            cmdSelectItem = new SqlCommand(strSelectItem, orderCon);
            cmdSelectItem.Parameters.AddWithValue("@ArtworkID", Session["artworkID"].ToString());
            SqlDataReader dtrArtwork = cmdSelectItem.ExecuteReader();
            if (dtrArtwork.HasRows)
            {
                while (dtrArtwork.Read())
                {
                    Session["stockQuantity"] = dtrArtwork["StockQuantity"].ToString();
                }
            }
            else
            {
                Session["stockQuantity"] = "1";
            }
            orderCon.Close();

            if (Session["username"] != null)
            {
                orderCon.Open();
                strSelectItem = "SELECT * FROM CartDetails WHERE (ArtworkID = @ArtworkID) AND (Username = @Username);";
                cmdSelectItem = new SqlCommand(strSelectItem, orderCon);
                cmdSelectItem.Parameters.AddWithValue("@ArtworkID", Session["artworkID"].ToString());
                cmdSelectItem.Parameters.AddWithValue("@Username", Session["username"].ToString());
                dtrArtwork = cmdSelectItem.ExecuteReader();
                if (dtrArtwork.HasRows)
                {
                    while (dtrArtwork.Read())
                    {
                        Session["alreadyInCart"] = "true";
                    }
                }
                else
                {
                    Session["alreadyInCart"] = "false";
                }
                orderCon.Close();

                orderCon.Open();
                strSelectItem = "SELECT * FROM Favourite WHERE (ArtworkID = @ArtworkID) AND (Username = @Username);";
                cmdSelectItem = new SqlCommand(strSelectItem, orderCon);
                cmdSelectItem.Parameters.AddWithValue("@ArtworkID", Session["artworkID"].ToString());
                cmdSelectItem.Parameters.AddWithValue("@Username", Session["username"].ToString());
                dtrArtwork = cmdSelectItem.ExecuteReader();
                if (dtrArtwork.HasRows)
                {
                    while (dtrArtwork.Read())
                    {
                        btnAddToWishlist.Visible = false;
                        btnRemoveWishlist.Visible = true;
                    }
                }
                else
                {
                    btnRemoveWishlist.Visible = false;
                    btnAddToWishlist.Visible = true;
                }
                orderCon.Close();
            }
            else
            {
                btnAddToWishlist.Visible = true;
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                String strOrderCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection orderCon = new SqlConnection(strOrderCon);
                orderCon.Open();

                if (Session["alreadyInCart"].ToString().Equals("false"))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO CartDetails (Username, ArtworkID, Quantity) VALUES ('" + Session["username"].ToString() + "','" + Session["artworkID"].ToString() + "','" + txtQuantity.Text + "')", orderCon);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("UPDATE CartDetails SET Quantity =  Quantity + " + txtQuantity.Text + " WHERE ArtworkID = @ArtworkID AND Username = @Username;", orderCon);
                    cmd.Parameters.AddWithValue("@ArtworkID", Session["artworkID"].ToString());
                    cmd.Parameters.AddWithValue("@Username", Session["username"].ToString());
                    cmd.ExecuteNonQuery();
                }
                orderCon.Close();
                Response.Redirect("~/App_Pages/Art.aspx");
            }
            else
            {
                Response.Redirect("~/App_Pages/Login.aspx");
            }
        }

        protected void btnBuyNow_Click(object sender, EventArgs e)
        {
            Session["txtQuantity"] = txtQuantity.Text;
        }

        protected void btnMinus_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(txtQuantity.Text);
            if (a > 1)
            {
                a--;
                txtQuantity.Text = Convert.ToString(a);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int a = Convert.ToInt32(txtQuantity.Text);
            int maxQuantity = Convert.ToInt32(Session["stockQuantity"].ToString()); ; //gonna replace with maxQuantity from database
            if (a < maxQuantity)
            {
                a++;
                txtQuantity.Text = Convert.ToString(a);
            }
        }

        protected void btnAddToWishlist_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                String strOrderCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection orderCon = new SqlConnection(strOrderCon);
                orderCon.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Favourite (Username, ArtworkID) VALUES ('" + Session["username"].ToString() + "','" + Session["artworkID"].ToString() + "')", orderCon);
                cmd.ExecuteNonQuery();
                orderCon.Close();
                btnAddToWishlist.Visible = false;
                btnRemoveWishlist.Visible = true;
            }
            else
            {
                Response.Redirect("~/App_Pages/Login.aspx");
            }
        }

        protected void btnRemoveWishlist_Click(object sender, EventArgs e)
        {
            String strOrderCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection orderCon = new SqlConnection(strOrderCon);
            orderCon.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM Favourite WHERE ArtworkID = @ArtworkID AND Username = @Username;", orderCon);
            cmd.Parameters.AddWithValue("@ArtworkID", Session["artworkID"].ToString());
            cmd.Parameters.AddWithValue("@Username", Session["username"].ToString());
            cmd.ExecuteNonQuery();
            orderCon.Close();
            btnAddToWishlist.Visible = true;
            btnRemoveWishlist.Visible = false;
        }
    }
}