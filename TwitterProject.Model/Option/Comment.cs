﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterProject.Core.Entity;

namespace TwitterProject.Model.Option
{
    public class Comment:CoreEntity
    {
        public string Content { get; set; }
        public Guid AppUserID { get; set; }
        public virtual AppUser AppUser { get; set; }
        public Guid TweetID { get; set; }
        public virtual Tweet Tweet { get; set; }
    }
}
