﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Required for Identity and OWIN
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace TeamProject1
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            //Redirect back to the Default Page
            Response.Redirect("~/Default.aspx");


        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            //create a new userStore and UserManager Objects
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            //create a new user object
            var user = new IdentityUser()
            {
                
                UserName = UserNameTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text,
                Email = EmailTextBox.Text
            };

            // create a new user in the database and store the results
            IdentityResult result = userManager.Create(user, PasswordTextBox.Text);

            //Check if successfullyregistered?

            if(result.Succeeded)
            {
                //authenticate and login our new user
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                //sign in the user
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);

                //Redirect to the Main Menu Page
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                //Display error in the AlerFlash Div
                StatusLabel.Text = result.Errors.FirstOrDefault();
                AlertFlash.Visible = true;
            }


        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {

            //Create new userStore and userManager objects
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            // search for and create a new user object
            var user = userManager.Find(UserNameTextBox.Text, PasswordTextBox.Text);

            //if a match is found for the user

            if (user != null)
            {
                // Authenticate and login our users
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                //sign in the user

                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);

                //Redirect the user to the mainMenu
                Response.Redirect("~/Default.aspx");

            }
            else // user if not found
            {
                StatusLabel.Text = "Invalid UserName or Password";
                AlertFlash.Visible = true;

            }


        }
    }
}