using BCore.Dal.BlogModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Dal.Ef
{
    public class Unit : IUoW, IDisposable
    {
        private bool disposed = false;

        private BlogDbContext _db;        

        private IRepository<Post> _postRepository;
        public IRepository<Post> PostRepository
        {
            get { return _postRepository ?? (_postRepository = new BlogRepository<Post>(_db)); }
        }

        private IRepository<Part> _partRepository;
        public IRepository<Part> PartRepository
        {
            get { return _partRepository ?? (_partRepository = new BlogRepository<Part>(_db)); }
        }        

        private IRepository<Comment> _commentRepository;
        public IRepository<Comment> CommentRepository
        {
            get { return _commentRepository ?? (_commentRepository = new BlogRepository<Comment>(_db)); }
        }

        private IRepository<Hash> _hashRepository;
        public IRepository<Hash> HashRepository
        {
            get { return _hashRepository ?? (_hashRepository = new BlogRepository<Hash>(_db)); }
        }

        private IRepository<PostHash> _postHashRepository;
        public IRepository<PostHash> PostHashRepository
        {
            get { return _postHashRepository ?? (_postHashRepository = new BlogRepository<PostHash>(_db)); }
        }

        public Unit(BlogDbContext db)
        {
            _db = db;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
