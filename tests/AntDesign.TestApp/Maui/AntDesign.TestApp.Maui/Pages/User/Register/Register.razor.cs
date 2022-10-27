﻿using System.ComponentModel.DataAnnotations;

namespace AntDesign.TestApp.Maui.Pages.User
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Captcha { get; set; }
    }

    public partial class Register
    {
        private readonly RegisterModel _user = new RegisterModel();

        public void Reg()
        {
        }
    }
}