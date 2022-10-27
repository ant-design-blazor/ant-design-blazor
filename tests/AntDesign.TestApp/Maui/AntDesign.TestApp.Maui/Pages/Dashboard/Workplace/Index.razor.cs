﻿using System.Threading.Tasks;
using AntDesign.TestApp.Maui.Models;
using AntDesign.TestApp.Maui.Services;
using Microsoft.AspNetCore.Components;

namespace AntDesign.TestApp.Maui.Pages.Dashboard.Workplace {
  public partial class Index {
    private readonly EditableLink[] _links =
        {
            new EditableLink {Title = "Operation 1", Href = ""},
            new EditableLink {Title = "Operation 2", Href = ""},
            new EditableLink {Title = "Operation 3", Href = ""},
            new EditableLink {Title = "Operation 4", Href = ""},
            new EditableLink {Title = "Operation 5", Href = ""},
            new EditableLink {Title = "Operation 6", Href = ""}
        };

    private ActivitiesType[] _activities = { };
    private NoticeType[] _projectNotice = { };

    [Inject] public IProjectService ProjectService { get; set; }

    protected override async Task OnInitializedAsync() {
      await base.OnInitializedAsync();
      _projectNotice = await ProjectService.GetProjectNoticeAsync();
      _activities = await ProjectService.GetActivitiesAsync();
    }
  }
}