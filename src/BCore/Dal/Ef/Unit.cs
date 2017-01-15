using BCore.Dal.BlogModels;
using BCore.Dal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCore.Dal.Ef
{
    public class Unit : IDisposable
    {
        private bool disposed = false;

        private BlogDbContext _db;        

        private IRepository<Post> _postRepository;
        public IRepository<Post> PostRepository
        {
            get
            {
                if (_postRepository == null)
                    _postRepository = new BlogRepository<Post>(_db);

                return _postRepository;
            }
        }

        private IRepository<Part> _partRepository;
        public IRepository<Part> PartRepository
        {
            get
            {
                if (_partRepository == null)
                    _partRepository = new BlogRepository<Part>(_db);

                return _partRepository;
            }
        }        

        private IRepository<Comment> _commentRepository;
        public IRepository<Comment> CommentRepository
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new BlogRepository<Comment>(_db);

                return _commentRepository;
            }
        }

        private IRepository<Hash> _hashRepository;
        public IRepository<Hash> HashRepository
        {
            get
            {
                if (_hashRepository == null)
                    _hashRepository = new BlogRepository<Hash>(_db);

                return _hashRepository;
            }
        }

        private IRepository<PostHash> _postHashRepository;
        public IRepository<PostHash> PostHashRepository
        {
            get
            {
                if (_postHashRepository == null)
                    _postHashRepository = new BlogRepository<PostHash>(_db);

                return _postHashRepository;
            }
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
