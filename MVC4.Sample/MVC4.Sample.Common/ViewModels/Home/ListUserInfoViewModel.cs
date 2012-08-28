﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVC4.Sample.Common.Entities;

namespace MVC4.Sample.Common.ViewModels.Home
{
    [Serializable]
    public class ListUserInfoViewModel
    {
        public List<UserInfo> UserInfos { get; set; } 
    }
}
