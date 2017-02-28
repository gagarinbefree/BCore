using BCore.Dal.BlogModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Dal
{
    public interface IUoW
    {
        IRepository<Post> PostRepository { get; }
        IRepository<Part> PartRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<Hash> HashRepository { get; }
        IRepository<PostHash> PostHashRepository { get; }

    }
}
