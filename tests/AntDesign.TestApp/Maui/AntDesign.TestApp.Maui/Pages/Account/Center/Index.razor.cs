﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.TestApp.Maui.Models;
using AntDesign.TestApp.Maui.Services;
using Microsoft.AspNetCore.Components;

namespace AntDesign.TestApp.Maui.Pages.Account.Center
{
    public partial class Index
    {
        private CurrentUser _currentUser = new CurrentUser
        {
            Geographic = new GeographicType {City = new TagType(), Province = new TagType()}
        };

        private IList<ListItemDataType> _fakeList = new List<ListItemDataType>();
        private bool _inputVisible;
        public string InputValue { get; set; }

        [Inject] public IProjectService ProjectService { get; set; }
        [Inject] public IUserService UserService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currentUser = await UserService.GetCurrentUserAsync();
            _fakeList = await ProjectService.GetFakeListAsync();
        }

        protected void ShowInput()
        {
            _inputVisible = true;
        }

        protected void HandleInputConfirm()
        {
            _inputVisible = false;
        }
    }
}