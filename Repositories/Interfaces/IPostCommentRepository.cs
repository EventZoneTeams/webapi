﻿using EventZone.Domain.Entities;
using EventZone.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Repositories.Interfaces
{
    public interface IPostCommentRepository : IGenericRepository<PostComment>
    {
    }
}