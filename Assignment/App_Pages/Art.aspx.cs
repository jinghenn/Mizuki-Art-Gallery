﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment.App_Pages
{
    public partial class Art : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Image_Click(object sender, System.EventArgs e)
        {
            ImageButton lnkRowSelection = (ImageButton)sender;
            string queryString = "~/App_Pages/ArtDetail.aspx?ArtworkID=" + lnkRowSelection.CommandArgument.ToString();
            Response.Redirect(queryString);
        }
    }
}