﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Mizuki.Master" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="Assignment.App_Pages.MainPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        .carousel-item img {
            max-height: 600px; /* input your desired height here */
            max-width: 1000px;
            margin: auto;
        }
        .container {
			max-width: 1250px;
			margin: 0 auto;
			padding: 0 80px 80px;
		}
		
		.heading {
			font-family: "Montserrat", Arial, sans-serif;
			font-size: 2rem;
			font-weight: 500;
			line-height: 1.5;
			text-align: center;
			padding: 3.5rem 0;
			color: #1a1a1a;
		}
		
		.heading span {
			display: block;
		}
		
		.gallery {
			display: flex;
			flex-wrap: wrap;
			/* Compensate for excess margin on outer gallery flex items */
			margin: 0 10 0 10;
		}
		
		.gallery-item {
			/* Minimum width of 24rem and grow to fit available space */
			flex: 1 0 10rem;
			/* Margin value should be half of grid-gap value as margins on flex items don't collapse */
			margin: 5px;
			box-shadow: 0.3rem 0.4rem 0.4rem rgba(0, 0, 0, 0.4);
			overflow: hidden;
		}
		
		.gallery-image {
			display: block;
			width: 100%;
			height: 100%;
			object-fit: cover;
			transition: transform 400ms ease-out;
		}
		
		.gallery-image:hover {
			transform: scale(1.15);

		}


    </style>
<script type="text/javascript">
function LoadDiv(url) {
    var img = new Image();
    var bcgDiv = document.getElementById("divBackground");
    var imgDiv = document.getElementById("divImage");
    var imgFull = document.getElementById("imgFull");
    var imgLoader = document.getElementById("imgLoader");
    imgLoader.style.display = "block";
    img.onload = function () {
        imgFull.src = img.src;
        imgFull.style.display = "block";
        imgLoader.style.display = "none";
   };
    img.src = url;
    var width = document.body.clientWidth;
    if (document.body.clientHeight > document.body.scrollHeight) {
        bcgDiv.style.height = document.body.clientHeight + "px";
    }
    else {
        bcgDiv.style.height = document.body.scrollHeight + "px";
    }
    imgDiv.style.left = (width - 650) / 2 + "px";
    imgDiv.style.top = "20px";
    bcgDiv.style.width = "100%";
 
    bcgDiv.style.display = "block";
    imgDiv.style.display = "block";
    return false;
</script>
}
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div id="carouselExampleCaptions" class="carousel slide " style="max-height: 600px; max-width: 1000px; margin: auto;" data-bs-ride="carousel">
  <div class="carousel-indicators">
    <button type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
    <button type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide-to="1" aria-label="Slide 2"></button>
    <button type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide-to="2" aria-label="Slide 3"></button>
  </div>
  <div class="carousel-inner">
    <div class="carousel-item active">
      <img src="https://images.alphacoders.com/173/thumb-1920-173483.jpg" class="d-block w-100" alt="...">
      <div class="carousel-caption d-none d-md-block">
        <h5>Live in the Moment </h5>
        <p>Forget yesterday.</p>
      </div>
    </div>
    <div class="carousel-item">
      <img src="https://static.rti.org.tw/assets/thumbnails/2019/10/01/26d27339a10153140d73771770621088.JPG" class="d-block w-100" alt="...">
      <div class="carousel-caption d-none d-md-block">
        <h5>Desire for Freedom</h5>
        <p>Take it Easy.</p>
      </div>
    </div>
    <div class="carousel-item">
      <img src="https://renewangtw.files.wordpress.com/2013/12/b9719-papas.jpg" class="d-block w-100" alt="...">
      <div class="carousel-caption d-none d-md-block">
        <h5>Follow Your Dreams</h5>
        <p>Have heart.</p>
      </div>
    </div>
  </div>
  <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleCaptions"  data-bs-slide="prev">
    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Previous</span>
  </button>
  <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleCaptions"  data-bs-slide="next">
    <span class="carousel-control-next-icon" aria-hidden="true"></span>
    <span class="visually-hidden">Next</span>
  </button>
</div>
<form runat="server">
  <h1 class="heading" style="color:black"><b><span>Mizuki Gallery</span></b></h1>
        <div class="container">
        <div class="gallery">
             <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Artwork]"></asp:SqlDataSource>
        <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1">
            <ItemTemplate> 
				<table>
					<tr><div class="gallery-item"><asp:ImageButton CssClass="gallery-image" ID="Image1"  runat="server" ImageUrl='<%# Eval("URL") %>' CommandArgument='<%# Eval("ArtworkID") %>' OnClick="Image_Click" /></div></tr>
					<asp:HiddenField ID="ArtworkID" runat="server" Value = '<%# Eval("ArtworkID") %>' />
				</table>
            </ItemTemplate>
        </asp:Repeater>
                </div>
        </div>


    </form>

</asp:Content>
